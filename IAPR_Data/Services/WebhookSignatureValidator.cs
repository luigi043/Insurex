using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IAPR_Data.Classes.Webhook;

namespace IAPR_Data.Services
{
    public interface IWebhookSignatureValidator
    {
        bool ValidateSignature(byte[] body, string signature, string secret);
        bool IsTimestampValid(string timestampHeader, int maxAgeMinutes = 5);
    }

    public class WebhookSignatureValidator : IWebhookSignatureValidator
    {
        private readonly ILogger<WebhookSignatureValidator> _logger;

        public WebhookSignatureValidator(ILogger<WebhookSignatureValidator> logger)
        {
            _logger = logger;
        }

        public bool ValidateSignature(byte[] body, string signature, string secret)
        {
            return HmacValidator.IsValid(body, signature, secret);
        }

        public bool IsTimestampValid(string timestampHeader, int maxAgeMinutes = 5)
        {
            if (string.IsNullOrEmpty(timestampHeader)) return false;

            if (!long.TryParse(timestampHeader, out long unixTimestamp))
                return false;

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
            var drift = DateTime.UtcNow - requestTime;

            if (Math.Abs(drift.TotalMinutes) > maxAgeMinutes)
            {
                _logger.LogWarning("Webhook timestamp drift too high: {Drift} minutes", drift.TotalMinutes);
                return false;
            }

            return true;
        }
    }
}
