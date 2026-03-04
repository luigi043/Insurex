using System;
using System.Security.Claims;
using System.Web;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Resolves the current Tenant from the authenticated user's OWIN claims on every HTTP request.
    /// Exposes TenantId and OrganizationId for tenant-aware data access.
    /// </summary>
    public static class TenantContext
    {
        private const string HttpContextKey = "TenantContext.TenantId";
        private const string OrgContextKey  = "TenantContext.OrganizationId";

        /// <summary>
        /// Gets the TenantId for the current HTTP request, resolved from OWIN ClaimsIdentity.
        /// Returns null if the user is not authenticated or claim is missing.
        /// </summary>
        public static int? Current
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;

                // Cached per-request
                if (ctx.Items[HttpContextKey] != null)
                    return (int?)ctx.Items[HttpContextKey];

                var identity = ctx.User?.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return null;

                var claim = identity.FindFirst("TenantId");
                if (claim != null && int.TryParse(claim.Value, out int tenantId))
                {
                    ctx.Items[HttpContextKey] = tenantId;
                    return tenantId;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the OrganizationId for the current HTTP request, resolved from OWIN ClaimsIdentity.
        /// </summary>
        public static int? CurrentOrganization
        {
            get
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;

                if (ctx.Items[OrgContextKey] != null)
                    return (int?)ctx.Items[OrgContextKey];

                var identity = ctx.User?.Identity as ClaimsIdentity;
                if (identity == null || !identity.IsAuthenticated) return null;

                var claim = identity.FindFirst("OrganizationId");
                if (claim != null && int.TryParse(claim.Value, out int orgId))
                {
                    ctx.Items[OrgContextKey] = orgId;
                    return orgId;
                }

                return null;
            }
        }
    }
}
