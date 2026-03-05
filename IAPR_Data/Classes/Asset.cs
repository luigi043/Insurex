using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAPR_Data.Classes
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant? Tenant { get; set; }

        [Required]
        [StringLength(100)]
        public string AssetType { get; set; }

        [Required]
        [StringLength(100)]
        public string AssetIdentifier { get; set; }

        [StringLength(100)]
        public string RegistrationNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinancedAmount { get; set; }

        [StringLength(200)]
        public string BorrowerReference { get; set; }

        public DateTime LoanStartDate { get; set; }
        public DateTime LoanEndDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ComplianceStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public Asset()
        {
            CreatedAt = DateTime.UtcNow;
            Status = "Active";
            ComplianceStatus = "Unknown";
        }
    }
}







