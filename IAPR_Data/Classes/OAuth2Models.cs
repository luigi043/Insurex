using System;
using System.ComponentModel.DataAnnotations;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Registered OAuth2 API client (machine-to-machine, Client Credentials flow).
    /// Banks and Insurers are issued a ClientId + ClientSecret instead of using
    /// user/password Basic Auth.
    /// </summary>
    public class ApiClientCredential
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Public identifier sent by the client in the token request.</summary>
        [Required]
        [StringLength(100)]
        public string ClientId { get; set; }

        /// <summary>
        /// Hashed client secret (SHA-256). The plaintext secret is NEVER stored.
        /// Compare candidate by hashing and comparing with ConstantTimeEquals.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ClientSecretHash { get; set; }

        /// <summary>Human-readable name (e.g., "FNB Bank Integration").</summary>
        [StringLength(200)]
        public string DisplayName { get; set; }

        /// <summary>Comma-separated list of scopes this client may request (e.g., "assets:read policies:read").</summary>
        [StringLength(500)]
        public string AllowedScopes { get; set; }

        /// <summary>Token lifetime in seconds (default 3600 = 1 hour).</summary>
        public int AccessTokenLifetimeSeconds { get; set; } = 3600;

        /// <summary>Tenant this client belongs to.</summary>
        public int? TenantId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RevokedAt { get; set; }
    }

    /// <summary>
    /// Issued access token record for audit and revocation tracking.
    /// </summary>
    public class IssuedToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientId { get; set; }

        /// <summary>SHA-256 hash of the raw token value (never store the raw token).</summary>
        [Required]
        [StringLength(200)]
        public string TokenHash { get; set; }

        [StringLength(500)]
        public string Scopes { get; set; }

        public int? TenantId { get; set; }

        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        /// <summary>True if revoked before natural expiry.</summary>
        public bool IsRevoked { get; set; }

        public IssuedToken()
        {
            IssuedAt = DateTime.UtcNow;
        }
    }
}
