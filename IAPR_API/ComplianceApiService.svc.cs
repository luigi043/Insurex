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
