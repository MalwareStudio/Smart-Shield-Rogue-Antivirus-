using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace RogueAntivirusPatched.Global
{
    public class Sounds
    {
        public void PlaySound(UnmanagedMemoryStream memory)
        {
            if (memory == null)
                return;

            memory.Position = 0;
            var current = new byte[memory.Length];
            memory.Read(current, 0, current.Length);
            var clone = new MemoryStream(current);

            using (var soundPlayer = new SoundPlayer(clone))
            {
                soundPlayer.Play();
            }
        }
    }
}
