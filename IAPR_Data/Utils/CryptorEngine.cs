using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Script.Serialization;
using U = IAPR_Data.Utils;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
namespace IAPR_Data.Utils
{
    public class CryptorEngine
    {
        // Helper Method for AES-GCM Encryption
        private static string GcmEncrypt(string toEncrypt, string configurationKeyName, bool useHashing)
        {
            byte[] plainTextBytes = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string keyString = ConfigurationManager.AppSettings[configurationKeyName]?.ToString();
            
            if (string.IsNullOrEmpty(keyString)) 
                throw new InvalidOperationException($"Encryption key '{configurationKeyName}' missing from AppSettings.");

            byte[] keyBytes;
            if (useHashing)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    keyBytes = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(keyString));
                }
            }
            else
            {
                // Pad or truncate to 32 bytes (256-bit)
                keyBytes = new byte[32];
                byte[] rawKeyBytes = UTF8Encoding.UTF8.GetBytes(keyString);
                Array.Copy(rawKeyBytes, keyBytes, Math.Min(rawKeyBytes.Length, 32));
            }

            // Generate 12-byte Nonce (IV) for GCM
            byte[] nonce = new byte[12];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(nonce);
            }

            GcmBlockCipher gcm = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, nonce);
            gcm.Init(true, parameters);

            byte[] cipherTextBytes = new byte[gcm.GetOutputSize(plainTextBytes.Length)];
            int len = gcm.ProcessBytes(plainTextBytes, 0, plainTextBytes.Length, cipherTextBytes, 0);
            gcm.DoFinal(cipherTextBytes, len);

            // Payload format: [12-byte Nonce] + [Ciphertext + AuthTag]
            byte[] payload = new byte[nonce.Length + cipherTextBytes.Length];
            Array.Copy(nonce, 0, payload, 0, nonce.Length);
            Array.Copy(cipherTextBytes, 0, payload, nonce.Length, cipherTextBytes.Length);

            return Convert.ToBase64String(payload);
        }

        // Helper Method for AES-GCM Decryption
        private static string GcmDecrypt(string cipherString, string configurationKeyName, bool useHashing)
        {
            byte[] payload = Convert.FromBase64String(cipherString);
            string keyString = ConfigurationManager.AppSettings[configurationKeyName]?.ToString();

            if (string.IsNullOrEmpty(keyString)) 
                throw new InvalidOperationException($"Encryption key '{configurationKeyName}' missing from AppSettings.");

            // Less than 12 bytes nonce + 16 bytes auth tag = invalid
            if (payload.Length < 28)
                throw new CryptographicException("Invalid ciphertext payload size for AES-GCM.");

            byte[] keyBytes;
            if (useHashing)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    keyBytes = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(keyString));
                }
            }
            else
            {
                keyBytes = new byte[32];
                byte[] rawKeyBytes = UTF8Encoding.UTF8.GetBytes(keyString);
                Array.Copy(rawKeyBytes, keyBytes, Math.Min(rawKeyBytes.Length, 32));
            }

            byte[] nonce = new byte[12];
            Array.Copy(payload, 0, nonce, 0, 12);

            byte[] cipherTextBytes = new byte[payload.Length - 12];
            Array.Copy(payload, 12, cipherTextBytes, 0, cipherTextBytes.Length);

            GcmBlockCipher gcm = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, nonce);
            gcm.Init(false, parameters);

            byte[] plainTextBytes = new byte[gcm.GetOutputSize(cipherTextBytes.Length)];
            try
            {
                int len = gcm.ProcessBytes(cipherTextBytes, 0, cipherTextBytes.Length, plainTextBytes, 0);
                gcm.DoFinal(plainTextBytes, len);
                return UTF8Encoding.UTF8.GetString(plainTextBytes);
            }
            catch (Org.BouncyCastle.Crypto.InvalidCipherTextException ex)
            {
                throw new CryptographicException("AES-GCM Authentication Tag Validation Failed. Data may have been tampered with or the wrong key was used.", ex);
            }
        }

        public static string ValidationEncrypt(string toEncrypt, bool useHashing)
        {
            return GcmEncrypt(toEncrypt, "ValCryptoKey", useHashing);
        }

        public static string ValidationDecrypt(string cipherString, bool useHashing)
        {
            return GcmDecrypt(cipherString, "ValCryptoKey", useHashing);
        }

        public static string GenericEncrypt(string toEncrypt, bool useHashing)
        {
            return GcmEncrypt(toEncrypt, "GenCryptokey", useHashing);
        }

        public static string GenericDecrypt(string cipherString, bool useHashing)
        {
            return GcmDecrypt(cipherString, "GenCryptokey", useHashing);
        }

        public static string GenericEncrypt_V2(string toEncrypt, bool useHashing)
        {
            string key = ConfigurationManager.AppSettings["AesCryptoKey"]?.ToString();
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("Encryption key missing.");
            return AES_Encrypt(toEncrypt, key);
        }
        public static string GenericDecrypt_V2(string cipherString, bool useHashing)
        {
            string key = ConfigurationManager.AppSettings["AesCryptoKey"]?.ToString();
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("Encryption key missing.");
            return AES_Decrypt(cipherString, key);
        }

        private static readonly Encoding encoding = Encoding.UTF8;
        private static string AES_Encrypt(string plainText, string key)
        {
            string encStr = string.Empty;

            AesManaged aes = new AesManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            aes.Key = encoding.GetBytes(key);
            aes.GenerateIV();

            ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] buffer = encoding.GetBytes(plainText);

            string encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));

            String mac = "";

            mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key)).Replace("-", "").ToLower();

            var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(aes.IV) },
                    { "value", encryptedText },
                    { "mac", mac },
                };

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            encStr = Convert.ToBase64String(encoding.GetBytes(serializer.Serialize(keyValues)));

            return encStr;
        }
        private static string AES_Decrypt(string plainText, string key)
        {
            string decStr = string.Empty;

            AesManaged aes = new AesManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = encoding.GetBytes(key);

            // Base 64 decode
            byte[] base64Decoded = Convert.FromBase64String(plainText);
            string base64DecodedStr = encoding.GetString(base64Decoded);

            // JSON Decode base64Str
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var payload = ser.Deserialize<Dictionary<string, string>>(base64DecodedStr);

            aes.IV = Convert.FromBase64String(payload["iv"]);

            ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] buffer = Convert.FromBase64String(payload["value"]);

            decStr = encoding.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));

            return decStr;
        }

        static byte[] HmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
            {
                return hmac.ComputeHash(encoding.GetBytes(data));
            }
        }
    }
}
