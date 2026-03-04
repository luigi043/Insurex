namespace InsureX.Domain.Entities;

public enum OrgType { Bank, Insurer, Broker, Admin }

public class Organisation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OrgType Type { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public Tenant Tenant { get; set; } = null!;
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
