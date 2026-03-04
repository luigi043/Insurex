using InsureX.Infrastructure.Data;

namespace InsureX.Api.Middleware;

/// <summary>
/// Resolves the current tenant from:
///   1. X-Tenant-Id header  (system-to-system calls)
///   2. A claim in the JWT (future: when auth is wired up)
///   3. Falls back to a dev/testing default if none supplied
/// </summary>
public class TenantContextMiddleware
{
    private readonly RequestDelegate _next;

    // ── Dev-only fallback tenant (remove in production) ───────────────────────
    private static readonly Guid DevTenant = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public TenantContextMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx, TenantContext tenantContext)
    {
        Guid tenantId = DevTenant;

        if (ctx.Request.Headers.TryGetValue("X-Tenant-Id", out var raw)
            && Guid.TryParse(raw.FirstOrDefault(), out var fromHeader))
        {
            tenantId = fromHeader;
        }
        // TODO: extract from JWT claim when auth is implemented

        tenantContext.Set(tenantId);
        await _next(ctx);
    }
}
