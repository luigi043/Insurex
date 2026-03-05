using System;
using System.Security.Cryptography;
using System.Text;

namespace IAPR_Data.Classes.Webhook
{
    /// <summary>
    /// Validates HMAC-SHA256 signatures on incoming webhook requests.
    /// The insurer signs the raw request body using a shared secret.
    /// We re-compute the signature and compare with constant-time equals to prevent timing attacks.
    /// </summary>
    public static class HmacValidator
    {
        /// <summary>
        /// Validates that the incoming signature matches what we expect,
        /// given the raw request body and the shared secret for this insurer.
        /// </summary>
        /// <param name="rawBody">Raw UTF-8 request body bytes</param>
        /// <param name="receivedSignature">The signature from the X-Signature-SHA256 request header (hex or base64)</param>
        /// <param name="sharedSecret">The pre-shared HMAC secret for this insurer partner</param>
        /// <returns>True if the signature is valid</returns>
        public static bool IsValid(byte[] rawBody, string receivedSignature, string sharedSecret)
        {
            if (rawBody == null || string.IsNullOrEmpty(receivedSignature) || string.IsNullOrEmpty(sharedSecret))
                return false;

            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(sharedSecret);
                using (var hmac = new HMACSHA256(keyBytes))
                {
                    var computedHash = hmac.ComputeHash(rawBody);
                    var computedHex = BitConverter.ToString(computedHash).Replace("-", "").ToLowerInvariant();

                    // Support both hex and base64 encoded signatures
                    var normalizedReceived = receivedSignature
                        .Replace("sha256=", "")
                        .Trim()
                        .ToLowerInvariant();

                    // Try hex comparison first
                    if (ConstantTimeEquals(computedHex, normalizedReceived))
                        return true;

                    // Try base64 comparison
                    var computedBase64 = Convert.ToBase64String(computedHash);
                    return ConstantTimeEquals(computedBase64, normalizedReceived);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Constant-time string comparison to prevent timing-based side-channel attacks.
        /// </summary>
        private static bool ConstantTimeEquals(string a, string b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            var result = 0;
            for (int i = 0; i < a.Length; i++)
                result |= a[i] ^ b[i];

            return result == 0;
        }
    }
}







