namespace InsureX.Application.Interfaces;

/// <summary>
/// Holds the current tenant ID for the HTTP request scope.
/// Implemented in Infrastructure/Api layer.
/// </summary>
public interface ITenantContext
{
    Guid TenantId { get; }
    bool IsResolved { get; }
}
