using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using IAPR_Data.Classes;
using IAPR_Data.Services;
using Newtonsoft.Json;

namespace IAPR_API
{
    /// <summary>
    /// Structured REST API for Compliance States, Cases, and Audit Logs.
    /// All endpoints require a valid Bearer token (OAuth2 Client Credentials).
    ///
    /// Endpoints:
    ///   GET  /api/compliance/states?page=1&amp;pageSize=20&amp;tenantId=&amp;outcome=
    ///   GET  /api/compliance/cases?page=1&amp;pageSize=20&amp;status=&amp;priority=
    ///   GET  /api/compliance/cases/{id}
    ///   POST /api/compliance/cases/{id}/resolve
    ///   GET  /api/compliance/audit?page=1&amp;pageSize=20&amp;entity=&amp;correlationId=
    /// </summary>
    [ServiceContract]
    public interface IComplianceApiService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/states?page={page}&pageSize={pageSize}&outcome={outcome}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetComplianceStates(int page, int pageSize, string outcome);

        [OperationContract]
        [WebGet(UriTemplate = "/cases?page={page}&pageSize={pageSize}&status={status}&priority={priority}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetCases(int page, int pageSize, string status, string priority);

        [OperationContract]
        [WebGet(UriTemplate = "/cases/{id}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetCase(string id);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/cases/{id}/resolve",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ResolveCase(string id, Stream body);

        [OperationContract]
        [WebGet(UriTemplate = "/audit?page={page}&pageSize={pageSize}&entity={entity}&correlationId={correlationId}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetAuditLog(int page, int pageSize, string entity, string correlationId);

        [OperationContract]
        [WebGet(UriTemplate = "/dashboard/summary",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetDashboardSummary();

        [OperationContract]
        [WebGet(UriTemplate = "/dashboard/charts",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetDashboardCharts();

        [OperationContract]
        [WebGet(UriTemplate = "/assets?page={page}&pageSize={pageSize}&status={status}&assetType={assetType}&registrationNumber={registrationNumber}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetAssets(int page, int pageSize, string status, string assetType, string registrationNumber);

        [OperationContract]
        [WebGet(UriTemplate = "/assets/{id}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetAsset(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/admin/users?page={page}&pageSize={pageSize}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetUsers(int page, int pageSize);

        [OperationContract]
        [WebGet(UriTemplate = "/admin/tenants?page={page}&pageSize={pageSize}",
                ResponseFormat = WebMessageFormat.Json)]
        Stream GetTenants(int page, int pageSize);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ComplianceApiService : IComplianceApiService
    {
        // ------------------------------------------------------------------
        // GET /compliance/states
        // ------------------------------------------------------------------
        public Stream GetComplianceStates(int page, int pageSize, string outcome)
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                var query = db.ComplianceStates.AsQueryable();

                if (token.TenantId.HasValue)
                    query = query.Where(s => s.TenantId == token.TenantId);

                if (!string.IsNullOrWhiteSpace(outcome))
                    query = query.Where(s => s.Outcome == outcome);

                var total = query.Count();
                var items = query
                    .OrderByDescending(s => s.EvaluatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new
                    {
                        s.Id, s.SourceEventId, s.Outcome, s.Reason,
                        s.PolicyId, s.AssetId, s.TenantId, s.CorrelationId, s.EvaluatedAt
                    })
                    .ToList();

                return WriteJson(new PagedResult<object>(items.Cast<object>(), total, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // GET /compliance/cases
        // ------------------------------------------------------------------
        public Stream GetCases(int page, int pageSize, string status, string priority)
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                var query = db.Cases.AsQueryable();

                if (token.TenantId.HasValue)
                    query = query.Where(c => c.TenantId == token.TenantId);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(c => c.Status == status);

                if (!string.IsNullOrWhiteSpace(priority))
                    query = query.Where(c => c.Priority == priority);

                var total = query.Count();
                var items = query
                    .OrderByDescending(c => c.OpenedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new
                    {
                        c.Id, c.CaseNumber, c.Title, c.Status, c.Priority,
                        c.AssignedToUserId, c.OpenedAt, c.DueAt, c.ResolvedAt,
                        c.EscalatedAt, c.TenantId, c.CorrelationId
                    })
                    .ToList();

                return WriteJson(new PagedResult<object>(items.Cast<object>(), total, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // GET /compliance/cases/{id}
        // ------------------------------------------------------------------
        public Stream GetCase(string id)
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            if (!int.TryParse(id, out int caseId))
                return BadRequest("Invalid case ID.");

            using (var db = ApplicationDbContext.Create())
            {
                var cas = db.Cases.Find(caseId);
                if (cas == null || (token.TenantId.HasValue && cas.TenantId != token.TenantId))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                    return WriteJson(ApiResponse<object>.Fail("Case not found."));
                }

                var notes = db.CaseNotes
                    .Where(n => n.CaseId == caseId)
                    .OrderBy(n => n.CreatedAt)
                    .Select(n => new { n.Id, n.AuthorName, n.NoteText, n.CreatedAt, n.IsSystemGenerated })
                    .ToList();

                return WriteJson(ApiResponse<object>.Ok(new
                {
                    cas.Id, cas.CaseNumber, cas.Title, cas.Description, cas.Status, cas.Priority,
                    cas.SourceComplianceStateId, cas.AssignedToUserId, cas.OpenedAt, cas.DueAt,
                    cas.ResolvedAt, cas.EscalatedAt, cas.TenantId, cas.CorrelationId,
                    Notes = notes
                }));
            }
        }

        // ------------------------------------------------------------------
        // POST /compliance/cases/{id}/resolve
        // ------------------------------------------------------------------
        public Stream ResolveCase(string id, Stream body)
        {
            var token = RequireBearer("compliance:write");
            if (token == null) return Unauthorized();

            if (!int.TryParse(id, out int caseId))
                return BadRequest("Invalid case ID.");

            string resolution;
            try
            {
                using (var r = new StreamReader(body))
                {
                    dynamic req = JsonConvert.DeserializeObject(r.ReadToEnd());
                    resolution = req?.resolution?.ToString() ?? "Resolved via API.";
                }
            }
            catch { resolution = "Resolved via API."; }

            var success = CaseManager.Instance.ResolveCase(caseId, resolution, actorName: token.ClientId);

            if (!success)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                return WriteJson(ApiResponse<object>.Fail("Case not found or already resolved."));
            }

            return WriteJson(ApiResponse<object>.Ok(null, $"Case {id} resolved successfully."));
        }

        // ------------------------------------------------------------------
        // GET /compliance/audit
        // ------------------------------------------------------------------
        public Stream GetAuditLog(int page, int pageSize, string entity, string correlationId)
        {
            var token = RequireBearer("audit:read");
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                var query = db.AuditLog.AsQueryable();

                if (token.TenantId.HasValue)
                    query = query.Where(a => a.TenantId == token.TenantId);

                if (!string.IsNullOrWhiteSpace(entity))
                    query = query.Where(a => a.EntityName == entity);

                if (!string.IsNullOrWhiteSpace(correlationId))
                    query = query.Where(a => a.CorrelationId == correlationId);

                var total = query.Count();
                var items = query
                    .OrderByDescending(a => a.OccurredAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new
                    {
                        a.Id, a.CorrelationId, a.EntityName, a.EntityId,
                        a.Action, a.ActorName, a.TenantId, a.OccurredAt, a.Notes
                    })
                    .ToList();

                return WriteJson(new PagedResult<object>(items.Cast<object>(), total, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // GET /dashboard/summary
        // ------------------------------------------------------------------
        public Stream GetDashboardSummary()
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            try
            {
                var provider = new IAPR_Data.Providers.Daschboard_Provider();
                System.Data.DataSet ds;

                // Determine which dashboard to fetch based on tenant/role (simplified for now)
                if (token.TenantId.HasValue)
                {
                    // If tenant is an insurer, fetch insurer dashboard, else financer
                    // For logic parity with legacy, we'll try to fetch based on Partner_Id if we can resolve it
                    // For now, default to Admin or Financer based on presence of TenantId
                    ds = provider.Get_Financer_Landing_DashboardTable(token.TenantId.Value);
                }
                else
                {
                    ds = provider.Get_Admin_Landing_DashboardTable();
                }

                if (ds == null || ds.Tables.Count == 0)
                    return WriteJson(ApiResponse<DashboardSummaryDto>.Ok(new DashboardSummaryDto()));

                var summary = new DashboardSummaryDto();

                // Mapping logic (based on spGet_Admin_Landing_Dashboard_Totals and legacy code)
                // Table[0]: Unpaid Premium Assets, Table[1]: No Insurance, Table[2]: All Assets, Table[3]: Insured, Table[4]: Adequate, Table[5]: Under
                if (ds.Tables.Count >= 6)
                {
                    summary.PremiumUnpaidAssets = Convert.ToInt32(ds.Tables[0].Rows[0]["iNumber_Of_Assets"]);
                    summary.PremiumUnpaidValue  = Convert.ToDecimal(ds.Tables[0].Rows[0]["dcUninsured_Finance_Value"]);

                    summary.NoInsuranceDetailsAssets = Convert.ToInt32(ds.Tables[1].Rows[0]["iNumber_Of_Assets"]);
                    summary.NoInsuranceDetailsValue  = Convert.ToDecimal(ds.Tables[1].Rows[0]["dcUninsured_Finance_Value"]);

                    summary.TotalAssets = Convert.ToInt32(ds.Tables[2].Rows[0]["iNumber_Of_Assets"]);
                    summary.TotalValue  = Convert.ToDecimal(ds.Tables[2].Rows[0]["dcFinance_Value"]);

                    summary.UninsuredAssets = summary.PremiumUnpaidAssets + summary.NoInsuranceDetailsAssets;
                    summary.UninsuredValue  = summary.PremiumUnpaidValue + summary.NoInsuranceDetailsValue;
                    summary.UninsuredPercentage = summary.TotalAssets > 0 ? (double)summary.UninsuredAssets / summary.TotalAssets * 100 : 0;

                    summary.AdequatelyInsuredAssets = Convert.ToInt32(ds.Tables[4].Rows[0]["iNumber_Of_Assets"]);
                    summary.AdequatelyInsuredValue  = Convert.ToDecimal(ds.Tables[4].Rows[0]["dcFinance_Value"]);

                    summary.UnderInsuredAssets = Convert.ToInt32(ds.Tables[5].Rows[0]["iNumber_Of_Assets"]);
                    summary.UnderInsuredValue  = Convert.ToDecimal(ds.Tables[5].Rows[0]["dcFinance_Value"]);
                }

                return WriteJson(ApiResponse<DashboardSummaryDto>.Ok(summary));
            }
        // ------------------------------------------------------------------
        // GET /dashboard/charts
        // ------------------------------------------------------------------
        public Stream GetDashboardCharts()
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            try
            {
                var provider = new IAPR_Data.Providers.Daschboard_Provider();
                System.Data.DataSet ds;

                if (token.TenantId.HasValue)
                    ds = provider.Get_Financer_Landing_DashboardCharts(token.TenantId.Value);
                else
                    ds = provider.Get_Admin_Landing_DashboardCharts();

                var results = new List<ChartSeriesDto>();

                if (ds != null && ds.Tables.Count > 0)
                {
                    // Map Table[0]: Insurance Status
                    var statusSeries = new ChartSeriesDto { Title = "Insurance Status", XAxisName = "Status", YAxisName = "Count" };
                    foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                    {
                        statusSeries.Data.Add(new ChartDataPointDto { Label = row[0].ToString(), Value = row[1].ToString() });
                    }
                    results.Add(statusSeries);

                    // Map Table[1]: Age Analysis (if exists)
                    if (ds.Tables.Count > 1)
                    {
                        var ageSeries = new ChartSeriesDto { Title = "Age Analysis", XAxisName = "Days", YAxisName = "Asset Count" };
                        foreach (System.Data.DataRow row in ds.Tables[1].Rows)
                        {
                            ageSeries.Data.Add(new ChartDataPointDto { Label = row["DayCount"].ToString(), Value = row["AssetCount"].ToString() });
                        }
                        results.Add(ageSeries);
                    }
                }

                return WriteJson(ApiResponse<List<ChartSeriesDto>>.Ok(results));
            }
            catch (Exception ex)
            {
                return WriteJson(ApiResponse<object>.Fail($"Error fetching dashboard charts: {ex.Message}"));
            }
        }

        // ------------------------------------------------------------------
        // GET /assets
        // ------------------------------------------------------------------
        public Stream GetAssets(int page, int pageSize, string status, string assetType, string registrationNumber)
        {
            var token = RequireBearer("compliance:read"); // Reusing for assets for now
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                var query = db.ComplianceStates.AsQueryable();

                if (token.TenantId.HasValue)
                    query = query.Where(s => s.TenantId == token.TenantId);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(s => s.Outcome == status);

                // Group by AssetId to get the latest state for each asset
                var latestStates = query
                    .GroupBy(s => s.AssetId)
                    .Select(g => g.OrderByDescending(s => s.EvaluatedAt).FirstOrDefault());

                var total = latestStates.Count();
                var items = latestStates
                    .OrderByDescending(s => s.EvaluatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var results = items.Select(s => new AssetDto
                {
                    Id = s.AssetId ?? 0,
                    TenantId = s.TenantId,
                    AssetType = "Motor", 
                    AssetIdentifier = s.SourceEventId,
                    RegistrationNumber = "REG-" + s.AssetId,
                    FinancedAmount = 25000,
                    LoanStartDate = DateTime.UtcNow.AddYears(-1),
                    LoanEndDate = DateTime.UtcNow.AddYears(3),
                    Status = "Active",
                    ComplianceStatus = s.Outcome
                }).ToList();

                return WriteJson(new PagedResult<AssetDto>(results, total, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // GET /assets/{id}
        // ------------------------------------------------------------------
        public Stream GetAsset(string id)
        {
            var token = RequireBearer("compliance:read");
            if (token == null) return Unauthorized();

            if (!int.TryParse(id, out int assetId))
                return BadRequest("Invalid asset ID.");

            using (var db = ApplicationDbContext.Create())
            {
                var latestState = db.ComplianceStates
                    .Where(s => s.AssetId == assetId)
                    .OrderByDescending(s => s.EvaluatedAt)
                    .FirstOrDefault();

                if (latestState == null || (token.TenantId.HasValue && latestState.TenantId != token.TenantId))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                    return WriteJson(ApiResponse<object>.Fail("Asset not found."));
                }

                var history = db.ComplianceStates
                    .Where(s => s.AssetId == assetId)
                    .OrderByDescending(s => s.EvaluatedAt)
                    .Take(10)
                    .Select(s => new ComplianceStateDto
                    {
                        Id = s.Id,
                        Outcome = s.Outcome,
                        Reason = s.Reason,
                        EvaluatedAt = s.EvaluatedAt,
                        CorrelationId = s.CorrelationId
                    }).ToList();

                var detail = new AssetDetailDto
                {
                    Id = assetId,
                    TenantId = latestState.TenantId,
                    AssetType = "Motor",
                    AssetIdentifier = latestState.SourceEventId,
                    RegistrationNumber = "REG-" + assetId,
                    FinancedAmount = 25000,
                    Status = "Active",
                    ComplianceStatus = latestState.Outcome,
                    Borrower = new BorrowerDto
                    {
                        Name = "John Doe",
                        IDNumber = "8501015000088",
                        Email = "john.doe@example.com",
                        Phone = "+27 82 000 0000"
                    },
                    Policies = new List<PolicyDto>
                    {
                        new PolicyDto 
                        { 
                            Id = 1, 
                            PolicyNumber = "POL-INS-7782", 
                            InsurerName = "Modern Insure Ltd", 
                            Status = "Active", 
                            ExpiryDate = DateTime.UtcNow.AddMonths(11), 
                            InsuredValue = 32000 
                        }
                    },
                    ComplianceHistory = history
                };

                return WriteJson(ApiResponse<AssetDetailDto>.Ok(detail));
            }
        }

        // ------------------------------------------------------------------
        // GET /admin/users
        // ------------------------------------------------------------------
        public Stream GetUsers(int page, int pageSize)
        {
            var token = RequireBearer("admin:read");
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                // In a real system, this would query AspNetUsers and link to Tenants
                // Mocking for now as we transition.
                var users = new List<UserDto>
                {
                    new UserDto { Id = "1", UserName = "admin@insurex.com", Email = "admin@insurex.com", FullName = "System Admin", Role = "Administrator", IsActive = true },
                    new UserDto { Id = "2", UserName = "banker@testbank.com", Email = "banker@testbank.com", FullName = "James Banker", TenantId = 1, Role = "Manager", IsActive = true }
                };

                return WriteJson(new PagedResult<UserDto>(users, users.Count, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // GET /admin/tenants
        // ------------------------------------------------------------------
        public Stream GetTenants(int page, int pageSize)
        {
            var token = RequireBearer("admin:read");
            if (token == null) return Unauthorized();

            page     = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            using (var db = ApplicationDbContext.Create())
            {
                var tenants = db.Tenants.OrderBy(t => t.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var results = tenants.Select(t => new TenantDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Identifier = t.Identifier,
                    Type = "Financer", // Simplified mapping
                    CreatedAt = DateTime.UtcNow.AddYears(-1),
                    IsActive = t.IsActive
                }).ToList();

                var total = db.Tenants.Count();
                return WriteJson(new PagedResult<TenantDto>(results, total, page, pageSize));
            }
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        private static ValidatedToken RequireBearer(string requiredScope = null)
        {
            var authHeader = WebOperationContext.Current?.IncomingRequest.Headers["Authorization"] ?? "";
            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;

            var rawToken = authHeader.Substring(7).Trim();
            var token = OAuth2TokenService.Instance.Validate(rawToken);
            if (token == null) return null;

            if (!string.IsNullOrEmpty(requiredScope) && !token.HasScope(requiredScope))
                return null;

            return token;
        }

        private static Stream Unauthorized()
        {
            var ctx = WebOperationContext.Current;
            ctx.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            ctx.OutgoingResponse.Headers["WWW-Authenticate"] = "Bearer realm=\"InsureX\"";
            return WriteJson(ApiResponse<object>.Fail("Unauthorized. Valid Bearer token required."));
        }

        private static Stream BadRequest(string msg)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
            return WriteJson(ApiResponse<object>.Fail(msg));
        }

        private static Stream WriteJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new System.IO.MemoryStream(bytes);
        }
    }
}
