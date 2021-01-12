using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace AuthenticationAPI
{
    public class Encryption
    {
        public static string CreateSHAHash(string PasswordSHA512, string salt)
        {
            SHA512Managed sha512 = new SHA512Managed();

            Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Concat(
                PasswordSHA512, salt)));
            sha512.Clear();
            return Convert.ToBase64String(EncryptedSHA512);
        }

        public static string GetSalt()
        {
            int maximumSaltLength = 25;
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}
