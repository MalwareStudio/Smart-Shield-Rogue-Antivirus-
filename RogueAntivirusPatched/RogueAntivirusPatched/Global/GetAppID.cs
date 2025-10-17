using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RogueAntivirusPatched.Global
{
    internal class GetAppID
    {
        public static string ThisAppID = "";
        public string ObtainAppID(string filePath)
        {
            int shortLenght = 1000;
            byte[] shortBuffer = new byte[shortLenght];

            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    var limitedBuffer = stream.Read(shortBuffer, 0, shortLenght);

                    var finalBuffer = md5.ComputeHash(stream);
                    return BitConverter.ToString(finalBuffer).Replace("-", "").ToUpperInvariant();
                }
            }
        }
    }
}
