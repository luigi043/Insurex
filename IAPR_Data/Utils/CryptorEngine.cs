using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text.Json;

namespace IAPR_Data.Utils
{
    public class CryptorEngine
    {
        private static IConfiguration? _config;

        public static void Initialize(IConfiguration configuration)
        {
            _config = configuration;
        }

        private static string GetKey(string keyName) => 
            _config?[$"AppSettings:{keyName}"] ?? throw new InvalidOperationException($"Encryption key '{keyName}' missing from configuration.");

        // Helper Method for AES-GCM Encryption
        private static string GcmEncrypt(string toEncrypt, string configurationKeyName, bool useHashing)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(toEncrypt);
            string keyString = GetKey(configurationKeyName);
            
            byte[] keyBytes;
            if (useHashing)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
                }
            }
            else
            {
                keyBytes = new byte[32];
                byte[] rawKeyBytes = Encoding.UTF8.GetBytes(keyString);
                Array.Copy(rawKeyBytes, keyBytes, Math.Min(rawKeyBytes.Length, 32));
            }

            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            GcmBlockCipher gcm = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, nonce);
            gcm.Init(true, parameters);

            byte[] cipherTextBytes = new byte[gcm.GetOutputSize(plainTextBytes.Length)];
            int len = gcm.ProcessBytes(plainTextBytes, 0, plainTextBytes.Length, cipherTextBytes, 0);
            gcm.DoFinal(cipherTextBytes, len);

            byte[] payload = new byte[nonce.Length + cipherTextBytes.Length];
            Array.Copy(nonce, 0, payload, 0, nonce.Length);
            Array.Copy(cipherTextBytes, 0, payload, nonce.Length, cipherTextBytes.Length);

            return Convert.ToBase64String(payload);
        }

        private static string GcmDecrypt(string cipherString, string configurationKeyName, bool useHashing)
        {
            byte[] payload = Convert.FromBase64String(cipherString);
            string keyString = GetKey(configurationKeyName);

            if (payload.Length < 28)
                throw new CryptographicException("Invalid ciphertext payload size for AES-GCM.");

            byte[] keyBytes;
            if (useHashing)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
                }
            }
            else
            {
                keyBytes = new byte[32];
                byte[] rawKeyBytes = Encoding.UTF8.GetBytes(keyString);
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
                return Encoding.UTF8.GetString(plainTextBytes);
            }
            catch (Org.BouncyCastle.Crypto.InvalidCipherTextException ex)
            {
                throw new CryptographicException("AES-GCM Authentication Tag Validation Failed.", ex);
            }
        }

        public static string ValidationEncrypt(string toEncrypt, bool useHashing) => GcmEncrypt(toEncrypt, "ValCryptoKey", useHashing);
        public static string ValidationDecrypt(string cipherString, bool useHashing) => GcmDecrypt(cipherString, "ValCryptoKey", useHashing);
        public static string GenericEncrypt(string toEncrypt, bool useHashing) => GcmEncrypt(toEncrypt, "GenCryptokey", useHashing);
        public static string GenericDecrypt(string cipherString, bool useHashing) => GcmDecrypt(cipherString, "GenCryptokey", useHashing);

        public static string GenericEncrypt_V2(string toEncrypt, bool useHashing) => AES_Encrypt(toEncrypt, GetKey("AesCryptoKey"));
        public static string GenericDecrypt_V2(string cipherString, bool useHashing) => AES_Decrypt(cipherString, GetKey("AesCryptoKey"));

        private static string AES_Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] buffer = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            
            string encryptedText = Convert.ToBase64String(encrypted);
            string mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key)).Replace("-", "").ToLower();

            var payload = new { iv = Convert.ToBase64String(aes.IV), value = encryptedText, mac = mac };
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));
        }

        private static string AES_Decrypt(string cipherString, string key)
        {
            byte[] base64Decoded = Convert.FromBase64String(cipherString);
            string jsonPayload = Encoding.UTF8.GetString(base64Decoded);
            var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonPayload) ?? throw new InvalidOperationException("Invalid crypto payload.");

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Convert.FromBase64String(payload["iv"]);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] buffer = Convert.FromBase64String(payload["value"]);
            byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] HmacSHA256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }
}







