using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Security
{
    public sealed class EncryptionService : IEncryptionService
    {
        private const int NonceSize = 12; // Recommended for AES-GCM
        private const int TagSize = 16;   // 128-bit authentication tag

        private readonly byte[] _key;
        private const string ShaSalt = "Cfcs@502603#ByAnshul";

        public EncryptionService(IConfiguration configuration)
        {
            var keyString = configuration["Encryption:EncryptionKey"];

            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("Encryption key is missing from configuration.");

            // Recommended: store key in Base64 format in appsettings / secret store
            try
            {
                _key = Convert.FromBase64String(keyString);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Encryption:EncryptionKey must be a valid Base64 string.");
            }

            // AES keys must be 16, 24, or 32 bytes
            if (_key.Length is not (16 or 24 or 32)) throw new InvalidOperationException("Encryption key length must be 16, 24, or 32 bytes after Base64 decoding.");
        }

        public string EncryptToString(string plainText)
        {
            if (plainText is null)
                throw new ArgumentNullException(nameof(plainText));

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var nonce = RandomNumberGenerator.GetBytes(NonceSize);
            var cipherBytes = new byte[plainBytes.Length];
            var tag = new byte[TagSize];

            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);

            var output = new byte[NonceSize + TagSize + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, output, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, output, NonceSize, TagSize);
            Buffer.BlockCopy(cipherBytes, 0, output, NonceSize + TagSize, cipherBytes.Length);

            return Convert.ToBase64String(output);
        }

        public string DecryptFromString(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentException("Encrypted text cannot be null or empty.", nameof(cipherText));

            byte[] input;
            try
            {
                input = Convert.FromBase64String(cipherText);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Encrypted text is not a valid Base64 string.", nameof(cipherText), ex);
            }

            if (input.Length < NonceSize + TagSize)
                throw new ArgumentException("Encrypted payload is invalid.", nameof(cipherText));

            var nonce = new byte[NonceSize];
            var tag = new byte[TagSize];
            var cipherBytes = new byte[input.Length - NonceSize - TagSize];
            var plainBytes = new byte[cipherBytes.Length];

            Buffer.BlockCopy(input, 0, nonce, 0, NonceSize);
            Buffer.BlockCopy(input, NonceSize, tag, 0, TagSize);
            Buffer.BlockCopy(input, NonceSize + TagSize, cipherBytes, 0, cipherBytes.Length);

            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }

        public byte[] GenerateSha512Bytes(string input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            var finalInput = input + ShaSalt;
            var inputBytes = Encoding.UTF8.GetBytes(finalInput);
            return SHA512.HashData(inputBytes);
        }

        public string GenerateSha512String(string input)
        {
            var hashBytes = GenerateSha512Bytes(input);
            return Convert.ToHexString(hashBytes);
        }
        // Use this for CurrentToken / SessionId / logout/session validation
        public byte[] GenerateAuthTokenHash(string input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            return SHA256.HashData(Encoding.UTF8.GetBytes(input)); // 512
        }
    }
}
