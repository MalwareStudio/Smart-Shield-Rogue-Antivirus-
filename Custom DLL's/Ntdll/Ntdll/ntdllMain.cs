using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ntdll
{
    public static class ntdllMain
    {
        [DllImport("ntdll.dll")]
        public static extern int NtSetInformationProcess(IntPtr ProcessHandle, ProcClassInfo ProcessInformationClass,
        ref int ProcessInformation, int ProcessInformationLength);

        public enum ProcClassInfo : int
        {
            ProcessBasicInformation = 0,
            ProcessDebugPort = 7,
            ProcessWow64Information = 26,
            ProcessImageFileName = 27,
            ProcessBreakOnTermination = 29,
            ProcessProtectionInformation = 61
        }
    }
}
