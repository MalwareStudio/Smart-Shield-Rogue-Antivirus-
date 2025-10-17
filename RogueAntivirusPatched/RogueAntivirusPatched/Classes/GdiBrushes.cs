using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.Classes
{
    public static class GdiBrushes
    {
        static Random rand;
        public static Bitmap DiagonalBrush(Bitmap bitmap)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;

            rand = new Random();
            Color randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            Color randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (x == w)
                    {
                        randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                        randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                    }

                    if (x < y)
                        bitmap.SetPixel(x, y, randomColor);
                    else
                        bitmap.SetPixel(x, y, randomColor2);
                }
            }

            return bitmap;
        }

        public static Bitmap CursedDiagonalBrush(Bitmap bitmap)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;

            int thickness = 50;
            rand = new Random();

            Color randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            Color randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (x == w || y == h / 2)
                    {
                        randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                        randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                    }

                    int dx = x - y;
                    if (Math.Abs(dx) < thickness / 2)
                        bitmap.SetPixel(x, y, randomColor);
                    else
                        bitmap.SetPixel(x, y, randomColor2);
                }
            }

            return bitmap;
        }

        public static Bitmap ChessBrush(Bitmap bitmap)
        {
            int squareSize = 50;
            int tileSize = squareSize * 2;
            rand = new Random();

            Color randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            Color randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));

            for (int y = 0; y < tileSize; y++)
            {
                for (int x = 0; x < tileSize; x++)
                {
                    if (x == tileSize)
                    {
                        randomColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                        randomColor2 = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                    }
                    bool black = ((x / squareSize) + (y / squareSize)) % 2 == 0;
                    bitmap.SetPixel(x, y, black ? randomColor : randomColor2);
                }
            }

            return bitmap;
        }
    }
}
