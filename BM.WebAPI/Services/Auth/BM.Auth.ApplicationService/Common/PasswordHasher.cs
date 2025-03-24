using System;
using System.Security.Cryptography;

namespace BM.Auth.ApplicationService.Common
{
    public static class PasswordHasher
    {
        // Băm mật khẩu với PBKDF2
        public static (string hash, string salt) HashPassword(string password)
        {
            // Tạo salt ngẫu nhiên
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Băm mật khẩu với PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // Tạo mã băm 256-bit (32 byte)
                return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
            }
        }

        // Kiểm tra mật khẩu
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return CryptographicOperations.FixedTimeEquals(hashBytes, storedHashBytes);
            }
        }
    }
}