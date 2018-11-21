using System;
using System.Security.Cryptography;
using System.Text;

namespace Delivery.Authentication.Crosscutting.Helper
{
    public class PasswordHash : IPasswordHash
    {
        public string Converter(string text, Encoding enc)
        {
            byte[] buffer = enc.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
        }
    }
}
