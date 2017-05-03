using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DuoCallTesterLicenseKey
{
    /// <summary>
    /// generate license key
    /// </summary>
    public static class LicenseKeyHandler
    {
        // Init Vector
        private const string InitVector = "tu89geji340t89u2";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;
        // cipher Text
        private const string CipherText = "sss";

        /// <summary>
        /// Get License Key
        /// </summary>
        /// <param name="passPhrase"></param>
        /// <returns>Encoded String</returns>
        public static string GetLicenseKey(string passPhrase)
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            var cipherTextBytes = Convert.FromBase64String(CipherText);
            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                using (var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC})
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
            
        }
    }
}