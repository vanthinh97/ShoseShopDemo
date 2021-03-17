using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AptechShoseShop.Models.Security
{
    public static class MySecurity
    {
        public const string sign = "thiskey";
        public static string EncryptPassword(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password + sign));
            string rs = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return rs;
        }
    }
}