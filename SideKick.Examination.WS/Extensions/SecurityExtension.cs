using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Extensions
{
    public static class SecurityExtension
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static string jsSHA(this string key, string salt)
        {
            salt = salt ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(salt);
            byte[] messageBytes = encoding.GetBytes(key);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToHexString(hashmessage).ToLower();
            }
        }

        public static string HashHmac(this string password)
        {
            var keyByte = encoding.GetBytes(Cons.SaltKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(encoding.GetBytes(password));
                var buff = hmacsha256.Hash;
                string sbinary = "";
                for (int i = 0; i < buff.Length; i++)
                    sbinary += buff[i].ToString("X2"); /* hex format */
                return sbinary.ToLower();
            }
        }

        public static byte[] StringToByteArray(this string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string RandomString(int length = 64)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }
    }
}
