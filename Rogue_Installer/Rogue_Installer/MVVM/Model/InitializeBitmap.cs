using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static Rogue_Installer.MVVM.Model.BitmapGenerator;

namespace Rogue_Installer.MVVM.Model
{
    public static class InitializeBitmap
    {
        public static List<BitmapImage> MainBackgroundImages = new List<BitmapImage>();

        private static string[] MainBackgroundFrames = Directory.GetFiles(SetUp.BackMainWindow.directory);

        public static List<BitmapImage> AboutBackgroundImages = new List<BitmapImage>();

        private static string[] AboutBackgroundFrames = Directory.GetFiles(SetUp.BackAboutPage.directory);

        public static List<BitmapImage> CyberBackgroundImages = new List<BitmapImage>();

        private static string[] CyberBackgroundFrames = Directory.GetFiles(SetUp.BackCyberSoldier.directory);

        public static List<BitmapImage> ExlonBackgroundImages = new List<BitmapImage>();

        private static string[] ExlonBackgroundFrames = Directory.GetFiles(SetUp.BackExlon.directory);

        public static void StoreBitmapsIntoMemory()
        {
            MainBackgroundImages = FillList(MainBackgroundFrames);
            AboutBackgroundImages = FillList(AboutBackgroundFrames);
            CyberBackgroundImages = FillList(CyberBackgroundFrames);
            ExlonBackgroundImages = FillList(ExlonBackgroundFrames);
        }

        private static List<BitmapImage> FillList(string[] frames)
        {
            var bitmapList = new List<BitmapImage>();
            foreach (var frame in frames)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(frame);
                bitmap.EndInit();
                bitmap.Freeze();

                bitmapList.Add(bitmap);
            }

            return bitmapList;
        }
    }
}
