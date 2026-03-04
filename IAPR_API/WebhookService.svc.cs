using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;
using IAPR_Data.Services;
using Newtonsoft.Json;

namespace IAPR_API
{
    [ServiceContract]
    public interface IWebhookService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/insurers/{source}/events",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        WebhookResponse ReceiveEvent(string source, Stream body);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebhookService : IWebhookService
    {
        // Shared HMAC secret is loaded from Web.config AppSettings["WebhookHmacSecret"]
        private static string GetHmacSecret(string source)
        {
            // In production, per-insurer secrets would be stored in Key Vault or the DB.
            // For now, use a single shared secret from config.
            return System.Configuration.ConfigurationManager.AppSettings["WebhookHmacSecret"] ?? "CHANGEME";
        }

        public WebhookResponse ReceiveEvent(string source, Stream body)
        {
            var ctx = WebOperationContext.Current;

            try
            {
                // 1. Read raw body bytes for HMAC computation (must happen before JSON parse)
                byte[] rawBody;
                using (var ms = new MemoryStream())
                {
                    body.CopyTo(ms);
                    rawBody = ms.ToArray();
                }

                var payload = Encoding.UTF8.GetString(rawBody);

                // 2. Extract security headers
                var incomingRequest = ctx?.IncomingRequest;
                var signature = incomingRequest?.Headers["X-Signature-SHA256"]
                             ?? incomingRequest?.Headers["X-Hub-Signature-256"]
                             ?? incomingRequest?.Headers["X-Signature"]
                             ?? string.Empty;

                // 3. Replay protection: read X-Event-Timestamp and reject if too old (>5 min)
                var timestampHeader = incomingRequest?.Headers["X-Event-Timestamp"] ?? "";
                if (DateTime.TryParse(timestampHeader, out DateTime eventTime))
                {
                    var age = DateTime.UtcNow - eventTime.ToUniversalTime();
                    if (Math.Abs(age.TotalMinutes) > 5)
                    {
                        ctx.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        return new WebhookResponse { Success = false, Message = "Request timestamp too old (replay protection)." };
                    }
                }

                // 4. Extract Event-ID for idempotency
                var eventId = incomingRequest?.Headers["X-Event-ID"]
                           ?? incomingRequest?.Headers["X-Request-ID"]
                           ?? Guid.NewGuid().ToString();

                // 5. HMAC Signature Validation
                var secret = GetHmacSecret(source);
                if (!string.IsNullOrEmpty(signature) && !HmacValidator.IsValid(rawBody, signature, secret))
                {
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    return new WebhookResponse { Success = false, Message = "Invalid HMAC signature." };
                }

                // 6. Determine event type from payload
                string eventType = "unknown";
                try
                {
                    var parsed = JsonConvert.DeserializeObject<dynamic>(payload);
                    eventType = parsed?.eventType?.ToString() ?? parsed?.type?.ToString() ?? "unknown";
                }
                catch { /* ignore parse errors for event type extraction */ }

                // 7. Idempotency check + persist the event
                using (var db = ApplicationDbContext.Create())
                {
                    // Check if this EventId was already processed
                    var existing = db.WebhookEvents.Find(eventId);
                    if (existing == null)
                    {
                        // Look up using SQL since Find only works by PK
                        existing = db.WebhookEvents
                                     .FirstOrDefault(w => w.EventId == eventId);
                    }

                    if (existing != null && existing.Status == "Processed")
                    {
                        ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                        return new WebhookResponse { Success = true, Message = "Event already processed (idempotent).", EventId = eventId };
                    }

                    // Persist the event for audit and async processing
                    var webhookEvent = new WebhookEvent
                    {
                        EventId = eventId,
                        Source = source,
                        EventType = eventType,
                        Payload = payload,
                        ReceivedSignature = signature,
                        Status = "Pending",
                        TenantId = TenantContext.Current
                    };

                    db.WebhookEvents.Add(webhookEvent);
                    db.SaveChanges();

                    // Dispatch to the in-process queue for async ComplianceEngine processing
                    WebhookEventQueue.Instance.Enqueue(new WebhookEventMessage
                    {
                        EventId   = eventId,
                        Source    = source,
                        EventType = eventType,
                        Payload   = payload,
                        TenantId  = TenantContext.Current,
                        ReceivedAt = DateTime.UtcNow
                    });
                }

                ctx.OutgoingResponse.StatusCode = HttpStatusCode.Accepted;
                return new WebhookResponse { Success = true, Message = "Event accepted for processing.", EventId = eventId };
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                System.Diagnostics.Trace.TraceError("WebhookService error: " + ex.Message);
                return new WebhookResponse { Success = false, Message = "Internal server error." };
            }
        }
    }

    public class WebhookResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string EventId { get; set; }
    }
}
