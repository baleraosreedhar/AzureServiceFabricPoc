using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyUtils.Extensions.Crypto
{
    /// <summary>
    /// </summary>
    public class AesCrypto
    {
        /// <summary>
        ///     The iv
        /// </summary>
        private readonly string _iv;

        /// <summary>
        ///     The private key
        /// </summary>
        private readonly string _privateKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AesCrypto" /> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="iv">The iv.</param>
        public AesCrypto(string privateKey = null, string iv = null)
        {
           // var rnd = new Random();
            if (privateKey == null)
                privateKey = GenerateString(32);
            if (iv == null)
                iv = GenerateString(16);
            _privateKey = privateKey;
            _iv = iv;
        }

        /// <summary>
        ///     Generates the string.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private string GenerateString(int count)
        {
            int number;
            var checkCode = string.Empty;

            var random = new Random();

            for (var i = 0; i < count; i++)
            {
                number = random.Next();
                number = number%36;
                if (number < 10)
                    number += 48;
                else
                    number += 55;

                checkCode += ((char) number).ToString();
            }
            return checkCode;
        }

        /// <summary>
        ///     Decrypts the specified encrypt string.
        /// </summary>
        /// <param name="encryptStr">The encrypt string.</param>
        /// <returns></returns>
        public string Decrypt(string encryptStr)
        {
            var bKey = Encoding.UTF8.GetBytes(_privateKey);
            var bIv = Encoding.UTF8.GetBytes(_iv);
            var byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
#if !NET451
            var aes = Aes.Create();
#else
            var aes = Rijndael.Create();
#endif
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (
                        var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray(), 0, mStream.ToArray().Count());
                    }
                }
            }
            catch
            {
                // ignored
            }
#if NET451
            aes.Clear();
#endif

            return decrypt;
        }

        /// <summary>
        ///     Encrypts the specified plain string.
        /// </summary>
        /// <param name="plainStr">The plain string.</param>
        /// <returns></returns>
        public string Encrypt(string plainStr)
        {
            var bKey = Encoding.UTF8.GetBytes(_privateKey);
            var bIv = Encoding.UTF8.GetBytes(_iv);
            var byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
#if !NET451
            var aes = Aes.Create();
#else
            var aes = Rijndael.Create();
#endif
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (
                        var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch
            {
                // ignored
            }
#if NET451
            aes.Clear();
#endif
            return encrypt;
        }
    }
}