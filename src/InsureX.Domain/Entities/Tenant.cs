namespace InsureX.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;           // for subdomain routing
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
