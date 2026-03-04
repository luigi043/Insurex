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
        Guid? tenantId = null;

        // 1. Try resolving from authenticated user claims
        if (ctx.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = ctx.User.FindFirst("tenant_id")?.Value;
            if (Guid.TryParse(tenantClaim, out var parsedClaim))
            {
                tenantId = parsedClaim;
            }
        }

        // 2. Try resolving from custom header
        if (tenantId == null && ctx.Request.Headers.TryGetValue("X-Tenant-Id", out var raw)
            && Guid.TryParse(raw.FirstOrDefault(), out var fromHeader))
        {
            tenantId = fromHeader;
        }

        // 3. Fallback for local development
        if (tenantId == null)
        {
            // In a strict production environment, we might return 400 Bad Request here.
            tenantId = DevTenant;
        }

        tenantContext.Set(tenantId.Value);
        await _next(ctx);
    }
}
