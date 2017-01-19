using System.Security.Cryptography;
using System.Text;

namespace HentaiViewer.Common {
    public class MD5Converter {
        public static string MD5Hash(string input) {
            var hash = new StringBuilder();
            var md5Provider = new MD5CryptoServiceProvider();
            var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes) hash.Append(t.ToString("x2"));
            return hash.ToString();
        }
    }
}