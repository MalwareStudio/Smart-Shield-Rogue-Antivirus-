using RogueAntivirusPatched.View.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static RogueAntivirusPatched.View.Windows.AnnoyingPopUp;

namespace RogueAntivirusPatched.Global
{
    public class Events
    {
        public PopUpPublisher popUpPublisher;
        public NegavPopUpPublisher negavPopUpPublisher;
        public Events()
        {
            popUpPublisher = new PopUpPublisher();
            negavPopUpPublisher = new NegavPopUpPublisher();
        }

        public void ShowPopUp(PopUpArgs.PopUpType type, string title, string subTitle, string text,
            Bitmap bitmap, Thickness bitmapMargin = new Thickness(), int width = 500, int height = 400, UnmanagedMemoryStream soundEffect = null, string contentButton1 = "Ok", string contentButton2 = "Cancel",
            EventHandler btnFirst = null, EventHandler btnSecond = null,
            AnimationTypes animationType = AnimationTypes.SwipeFromLeft,
            int swipeDuration = 500, string bitmapAnimResource = "SwingAnimation", bool autoClick = false)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var popup = new AnnoyingPopUp();

                popup.Button1.Click += (s, e) => popUpPublisher.RaiseMessage(type);
                popup.Button2.Click += (s, e) => negavPopUpPublisher.RaiseMessage(type);
                popup.ShowAnnoyingPopUp(title, subTitle, text, bitmap, bitmapMargin, width, height, soundEffect,
                    contentButton1, contentButton2, null, null, animationType, swipeDuration,
                    bitmapAnimResource, autoClick);
            }));
        }

        public class PopUpArgs : EventArgs
        {
            public enum PopUpType
            {
                None = -1,
                Expirating = 0,
                AVThreatsFound = 1,
                AVThreatsNotFound = 2,
                AVThreatsRemoved = 3,
                AVLoaded = 4,
                JunkLoaded = 5,
                JunkAnalyzeComplete = 6,
                JunkRemoved = 7,
                JunkNotFound = 8,
                RegLoaded = 9,
                RegAnalyzeComplete = 10,
                RegRemoved = 11,
                RegNotFound = 12,
                LicenseInvalid = 13,
                LicenseValid = 14,
                Expiration = 15
            }

            public PopUpType popUpType { get; }

            public PopUpArgs(PopUpType type)
            {
                popUpType = type;
            }
        }

        public class PopUpPublisher
        {
            public event EventHandler<PopUpArgs> popUpHandler;

            public void RaiseMessage(PopUpArgs.PopUpType popUpType)
            {
                popUpHandler?.Invoke(this, new PopUpArgs(popUpType));
            }
        }

        public class NegavPopUpPublisher
        {
            public event EventHandler<PopUpArgs> popUpHandler;

            public void RaiseMessage(PopUpArgs.PopUpType popUpType)
            {
                popUpHandler?.Invoke(this, new PopUpArgs(popUpType));
            }
        }
    }
}
