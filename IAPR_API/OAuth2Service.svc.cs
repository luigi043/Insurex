using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using IAPR_Data.Services;
using Newtonsoft.Json;

namespace IAPR_API
{
    /// <summary>
    /// OAuth2 Client Credentials token endpoint.
    /// Exposed at: POST /api/oauth/token
    ///
    /// Request body (application/x-www-form-urlencoded or JSON):
    ///   grant_type=client_credentials
    ///   client_id={clientId}
    ///   client_secret={secret}
    ///   scope={space-separated scopes}  (optional)
    ///
    /// Response (RFC 6749 §4.4.3):
    ///   { "access_token": "...", "token_type": "Bearer", "expires_in": 3600, "scope": "..." }
    /// </summary>
    [ServiceContract]
    public interface IOAuth2Service
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/token",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Token(Stream body);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OAuth2Service : IOAuth2Service
    {
        public Stream Token(Stream body)
        {
            var ctx = WebOperationContext.Current;

            try
            {
                string rawBody;
                using (var reader = new StreamReader(body))
                    rawBody = reader.ReadToEnd();

                // Parse: support both JSON and form-encoded body
                string grantType, clientId, clientSecret, scope;

                if (rawBody.TrimStart().StartsWith("{"))
                {
                    // JSON body
                    dynamic req = JsonConvert.DeserializeObject(rawBody);
                    grantType    = req?.grant_type?.ToString() ?? "";
                    clientId     = req?.client_id?.ToString() ?? "";
                    clientSecret = req?.client_secret?.ToString() ?? "";
                    scope        = req?.scope?.ToString() ?? "";
                }
                else
                {
                    // x-www-form-urlencoded
                    var form = System.Web.HttpUtility.ParseQueryString(rawBody);
                    grantType    = form["grant_type"]    ?? "";
                    clientId     = form["client_id"]     ?? "";
                    clientSecret = form["client_secret"] ?? "";
                    scope        = form["scope"]         ?? "";
                }

                if (!string.Equals(grantType, "client_credentials", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    return WriteJson(new { error = "unsupported_grant_type" });
                }

                var token = OAuth2TokenService.Instance.IssueToken(clientId, clientSecret, scope);

                if (token == null)
                {
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    return WriteJson(new { error = "invalid_client" });
                }

                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                ctx.OutgoingResponse.Headers["Cache-Control"] = "no-store";
                ctx.OutgoingResponse.Headers["Pragma"] = "no-cache";
                return WriteJson(token);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[OAuth2Service] Token error: " + ex.Message);
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return WriteJson(new { error = "server_error" });
            }
        }

        private static Stream WriteJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(bytes);
        }
    }
}
