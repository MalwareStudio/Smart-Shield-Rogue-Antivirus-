using RogueAntivirusPatched.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RogueAntivirusPatched.Classes
{
    internal class NotificationHandler
    {
        public static List<Windows.Popup> popups = new List<Windows.Popup>();

        public static void UpdatePositions()
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            for (int i = 0; i < popups.Count; i++)
            {
                Popup popup = popups[i];
                popup.Top = bounds.Height - popup.Height * (i + 1);

            }
        }
    }
}
