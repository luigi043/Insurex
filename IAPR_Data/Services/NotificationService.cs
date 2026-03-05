using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Secure, singleton SMTP delivery service.
    ///
    /// Improvements over the legacy Notification_Provider:
    /// - Single shared SmtpClient (no repeated construction/disposal per call).
    /// - Template caching (reads HTML files from disk once, not on every email).
    /// - Built-in retry with exponential back-off (up to MaxRetries attempts).
    /// - No BCC/CC blast patterns — every Send targets exactly one recipient.
    /// - Credentials sourced exclusively from ConfigurationManager (never hardcoded).
    ///
    /// Call <see cref="Instance"/> anywhere; no startup registration needed (lazy init).
    /// </summary>
    public sealed class NotificationService
    {
        private static readonly Lazy<NotificationService> _instance =
            new Lazy<NotificationService>(() => new NotificationService());

        public static NotificationService Instance => _instance.Value;

        private const int MaxRetries     = 3;
        private const int RetryDelayMs   = 2000;

        // Thread-safe template cache: template key → HTML content
        private readonly ConcurrentDictionary<string, string> _templateCache =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Shared SmtpClient — one instance for the app lifetime
        private readonly SmtpClient _smtp;
        private readonly string _fromAddress;
        private readonly string _fromDisplayName;

        private NotificationService()
        {
            _fromAddress     = ConfigurationManager.AppSettings["Support_Email_Address"]
                           ?? "notifications@insurex.com";
            _fromDisplayName = ConfigurationManager.AppSettings["Support_Email_DisplayName"]
                           ?? "InsureX Notifications";

            var host = ConfigurationManager.AppSettings["SMTPServer"] ?? "localhost";
            var port = int.TryParse(ConfigurationManager.AppSettings["SMTPPort"], out int p) ? p : 25;
            var user = ConfigurationManager.AppSettings["SMTPServerAccount"] ?? "";
            var pass = ConfigurationManager.AppSettings["SMTPServerPassword"] ?? "";

            _smtp = new SmtpClient(host, port)
            {
                EnableSsl   = true,
                Credentials = new NetworkCredential(user, pass)
            };
        }

        // ------------------------------------------------------------------
        // Primary send method
        // ------------------------------------------------------------------

        /// <summary>
        /// Sends a single email to <paramref name="toAddress"/>.
        /// Performs template token replacement and retries on transient SMTP failures.
        /// </summary>
        /// <param name="toAddress">Single recipient email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="bodyOrTemplateKey">
        /// Either a raw HTML body string, or a named template key (e.g., "NewUser").
        /// If a template exists on disk, it is used; otherwise the string is used verbatim.
        /// </param>
        /// <param name="tokens">Optional token replacements applied to the body (e.g., {0}→name).</param>
        /// <returns>True if the mail was sent successfully.</returns>
        public bool Send(string toAddress, string subject, string bodyOrTemplateKey,
                         params string[] tokens)
        {
            if (string.IsNullOrWhiteSpace(toAddress))
            {
                System.Diagnostics.Trace.TraceWarning("[NotificationService] Send called with empty toAddress — skipped.");
                return false;
            }

            var body = ResolveBody(bodyOrTemplateKey, tokens);

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    using (var msg = BuildMessage(toAddress, subject, body))
                    {
                        _smtp.Send(msg);
                    }

                    System.Diagnostics.Trace.TraceInformation(
                        $"[NotificationService] Sent '{subject}' → {toAddress}");
                    return true;
                }
                catch (SmtpException ex)
                {
                    // Retry on any transient SMTP error
                    System.Diagnostics.Trace.TraceWarning(
                        $"[NotificationService] SMTP error (attempt {attempt}/{MaxRetries}): {ex.StatusCode} — {ex.Message}");

                    if (attempt >= MaxRetries) throw; // exhausted retries — rethrow to outer catch
                    System.Threading.Thread.Sleep(RetryDelayMs * attempt);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(
                        $"[NotificationService] Non-retryable error sending to {toAddress}: {ex.Message}");
                    return false;
                }
            }

            System.Diagnostics.Trace.TraceError(
                $"[NotificationService] Exhausted {MaxRetries} retries sending '{subject}' to {toAddress}.");
            return false;
        }

        /// <summary>Sends to the support inbox — routes through <see cref="Send"/>.</summary>
        public bool SendToSupport(string subject, string body)
        {
            var supportEmail = ConfigurationManager.AppSettings["Support_Email_Address"]
                            ?? _fromAddress;
            return Send(supportEmail, subject, body);
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        private MailMessage BuildMessage(string toAddress, string subject, string htmlBody)
        {
            var msg = new MailMessage
            {
                From       = new MailAddress(_fromAddress, _fromDisplayName),
                Subject    = subject,
                Body       = htmlBody,
                IsBodyHtml = true,
                Priority   = MailPriority.Normal,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
            msg.To.Add(toAddress);
            return msg;
        }

        private string ResolveBody(string bodyOrKey, string[] tokens)
        {
            var body = LoadTemplate(bodyOrKey);

            if (tokens == null || tokens.Length == 0)
                return body;

            // Apply positional token replacement: {0}, {1}, ...
            for (int i = 0; i < tokens.Length; i++)
                body = body.Replace("{" + i + "}", tokens[i] ?? string.Empty);

            return body;
        }

        private string LoadTemplate(string keyOrBody)
        {
            // Already-in-cache short-circuit
            if (_templateCache.TryGetValue(keyOrBody, out var cached))
                return cached;

            // Try to resolve as a template name — look in MailTemplates folder
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(basePath))
                {
                    var filePath = System.IO.Path.Combine(basePath, "MailTemplates", keyOrBody + ".html");
                    if (System.IO.File.Exists(filePath))
                    {
                        var content = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
                        _templateCache[keyOrBody] = content;
                        return content;
                    }
                }
            }
            catch { /* hosting env not available (unit test context) — fall through */ }

            // No template found — treat the input as a literal HTML body
            return keyOrBody;
        }
    }
}







