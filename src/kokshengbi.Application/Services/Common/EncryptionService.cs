using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace kokshengbi.Application.Services.Common
{
    public class EncryptionService
    {
        private const string SALT = "kokshengbi_backend";

        // Hashes the password using HMAC-SHA256 with a secret key
        public static string HashPasswordWithKey(string password, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static string EncryptPassword(string userPassword)
        {
            string hashedPassword = HashPasswordWithKey(userPassword, SALT);
            return hashedPassword;
        }

        public static bool VerifyPassword(string userPassword, string passwordToBeVerify)
        {
            string hashedPassword = EncryptPassword(passwordToBeVerify);
            if (userPassword == hashedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
