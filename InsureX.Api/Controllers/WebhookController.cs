using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;
using IAPR_Data.Services;

namespace InsureX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebhookSignatureValidator _validator;
        private readonly WebhookEventQueue _queue;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(
            ApplicationDbContext db, 
            IWebhookSignatureValidator validator,
            WebhookEventQueue queue,
            ILogger<WebhookController> logger)
        {
            _db = db;
            _validator = validator;
            _queue = queue;
            _logger = logger;
        }

        /// <summary>
        /// Ingests a signed webhook event from an external insurer system.
        /// </summary>
        [HttpPost("receive")]
        [AllowAnonymous] // Verification via HMAC Signature
        public async Task<IActionResult> Receive()
        {
            var source = Request.Headers["X-InsureX-Source"].ToString();
            var signature = Request.Headers["X-InsureX-Signature"].ToString();
            var timestamp = Request.Headers["X-InsureX-Timestamp"].ToString();
            var eventId = Request.Headers["X-InsureX-Event-Id"].ToString();

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(eventId))
            {
                return BadRequest("Missing required integration headers.");
            }

            // 1. Resolve Partner Config
            var config = await _db.PartnerWebhookConfigs
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(c => c.IsActive && c.Tenant.DomainKey == source);

            if (config == null)
            {
                _logger.LogWarning("Received webhook from unknown or inactive source: {Source}", source);
                return Unauthorized("Invalid partner source.");
            }

            // 2. Validate Timestamp (Anti-Replay)
            if (!_validator.IsTimestampValid(timestamp))
            {
                return Unauthorized("Stale request detected.");
            }

            // 3. Read Body & Validate Signature
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var bodyString = await reader.ReadToEndAsync();
            var bodyBytes = Encoding.UTF8.GetBytes(bodyString);

            if (!_validator.ValidateSignature(bodyBytes, signature, config.Secret))
            {
                _logger.LogWarning("HMAC Signature mismatch for source: {Source}", source);
                return Unauthorized("Signature verification failed.");
            }

            // 4. Check Identpotency
            var existing = await _db.WebhookEvents.FirstOrDefaultAsync(e => e.EventId == eventId && e.Source == source);
            if (existing != null)
            {
                return Ok(new { Message = "Event already processed (Idempotent)." });
            }

            // 5. Enqueue for Processing
            var webhookEvent = new WebhookEvent
            {
                EventId = eventId,
                Source = source,
                EventType = Request.Headers["X-InsureX-Event-Type"].ToString() ?? "unknown",
                Payload = bodyString,
                ReceivedSignature = signature,
                TenantId = config.TenantId
            };

            _db.WebhookEvents.Add(webhookEvent);
            await _db.SaveChangesAsync();

            _queue.Enqueue(new WebhookEventMessage
            {
                EventId = webhookEvent.EventId,
                Source = webhookEvent.Source,
                EventType = webhookEvent.EventType,
                Payload = webhookEvent.Payload,
                TenantId = webhookEvent.TenantId
            });

            return Accepted(new { EventId = webhookEvent.Id, Status = "Enqueued" });
        }
    }
}
