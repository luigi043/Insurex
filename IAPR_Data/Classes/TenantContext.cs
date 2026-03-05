using System;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Legacy TenantContext. Refactor to use IHttpContextAccessor in the API layer.
    /// </summary>
    public static class TenantContext
    {
        public static int? Current => null;
        public static int? CurrentOrganization => null;
    }
}




