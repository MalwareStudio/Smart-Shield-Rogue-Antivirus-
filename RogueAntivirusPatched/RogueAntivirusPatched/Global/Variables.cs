using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace RogueAntivirusPatched.Global
{
    public static class Variables
    {
        public const string ProductName = "Smart Shield";
        public const string RogueBaseDir = @"C:\Windows\" + ProductName;
        public static string ResourceDir = Path.Combine(RogueBaseDir, "Resources");

        public static bool IsPageWorking = false;
        public static string _assemblyName = Assembly.GetEntryAssembly().GetName().Name + ";component";
        public static string resourcePack = "pack://application:,,,/" + _assemblyName + "/";
        public static string gatheringData = "Gathering saved data, please wait for a while ...";
        public static string[] someResponses = { "fuck you!", "idiot", "bitch", "suck my ass", "cunt", "cut your glizzy",
            "you don't have money you poor whore?", "maggot"};
        public static BitmapImage CursedShieldImage = Convertor.ToBitmapImagePNG(Properties.Resources.shield_cursedB);
        public static bool PayloadsRunning = false;
        public static bool KeystrokeTriggerRunning = false;
    }
}
