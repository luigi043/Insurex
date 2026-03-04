using InsureX.Application.Interfaces;

namespace InsureX.Infrastructure.Data;

/// <summary>
/// Concrete tenant context implementation. Set by middleware on each request.
/// </summary>
public class TenantContext : ITenantContext
{
    public Guid TenantId { get; private set; }
    public bool IsResolved { get; private set; }

    public void Set(Guid tenantId)
    {
        TenantId = tenantId;
        IsResolved = true;
    }
}
