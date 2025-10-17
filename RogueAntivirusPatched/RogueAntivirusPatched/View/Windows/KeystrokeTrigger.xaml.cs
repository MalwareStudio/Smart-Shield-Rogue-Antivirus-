using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RogueAntivirusPatched.ViewModel;
using System.Timers;
using draw = System.Drawing;
using static RogueAntivirusPatched.Global.Variables;
using System.IO;
using form = System.Windows.Forms;
using System.Windows.Threading;
using RogueAntivirusPatched.Classes;
using RogueAntivirusPatched.Global;

namespace RogueAntivirusPatched.View.Windows
{
    /// <summary>
    /// Interaction logic for KeystrokeTrigger.xaml
    /// </summary>
    public partial class KeystrokeTrigger : Window
    {
        private vmKeystrokeTrigger _vmKeystrokeTrigger;
        private DispatcherTimer timerAnimationShake, timerAnimationPieces, timerAnimationStripes;
        private DispatcherTimer timerBackground;
        private DispatcherTimer timerText;
        private Beats beats;
        private ShieldAnimation shieldAnimation;
        public KeystrokeTrigger()
        {
            InitializeComponent();
            _vmKeystrokeTrigger = new vmKeystrokeTrigger();
            DataContext = _vmKeystrokeTrigger;
            beats = new Beats();
            shieldAnimation = new ShieldAnimation();
        }

        private static readonly Random rand = new Random();

        private static List<TextBlock> phrasesTextBlock = new List<TextBlock>();

        private string[] phrases = { "Do not search that!", "Not allowed!", "GET OUT!",
            "LEAVE!", "There is NO ESCAPE!!", "Cannot spell that!", "How dare YOU!",
            "Nothing can STOP ME!", "Null exception!", "You have been betrayed!",
            "I will not forgive you!", "BSOD!"};

        public class PhraseTextProperty
        {
            public static Point FontSize = new Point(10, 100);
            public static Point ScreenDimensions = new Point(form.Screen.PrimaryScreen.Bounds.Width, 
                form.Screen.PrimaryScreen.Bounds.Height);
            public static Point Angle = new Point(-40, 40);
        }

