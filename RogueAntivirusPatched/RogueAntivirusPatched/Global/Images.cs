using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueAntivirusPatched.Global;
using Windows.Security.Cryptography.Core;
using static RogueAntivirusPatched.Global.Variables;

namespace RogueAntivirusPatched.Global
{
    public static class Images
    {
        public static Uri warning = new Uri(Path.Combine(ResourceDir, "warning.png"), UriKind.Absolute);
        public static Uri registry = new Uri(Path.Combine(ResourceDir, "registry.png"), UriKind.Absolute);
        public static Uri threat = new Uri(Path.Combine(ResourceDir, "detected.png"), UriKind.Absolute);
        public static Uri sweep = new Uri(Path.Combine(ResourceDir, "sweep.png"), UriKind.Absolute);
        public static Uri support = new Uri(Path.Combine(ResourceDir, "online-chat.png"), UriKind.Absolute);
    }
}
