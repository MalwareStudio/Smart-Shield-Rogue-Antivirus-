using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.Model
{
    public class mThreatInfo
    {
        public bool IsChecked { get; set; }
        public string Vector { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Level { get; set; }
    }
}