        private static List<BitmapImage> backgroundFrames = new List<BitmapImage>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KeystrokeTriggerRunning = true;
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.Hide();
            });
        }

        public async Task DoAnimation()
        {
            shieldAnimation.DefineGridImages(_vmKeystrokeTrigger.ShieldImage,
                ShieldAnimation.ShieldImageProperty.shieldImageWidth,
                ShieldAnimation.ShieldImageProperty.shieldImageHeight, 3, ShieldGrid);
            DefineGridText(rand.Next(4, 10));

            string outputFolder = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

            await Task.Run(() =>
            {
                if (!File.Exists(Path.Combine(outputFolder, "trigger0.png")))
                    GenerateBackground(outputFolder);

                if (File.Exists(Path.Combine(outputFolder, "trigger0.png")))
                {
                    string[] frames = Directory.GetFiles(outputFolder, "*.png");

                    foreach (var frame in frames)
                    {
                        if (!frame.Contains("trigger"))
                            continue;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            backgroundFrames.Add(new BitmapImage(new Uri(frame)));
                        });
                    }
                }
            });

            await Task.Delay(500);

            SetUpTimers(1, 1, 1, 50, 100);

            timerBackground.Start();
            Application.Current.Dispatcher.Invoke(() => { _vmKeystrokeTrigger.IsShieldVisible = Visibility.Visible; });

            PCMAudio.PCM.WaveForms[] waves = { PCMAudio.PCM.WaveForms.Square, PCMAudio.PCM.WaveForms.Saw, PCMAudio.PCM.WaveForms.Noise };
            await beats.PCMAudio(beats.gibbersih(), waves, 100, 1.0, 5);

            Show();
        }

        private void DefineGridText(int textCount)
        {
            var alreadyUsed = new List<string>();

            for (int i = 0; i < textCount; i++)
            {
                string randomPhrase = phrases[rand.Next(phrases.Length)];

                if (alreadyUsed.Count > 0)
                {
                    bool escape = false;
                    while (!escape)
                    {
                        randomPhrase = phrases[rand.Next(phrases.Length)];
                        foreach (string item in alreadyUsed)
                        {
                            if (randomPhrase == item)
                            {
                                escape = false;
                                break;
                            }
                            escape = true;
                        }
                    }
                }

                alreadyUsed.Add(randomPhrase);

                var textblock = new TextBlock
                {
                    Text = randomPhrase,
                    FontSize = rand.Next((int)PhraseTextProperty.FontSize.X, 
                    (int)PhraseTextProperty.FontSize.Y),
                    FontFamily = new FontFamily("Comic Sans Ms"),
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Red,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    Margin = new Thickness(rand.Next((int)PhraseTextProperty.ScreenDimensions.X), 
                    rand.Next((int)PhraseTextProperty.ScreenDimensions.Y), 0, 0)
                };

                phrasesTextBlock.Add(textblock);
                TextGrid.Children.Add(textblock);
            }
        }

        private void SetUpTimers(int shakeInterval, int piecesInterval, int stripesInterval,
            int backgroundInterval, int textInterval)
        {
            timerAnimationShake = new DispatcherTimer();
            timerAnimationShake.IsEnabled = true;
            timerAnimationShake.Interval = TimeSpan.FromMilliseconds(shakeInterval);
            timerAnimationShake.Tick += TimerAnimationShake_Elapsed;

            timerAnimationPieces = new DispatcherTimer();
            timerAnimationPieces.IsEnabled = true;
            timerAnimationPieces.Interval = TimeSpan.FromMilliseconds(piecesInterval);
            timerAnimationPieces.Tick += TimerAnimationPieces_Elapsed;

            timerAnimationStripes = new DispatcherTimer();
            timerAnimationStripes.IsEnabled = true;
            timerAnimationStripes.Interval = TimeSpan.FromMilliseconds(stripesInterval);
            timerAnimationStripes.Tick += TimerAnimationStripes_Elapsed;

            timerBackground = new DispatcherTimer();
            timerBackground.IsEnabled = false;
            timerBackground.Interval = TimeSpan.FromMilliseconds(backgroundInterval);
            timerBackground.Tick += TimerBackground_Tick;

            timerText = new DispatcherTimer();
            timerText.IsEnabled = true;
            timerText.Interval = TimeSpan.FromMilliseconds(textInterval);
            timerText.Tick += TimerText_Tick; ;
        }

        private void GenerateBackground(string outputFolder)
        {
            var scrWidth = form.Screen.PrimaryScreen.Bounds.Width;
            var scrHeight = form.Screen.PrimaryScreen.Bounds.Height;

            var backgroundBitmaps = new List<draw.Bitmap>();

            for (int imageCount = 0; imageCount < 50; imageCount++)
            {
                var bitmap = new draw.Bitmap(100, 100);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int color = rand.Next(200, 255);
                        bitmap.SetPixel(x, y, draw.Color.FromArgb(0, 0, color));
                    }
                }

                backgroundBitmaps.Add(bitmap);
            }

            int foreachIndex = 0;

            foreach (var generatedBitmap in backgroundBitmaps)
            {
                var newBitmap = new draw.Bitmap(scrWidth, scrHeight);

                using (var g = draw.Graphics.FromImage(newBitmap))
                {
                    g.SmoothingMode = draw.Drawing2D.SmoothingMode.HighSpeed;
                    g.CompositingQuality = draw.Drawing2D.CompositingQuality.HighSpeed;
                    g.InterpolationMode = draw.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(generatedBitmap, 0, 0, scrWidth, scrHeight);
                }

                newBitmap.Save(Path.Combine(outputFolder, "trigger" + foreachIndex.ToString() + ".png"), draw.Imaging.ImageFormat.Png);
                generatedBitmap.Dispose();
                newBitmap.Dispose();

                foreachIndex++;
            }
        }

        private void TimerBackground_Tick(object sender, EventArgs e)
        {
            _vmKeystrokeTrigger.BackgroundImage = backgroundFrames[rand.Next(backgroundFrames.Count)];
        }

        private void TimerText_Tick(object sender, EventArgs e)
        {
            foreach (var phrase in phrasesTextBlock)
            {
                phrase.RenderTransformOrigin = new Point(0.5, 0.5);
                phrase.RenderTransform = new RotateTransform
                {
                    Angle = (rand.Next((int)PhraseTextProperty.Angle.X,
                    (int)PhraseTextProperty.Angle.Y))
                };

                phrase.FontSize = rand.Next((int)PhraseTextProperty.FontSize.X,
                    (int)PhraseTextProperty.FontSize.Y);
                phrase.Margin = new Thickness(rand.Next(-100,
                    (int)PhraseTextProperty.ScreenDimensions.X),
                    rand.Next(-100, (int)PhraseTextProperty.ScreenDimensions.Y), 0, 0);
            }
        }

        private void TimerAnimationShake_Elapsed(object sender, EventArgs e)
        {
            shieldAnimation.MakeShieldGlitch(ShieldAnimation.GlitchTypes.Shake);
        }

        private void TimerAnimationPieces_Elapsed(object sender, EventArgs e)
        {
            shieldAnimation.MakeShieldGlitch(ShieldAnimation.GlitchTypes.Pieces);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void TimerAnimationStripes_Elapsed(object sender, EventArgs e)
        {
            shieldAnimation.MakeShieldGlitch(ShieldAnimation.GlitchTypes.Stripes);
        }
    }
}
