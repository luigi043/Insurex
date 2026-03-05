using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using IAPR_Data.Classes;
using Newtonsoft.Json;

namespace IAPR_Data.Services
{
    /// <summary>
    /// OAuth2 Client Credentials token service.
    /// Issues short-lived opaque Bearer tokens to registered API clients (Banks, Insurers).
    /// Tokens are SHA-256 hashed before storage — the raw token is only held in memory.
    /// </summary>
    public sealed class OAuth2TokenService
    {
        private static readonly Lazy<OAuth2TokenService> _instance =
            new Lazy<OAuth2TokenService>(() => new OAuth2TokenService());

        public static OAuth2TokenService Instance => _instance.Value;

        private OAuth2TokenService() { }

        // ------------------------------------------------------------------
        // Issue Token
        // ------------------------------------------------------------------

        /// <summary>
        /// Validates client credentials and issues a Bearer token.
        /// Returns null if the client is unknown, inactive, or the secret is wrong.
        /// </summary>
        public TokenResponse IssueToken(string clientId, string clientSecret, string requestedScope)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return null;

            using (var db = ApplicationDbContext.Create(System.Configuration.ConfigurationManager.ConnectionStrings["connIAPRData"].ToString()))
            {
                var client = db.ApiClients
                    .FirstOrDefault(c => c.ClientId == clientId && c.IsActive && c.RevokedAt == null);

                if (client == null) return null;

                // Constant-time secret verification
                var candidateHash = Hash(clientSecret);
                if (!ConstantTimeEquals(client.ClientSecretHash, candidateHash))
                    return null;

                // Resolve scopes: intersect requested with allowed
                var allowedScopes = (client.AllowedScopes ?? "").Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var requestedScopes = (requestedScope ?? "").Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var grantedScopes = requestedScopes.Length > 0
                    ? requestedScopes.Where(s => allowedScopes.Contains(s)).ToArray()
                    : allowedScopes;

                // Generate random opaque token (32 bytes → 64 hex chars)
                var rawToken = GenerateSecureToken();
                var tokenHash = Hash(rawToken);
                var expiresAt = DateTime.UtcNow.AddSeconds(client.AccessTokenLifetimeSeconds);

                // Persist hash for revocation checking
                db.IssuedTokens.Add(new IssuedToken
                {
                    ClientId   = clientId,
                    TokenHash  = tokenHash,
                    Scopes     = string.Join(" ", grantedScopes),
                    TenantId   = client.TenantId,
                    ExpiresAt  = expiresAt
                });
                db.SaveChanges();

                return new TokenResponse
                {
                    AccessToken = rawToken,
                    TokenType   = "Bearer",
                    ExpiresIn   = client.AccessTokenLifetimeSeconds,
                    Scope       = string.Join(" ", grantedScopes)
                };
            }
        }

        // ------------------------------------------------------------------
        // Validate Token
        // ------------------------------------------------------------------

        /// <summary>
        /// Validates an incoming Bearer token from the Authorization header.
        /// Returns the token's TenantId and Scopes, or null if invalid/expired/revoked.
        /// </summary>
        public ValidatedToken Validate(string rawToken)
        {
            if (string.IsNullOrWhiteSpace(rawToken)) return null;

            var tokenHash = Hash(rawToken);
            var now = DateTime.UtcNow;

            using (var db = ApplicationDbContext.Create(System.Configuration.ConfigurationManager.ConnectionStrings["connIAPRData"].ToString()))
            {
                var issued = db.IssuedTokens.FirstOrDefault(t =>
                    t.TokenHash == tokenHash &&
                    !t.IsRevoked &&
                    t.ExpiresAt > now);

                if (issued == null) return null;

                return new ValidatedToken
                {
                    ClientId = issued.ClientId,
                    Scopes   = (issued.Scopes ?? "").Split(' '),
                    TenantId = issued.TenantId,
                    ExpiresAt = issued.ExpiresAt
                };
            }
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        private static string GenerateSecureToken()
        {
            var bytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        private static string Hash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private static bool ConstantTimeEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            var result = 0;
            for (var i = 0; i < a.Length; i++)
                result |= a[i] ^ b[i];
            return result == 0;
        }
    }

    // ------------------------------------------------------------------
    // Response/result models
    // ------------------------------------------------------------------

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    public class ValidatedToken
    {
        public string ClientId { get; set; }
        public string[] Scopes { get; set; }
        public int? TenantId { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool HasScope(string scope) =>
            Scopes != null && Scopes.Contains(scope, StringComparer.OrdinalIgnoreCase);
    }
}







