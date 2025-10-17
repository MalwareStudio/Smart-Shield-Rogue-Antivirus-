using Rogue_Installer.MVVM.ViewModel;
using Rogue_Installer.WpfWindow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using win = System.Windows;
using static Rogue_Installer.MainWindow;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Rogue_Installer.MVVM.Model
{
    public static class BitmapGenerator
    {
        private static Random rand = new Random();
        private static string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

        public static class SetUp
        {
            public static class BackMainWindow
            {
                public static string directory = Path.Combine(baseDirectory, nameof(BackMainWindow));
                public static int frameCount = 101;
                public static Color defaultColor = Color.FromArgb(0, 0, 0);
                public static List<Color> randomColors = RandomColors(false, false, false, new Point(5, 80));
                public static Point bitmapSize = new Point(800, 800);
                public static int randomBarValue = 100;
                public static int chanceForRandomColor = 100;
            }

            public static class BackAboutPage
            {
                public static string directory = Path.Combine(baseDirectory, nameof(BackAboutPage));
                public static int frameCount = 31;
                public static Color defaultColor = Color.FromArgb(0, 0, 255);
                public static List<Color> randomColors = RandomColors(false, false, true, new Point(50, 200));
                public static Point bitmapSize = new Point(250, 250);
                public static int randomBarValue = 50;
                public static int chanceForRandomColor = 30;
            }

            public static class BackCyberSoldier
            {
                public static string directory = Path.Combine(baseDirectory, nameof(BackCyberSoldier));
                public static int frameCount = 31;
                public static Color defaultColor = Color.FromArgb(0, 0, 200);
                public static List<Color> randomColors = RandomColors(false, false, true, new Point(30, 150));
                public static Point bitmapSize = new Point(250, 250);
                public static int randomBarValue = 50;
                public static int chanceForRandomColor = 30;
            }

            public static class BackExlon
            {
                public static string directory = Path.Combine(baseDirectory, nameof(BackExlon));
                public static int frameCount = 31;
                public static Color defaultColor = Color.FromArgb(102, 0, 204);
                public static List<Color> randomColors = RandomColors(true, false, true, new Point(30, 150));
                public static Point bitmapSize = new Point(250, 250);
                public static int randomBarValue = 50;
                public static int chanceForRandomColor = 30;
            }

            private static int totalFrameCount = BackMainWindow.frameCount + 
                BackAboutPage.frameCount + BackCyberSoldier.frameCount + BackExlon.frameCount;
            private static double oneProcent = 0.01 * totalFrameCount;
            public static double oneProcentBar = 100.0 / totalFrameCount;
            public static double procentResult = 0;

            public static void Generate()
            {
                GenerateBackground(BackMainWindow.directory,
                    BackMainWindow.frameCount,
                    BackMainWindow.defaultColor,
                    BackMainWindow.randomColors,
                    BackMainWindow.bitmapSize,
                    BackMainWindow.randomBarValue,
                    BackMainWindow.chanceForRandomColor);

                GenerateBackground(BackAboutPage.directory,
                    BackAboutPage.frameCount,
                    BackAboutPage.defaultColor,
                    BackAboutPage.randomColors,
                    BackAboutPage.bitmapSize,
                    BackAboutPage.randomBarValue,
                    BackAboutPage.chanceForRandomColor);

                GenerateBackground(BackCyberSoldier.directory,
                    BackCyberSoldier.frameCount,
                    BackCyberSoldier.defaultColor,
                    BackCyberSoldier.randomColors,
                    BackCyberSoldier.bitmapSize,
                    BackCyberSoldier.randomBarValue,
                    BackCyberSoldier.chanceForRandomColor);

                GenerateBackground(BackExlon.directory,
                    BackExlon.frameCount,
                    BackExlon.defaultColor,
                    BackExlon.randomColors,
                    BackExlon.bitmapSize,
                    BackExlon.randomBarValue,
                    BackExlon.chanceForRandomColor);
            }
        }

        private static List<Color> RandomColors(bool red = true, bool green = true, bool blue = true,
            Point randomnessRange = new Point())
        {
            if (randomnessRange.IsEmpty)
            {
                randomnessRange = new Point(1, 255);
            }

            int randX = randomnessRange.X;
            int randY = randomnessRange.Y;

            var colors = new List<Color>();
            int colorRed = 0, colorGreen = 0, colorBlue = 0;
            int colorWhite = 0;

            for (int i = 0; i < 100; i++)
            {
                if (red)
                    colorRed = rand.Next(randX, randY);
                if (green)
                    colorGreen = rand.Next(randX, randY);
                if (blue)
                    colorBlue = rand.Next(randX, randY);

                if (red || green || blue)
                    colors.Add(Color.FromArgb(colorRed, colorGreen, colorBlue));
                else
                {
                    colorWhite = rand.Next(randX, randY);
                    colors.Add(Color.FromArgb(colorWhite, colorWhite, colorWhite));
                }
            }

            return colors;
        }

        private static void GenerateBackground(string directory, int framesCount, Color color,
            List<Color> randomColors, Point bitmapSize,
            int randomBarValue, int chanceForRandomColor)
        {
            if (Directory.Exists(directory))
                return;

            Directory.CreateDirectory(directory);

            for (int frame = 0; frame < framesCount; frame++)
            {
                var bitmap = new Bitmap(bitmapSize.X, bitmapSize.Y);
                var randomColor = randomColors[rand.Next(randomColors.Count)];
                int barIndex = 0;
                string fileName = Path.Combine(directory, frame.ToString() + ".png");

                for (int y = 0; bitmap.Height > y; y++)
                {
                    if (rand.Next(chanceForRandomColor) == 1 && barIndex == 0)
                    {
                        barIndex = rand.Next(1, randomBarValue);
                        randomColor = randomColors[rand.Next(randomColors.Count)];
                    }

                    if (barIndex > 0)
                    {
                        barIndex -= 1;
                    }

                    for (int x = 0; bitmap.Width > x; x++)
                    {
                        if (barIndex > 0)
                        {
                            bitmap.SetPixel(x, y, randomColor);
                        }
                        else
                            bitmap.SetPixel(x, y, color);
                    }
                }

                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                bitmap.Dispose();
                SetUp.procentResult += SetUp.oneProcentBar;
                string contentProcentValue = Math.Round(SetUp.procentResult).ToString() + "%";

                win.Application.Current.Dispatcher.Invoke(() =>
                {
                    var loader = (Loader)win.Application.Current.MainWindow;

                    loader.Dispatcher.Invoke(() =>
                    {
                        loader.sharedVmLoader.BarValue = SetUp.procentResult;
                        loader.sharedVmLoader.BarContent = contentProcentValue;
                        loader.sharedVmLoader.ProgressDescription = "Generating Resources";
                    });
                });
            }
        }
    }
}
