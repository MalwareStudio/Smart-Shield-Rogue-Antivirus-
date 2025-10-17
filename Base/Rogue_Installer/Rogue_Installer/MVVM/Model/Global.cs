using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rogue_Installer.MVVM.Model
{
    public static class Global
    {
        public static string _assemblyName = Assembly.GetEntryAssembly().GetName().Name + ";component";
        public static bool IsSetupRunning = false;
        public static string resourcePack = "pack://application:,,,/" + _assemblyName + "/";
    }
}
