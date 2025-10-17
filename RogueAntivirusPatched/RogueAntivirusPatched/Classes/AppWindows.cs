using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RogueAntivirusPatched.Advertisement;
using RogueAntivirusPatched.Global;
using RogueAntivirusPatched.Windows;
using static RogueAntivirusPatched.View.Windows.AnnoyingPopUp;
using static RogueAntivirusPatched.Windows.Popup;

namespace RogueAntivirusPatched.Classes
{
    internal class AppWindows
    {
        private Events events;
        private Popup popup;
        private static readonly Random rand = new Random();   

        public AppWindows()
        {
            events = new Events();
        }

        public void Advertisement()
        {
            string title = WindowManager.RandomAscii(rand.Next(5, 20));
            string subTitle = WindowManager.RandomAscii(rand.Next(5, 20));
            string text = WindowManager.RandomAscii(rand.Next(5, 500));
            string btn1 = WindowManager.RandomAscii(rand.Next(5, 20));
            string btn2 = WindowManager.RandomAscii(rand.Next(5, 20));
            var width = rand.Next(200, 800);
            var height = rand.Next(200, 800);

            var ad = new Windows.Advertisement();
            ad.ShowAdvertisement(false, subTitle, title, text, btn1, btn2, null, null, width, height,
                null, null, 0, 0, null, false, true);
        }

        public void AnnoyingPopUp()
        {
            string title = WindowManager.RandomAscii(rand.Next(5, 20));
            string subTitle = WindowManager.RandomAscii(rand.Next(5, 20));
            string text = WindowManager.RandomAscii(rand.Next(5, 500));
            string btn1 = WindowManager.RandomAscii(rand.Next(5, 20));
            string btn2 = WindowManager.RandomAscii(rand.Next(5, 20));
            var animTypes = (AnimationTypes[])Enum.GetValues(typeof(AnimationTypes));
            var randomAnimType = animTypes[rand.Next(animTypes.Length)];
            var randomAnimDuration = rand.Next(500, 1500);
            var width = rand.Next(200, 800);
            var height = rand.Next(200, 800);
            events.ShowPopUp(Events.PopUpArgs.PopUpType.None, title, subTitle, text, null,
                new Thickness(0), width, height, null, btn1, btn2, null, null, randomAnimType,
                randomAnimDuration, "SwingAnimation", true);
        }

        public void PopUp()
        {
            string title = WindowManager.RandomAscii(rand.Next(5, 20));
            string subTitle = WindowManager.RandomAscii(rand.Next(5, 20));
            string text = WindowManager.RandomAscii(rand.Next(5, 500));
            string btn1 = WindowManager.RandomAscii(rand.Next(5, 20));

            popup = new Popup();
            popup.ShowPopup(title, subTitle, text, btn1, null, PopUpDuration.ANIM_SHORT,
                null);
        }

        public void Notifications()
        {
            string title = RandomChars(rand.Next(5, 50));
            string text = RandomChars(rand.Next(5, 1000));
            string btn1 = RandomChars(rand.Next(5, 20));
            string btn2 = RandomChars(rand.Next(5, 20));

            var notify = new NotifyAd();
            notify.SetNotification(text, title, btn1, btn2, "Cursed", "",
                Microsoft.Toolkit.Uwp.Notifications.ToastDuration.Short, true);
        }

        private string RandomChars(int charCount)
        {
            string letters = "abcdefghijkl mnopqrst uvwx  yzABCDE FGHIJKLMN OPQRSTUVWXYZ 012345 6789";

            var sb = new StringBuilder();

            for (int i = 0; i < charCount; i++)
            {
                sb.Append(letters[rand.Next(letters.Length)]);
            }

            return sb.ToString();
        }
    }
}
