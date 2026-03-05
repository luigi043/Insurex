using System;
using System.ComponentModel.DataAnnotations;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Outbox pattern message record.
    /// Written inside the same DB transaction as the domain state change,
    /// then read and published by the <see cref="OutboxPublisher"/> background process.
    /// This guarantees exactly-once delivery even if the process crashes after saving to DB
    /// but before publishing to an external queue / bus.
    /// </summary>
    public class OutboxMessage
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Logical message type (e.g., "ComplianceOutcomeEmitted").</summary>
        [Required]
        [StringLength(200)]
        public string MessageType { get; set; }

        /// <summary>JSON-serialized message payload.</summary>
        [Required]
        public string Payload { get; set; }

        /// <summary>Correlation / trace ID linking this message to the originating event.</summary>
        [StringLength(100)]
        public string CorrelationId { get; set; }

        /// <summary>The tenant this message belongs to.</summary>
        public int? TenantId { get; set; }

        /// <summary>UTC timestamp the message was inserted.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC timestamp the message was successfully published. Null = not yet published.</summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>Number of publish attempts made (for retry tracking).</summary>
        public int AttemptCount { get; set; }

        /// <summary>Last error if a publish attempt failed.</summary>
        [StringLength(2000)]
        public string LastError { get; set; }

        public OutboxMessage()
        {
            CreatedAt = DateTime.UtcNow;
            AttemptCount = 0;
        }
    }
}







