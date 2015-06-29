using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Congress.App.Helpers
{
    public class Helper
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static Guid GetBuildGuid()
        {
            return new Object().GetType().Assembly.ManifestModule.ModuleVersionId;
        }

        public static string GetClientIP()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        public static string GenerateClientUID()
        {
            return Encoding.Default.GetString(new SHA512Managed().ComputeHash(Encoding.Default.GetBytes((GetClientIP() + GetBuildGuid().ToString()))));
        }

        public string generateSalt(int saltSize = 32)
        {
            var salt = new byte[saltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return Encoding.Default.GetString(salt);
        }

        public static string GenerateRandomString(int size = 10)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}