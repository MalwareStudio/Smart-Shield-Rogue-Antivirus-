using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.Model
{
    internal class mThreatResults
    {
        public static readonly string[] MalwareTypes = 
        { 
            "Malware", "Virus", "Trojan", "Worm", "Spyware", "Adware" 
        };

        public static readonly string[] ThreatLevel = 
        { 
            "Low", "Medium", "High", "Critical" 
        };

        public static readonly string[] ThreatVector = 
        { 
            "Exe.trojan.nekark", "Gen:Variant.Fragtor.477024", "Unsafe", "HEUR:Trojan/VBS.Sendmail.b",
            "Trojan.Win32.Nekark.kuhjwd", "Worm.Generic!8.402 (CLOUD)", 
            "GayCode.Sus", "Indian.Win32.Virus", "Mal/Generic-S",
            "Corn.Illegal.POPUP", "Generic.Unwanted.POPUP", "Virus.Win32.Ho*ny", 
            "amogus.Killer.Win64", "mal.Clutter",
            "Generic.ml", "Malware.AI.4212279010", "Unwanted.File", "amaboutocum.org.POPUP", 
            "Disk.Writer", "File.KILLER",
            "BadProgram.Win32", "W32.Common.1DECEA2E", "MALICIOUS", 
            "Malware.Win32.GenericMC.cc", "Generic Reputation PUA (PUA)",
            "ML.Attribute.HighConfidence", "sex.Explicit.Ware", "Virus.Win32.Alert"
        };
    }
}
