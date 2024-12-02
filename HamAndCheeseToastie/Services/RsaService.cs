using System.Security.Cryptography;

namespace HamAndCheeseToastie.Services
{
    public interface IRsaService
    {
        string GetPublicKey();
        string DecryptPassword(string encryptedPassword);
    }

    public class RsaService : IRsaService, IDisposable
    {
        private readonly RSA _rsa;

        public RsaService()
        {
            _rsa = RSA.Create(2048);
        }

        public string GetPublicKey()
        {
            // Export the public key in PEM format
            return Convert.ToBase64String(_rsa.ExportRSAPublicKey());
        }

        public string DecryptPassword(string encryptedPassword)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
                byte[] decryptedBytes = _rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                return System.Text.Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("Failed to decrypt password", ex);
            }
        }

        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}
