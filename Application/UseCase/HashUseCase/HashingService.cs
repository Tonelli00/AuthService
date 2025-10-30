using Application.Interfaces.HelperInterface;
using System.Security.Cryptography;
using System.Text;

namespace Application.UseCase.HashUseCase
{
    public class HashingService : IHashingService
    {
        public string encryptSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
