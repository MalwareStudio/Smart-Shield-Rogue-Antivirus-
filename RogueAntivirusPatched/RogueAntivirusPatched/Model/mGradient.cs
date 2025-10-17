using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RogueAntivirusPatched.Model
{
    public class mGradient
    {
        public static LinearGradientBrush WhiteFade()
        {
            var gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0.0));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(186, 186, 186), 1.0));

            return gradient;
        }

        public static LinearGradientBrush RedFade()
        {
            var gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 0, 0), 0.0));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(186, 0, 0), 1.0));

            return gradient;
        }

        public static LinearGradientBrush OrangeGradient()
        {
            var gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 171, 0), 0.0));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 200, 80), 1.0));

            return gradient;
        }

        public static LinearGradientBrush RedGradient()
        {
            var gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 0, 0), 0.0));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(255, 80, 80), 1.0));

            return gradient;
        }

        public static LinearGradientBrush GreenGradient()
        {
            var gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 0);

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(0, 255, 0), 0.0));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(80, 255, 80), 1.0));

            return gradient;
        }
    }
}
