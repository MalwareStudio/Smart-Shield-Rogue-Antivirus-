using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.Global
{
    public static class ThreadRestriction
    {
        public static int threadCount = 0;

        public static bool IsThreadAllowed()
        {
            threadCount++;

            if (threadCount == 1)
                return true;

            threadCount = 0;
            return false;
        }
    }
}
