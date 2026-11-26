namespace Application.Interfaces
{
    public interface IEncryptionService
    {
        string EncryptToString(string plainText);
        string DecryptFromString(string cipherText);
        byte[] GenerateAuthTokenHash(string input);
        byte[] GenerateSha512Bytes(string input);
        string GenerateSha512String(string input);
    }
}
