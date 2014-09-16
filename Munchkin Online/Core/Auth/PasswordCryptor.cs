using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Munchkin_Online.Core.Auth
{
    public class PasswordCryptor
    {
        public static string Crypt(string password)
        {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}