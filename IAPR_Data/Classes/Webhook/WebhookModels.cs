using System;
using System.ComponentModel.DataAnnotations;

namespace IAPR_Data.Classes.Webhook
{
    /// <summary>
    /// Represents a webhook event received from an external insurer system.
    /// Stored for idempotency checking and audit purposes.
    /// </summary>
    public class WebhookEvent
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Unique event ID from the insurer (used for idempotency)</summary>
        [Required]
        [StringLength(200)]
        public string EventId { get; set; }

        /// <summary>Source insurer identifier</summary>
        [StringLength(100)]
        public string Source { get; set; }

        /// <summary>Event type e.g. "policy.created", "claim.submitted"</summary>
        [Required]
        [StringLength(100)]
        public string EventType { get; set; }

        /// <summary>Raw JSON payload from the webhook request body</summary>
        public string Payload { get; set; }

        /// <summary>HMAC-SHA256 signature received in the X-Signature header</summary>
        [StringLength(500)]
        public string ReceivedSignature { get; set; }

        /// <summary>Processing state: Pending, Processed, Failed</summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        /// <summary>Timestamp the webhook arrived on our server</summary>
        public DateTime ReceivedAt { get; set; }

        /// <summary>Timestamp when processing completed</summary>
        public DateTime? ProcessedAt { get; set; }

        /// <summary>Error message if processing failed</summary>
        public string ProcessingError { get; set; }

        /// <summary>TenantId the event is scoped to (optional, resolved from Source)</summary>
        public int? TenantId { get; set; }

        public WebhookEvent()
        {
            ReceivedAt = DateTime.UtcNow;
            Status = "Pending";
        }
    }
}
