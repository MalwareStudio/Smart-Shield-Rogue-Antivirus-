using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace RogueAntivirusPatched.Global
{
    internal class ShieldAnimation
    {
        private static readonly Random rand = new Random();
        public static List<Image> shieldImages;

        public enum GlitchTypes
        {
            Shake = 0,
            Pieces = 1,
            Stripes = 2,
        };

        public class ShieldImageProperty
        {
            public static int shieldImageWidth = 800;
            public static int shieldImageHeight = 800;
        }

        public ShieldAnimation()
        {
            shieldImages = new List<Image>();
        }

        public void DefineGridImages(BitmapImage imageSource, int width, int height,
            int imageCount = 3, Grid grid = null)
        {
            ShieldImageProperty.shieldImageWidth = width;
            ShieldImageProperty.shieldImageHeight = height;

            for (int i = 0; i < imageCount; i++)
            {
                var image = new Image
                {
                    Source = imageSource,
                    Width = width,
                    Height = height
                };

                shieldImages.Add(image);
                grid.Children.Add(image);
            }
        }

        public void MakeShieldGlitch(GlitchTypes type)
        {
            var rect = new Rect();
            var point = new Point();

            int shakeRandomRange = 10;
            int piecesRandomRange = 80;
            int stripeRandomRange = 80;

            switch (type)
            {
                case GlitchTypes.Shake:
                    rect = new Rect(0, 0, ShieldImageProperty.shieldImageWidth, ShieldImageProperty.shieldImageHeight);
                    point = new Point(rand.Next(-shakeRandomRange, shakeRandomRange), rand.Next(-shakeRandomRange, shakeRandomRange));
                    break;
                case GlitchTypes.Pieces:
                    rect = new Rect(rand.Next(ShieldImageProperty.shieldImageWidth), rand.Next(ShieldImageProperty.shieldImageHeight), rand.Next(ShieldImageProperty.shieldImageWidth), rand.Next(ShieldImageProperty.shieldImageHeight));
                    point = new Point(rand.Next(-piecesRandomRange, piecesRandomRange), rand.Next(-piecesRandomRange, piecesRandomRange));
                    break;
                case GlitchTypes.Stripes:
                    int xy = rand.Next(2);

                    if (xy == 1)
                        rect = new Rect(rand.Next(ShieldImageProperty.shieldImageWidth), rand.Next(ShieldImageProperty.shieldImageHeight), ShieldImageProperty.shieldImageWidth, rand.Next(ShieldImageProperty.shieldImageHeight));
                    else
                        rect = new Rect(rand.Next(ShieldImageProperty.shieldImageWidth), rand.Next(ShieldImageProperty.shieldImageHeight), rand.Next(ShieldImageProperty.shieldImageWidth), ShieldImageProperty.shieldImageHeight);
                    point = new Point(rand.Next(-stripeRandomRange, stripeRandomRange), rand.Next(-stripeRandomRange, stripeRandomRange));
                    break;
            }
            var shield = shieldImages[rand.Next(shieldImages.Count)];
            var gemetry = new RectangleGeometry
            {
                Rect = rect
            };

            shield.Clip = gemetry;

            var transform = new TranslateTransform();
            transform.X = point.X;
            transform.Y = point.Y;

            shield.RenderTransform = transform;
            int getZIndex = Panel.GetZIndex(shield);
            Panel.SetZIndex(shield, getZIndex + 1);
        }
    }
}
