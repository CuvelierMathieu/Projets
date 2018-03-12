using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BCL.Models
{
    public class Encryptor
    {
        public string Encrypt(string notEncryptedString)
        {
            if (string.IsNullOrWhiteSpace(notEncryptedString))
                throw new Exception("Password can't be null or empty.");

            if (notEncryptedString.Contains(' '))
                throw new Exception("Password can't contain white spaces.");

            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(notEncryptedString));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
