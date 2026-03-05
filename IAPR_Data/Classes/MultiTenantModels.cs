using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAPR_Data.Classes
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string DomainKey { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        // Navigation property for subsidiary organizations
        public virtual ICollection<Organization> Organizations { get; set; }

        public Tenant()
        {
            Organizations = new HashSet<Organization>();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }

    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Type { get; set; } // e.g. "HQ", "Branch", "Franchise"

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public Organization()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}







