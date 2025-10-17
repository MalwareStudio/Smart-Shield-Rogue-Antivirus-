using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.Global
{
    public class Contact
    {
        public static string emailAddress = "rogueav2025@gmail.com";
        public static string subject = "Problem Name";
        public static string body = "Help me, I don't know what to do :(";

        public static void GetToEmail()
        {
            string gmailUrl = $"https://mail.google.com/mail/?view=cm&fs=1" + 
                $"&to={Uri.EscapeDataString(emailAddress)}" +
                $"&su={Uri.EscapeDataString(subject)}" +
                $"&body={Uri.EscapeDataString(body)}";

            Process.Start(new ProcessStartInfo
            {
                FileName = gmailUrl,
                UseShellExecute = true
            });
        }
    }
}
