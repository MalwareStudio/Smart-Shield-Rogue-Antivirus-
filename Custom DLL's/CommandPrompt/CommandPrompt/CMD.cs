using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CommandPrompt
{
    public static class CMD
    {
        public static void Command(string arg)
        {
            var cmd = new ProcessStartInfo();
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.FileName = "cmd.exe";
            cmd.UseShellExecute = true;
            cmd.CreateNoWindow = true;
            cmd.Arguments = "/c " + arg;

            using (var process = Process.Start(cmd))
            {
                process.WaitForExit();
            }
        }

        public static void Command(string arg, ProcessWindowStyle style, bool noWindow = false)
        {
            var cmd = new ProcessStartInfo();
            cmd.WindowStyle = style;
            cmd.FileName = "cmd.exe";
            cmd.UseShellExecute = true;
            cmd.CreateNoWindow = noWindow;
            cmd.Arguments = "/c " + arg;

            using (var process = Process.Start(cmd))
            {
                process.WaitForExit();
            }
        }
    }
}
