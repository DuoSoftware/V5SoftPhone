using System;
using System.Text;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    /// <summary>
    /// SessionHandler
    /// </summary>
    public static class SessionHandler
    {
        /// <summary>
        /// RandomCharId
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        private static string RandomCharId(int passwordLength)
        {
            //const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "0123456789";
            var chars = new char[passwordLength];
            var rd = new Random();

            for (var i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// create Unique id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string UniqueId(string userName)
        {
            var dateObject = DateTime.Now;
            var id = new StringBuilder();
            id.Append(dateObject.Year);
            id.Append(dateObject.Month);
            id.Append(dateObject.Day);
            id.Append(dateObject.Hour);
            id.Append(dateObject.Minute);
            id.Append(dateObject.Second);
            id.Append(RandomCharId(4));
            id.Append(userName);
            return id.ToString();
        }
    }
}