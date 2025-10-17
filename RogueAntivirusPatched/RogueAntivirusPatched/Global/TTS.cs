using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Threading;

namespace RogueAntivirusPatched.Global
{
    public static class TTS
    {
        public static class TTSProperty
        {
            public static class Antivirus
            {
                public static string threatDetectedPart1 = "Warning, ";
                public static string threatDetectedPart2 = " has been detected!";
                public static string threatsRemoved = "All selected threats have been successfully removed!";
                public static string removalInit = "Removal has been initialized.";
                public static string scanInit = "Searching for threats!";
                public static string noThreatsFound = "No threats have been found; your system is safe.";
                public static string threatsFound = "The system is infected; please remove all threats immediately!";
                public static string resumed = "Scanning has been resumed!";
                public static string paused = "Scanning has been paused!";
            }

            public static class JunkCleaner
            {
                public static string scanInit = "Analyzing files";
                public static string nothingFound = "Could not find any junk files! Your computer is clean!";
                public static string result = "We have found ";
                public static string canGet = "If you remove all found junk files, you can get ";
                public static string removing = "Removing selected files";
                public static string removeFinish = "Cleaning is completed, you got ";
                public static string onLoad = "Some junk files have been found! " +
                "You should get rid of these files to get more free space and get better performance.";
            }

            public static class RegistryOptimizer
            {
                public static string onLoad = "Some unnecessary registry data has been found! You should remove them so" +
                " the registry can be more organized! By removing these data, you can also get " +
                "better performance!";
                public static string nothingFound = "Nothing has been found! Your registry is clean.";
                public static string dataFound = "The analyzing is completed! In total there are ";
                public static string searching = "Searching for unused or any useless registry data.";
                public static string onRemoving = "Removing selected data";
                public static string success = "We have successfully removed ";
            }

            public static class LicenseWin
            {
                public static string invalidCode = "The inserted key is not valid!";
                public static string validCode = "Very well! You successfully upgraded to the Pro Version!";
            }
        }

        private static Task speechTask = Task.CompletedTask;
        private static readonly object speechLock = new object();

        /// <summary>
        /// This TTS runs under the Task which provides smoother and stable functionality than
        /// bald TTS class.
        /// Here are the key points which makes this work fine.
        /// <para/>
        /// Task.CompletedTask - Creates intentionally an "fake" task which will start the
        /// chain because we must run ContinueWith() which allow us to move onto other Task which
        /// will not be null. Without this "fake" or "null" task, we could not use ContinueWith().
        /// <para/>
        /// Lock - I have never used it before. This prevents task to overlap with other tasks.
        /// Simply it only gives permissions to the first task, the second task must wait until
        /// first task made its job.
        /// <para/>
        /// ContinueWith() - This function will run the task/code when the previous task is finished.
        /// <para/>
        /// Unwrap() - Because inside the function ContinueWith() we run the code within a new created Task
        /// we must use Unwrap. It combines these two Tasks (Outer & Inner) together. 
        /// So all tasks inside ContinueWith must be done as a whole thing.
        /// <para/>
        /// TaskScheduler.Default - This option will run the task on the background thread.
        /// If it was set to Current, it would try to run the task on UI thread which could slow down
        /// the entire Window (UI)
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        /*public static Task Speak(string script, bool wait = false, int volume = 100, int rate = 0, VoiceGender gender = VoiceGender.Female)
        {
            Task newTask;

            lock (speechLock)
            {
                if (!wait && !speechTask.IsCompleted)
                    return Task.CompletedTask;

                speechTask = speechTask.ContinueWith(_ => Task.Run(() => 
                {
                    using (var speech = new SpeechSynthesizer())
                    {
                        speech.SelectVoiceByHints(gender);
                        speech.Volume = volume;
                        speech.Rate = rate;
                        speech.Speak(script);
                    }
                } ), 
                    TaskScheduler.Default).Unwrap(); 

                newTask = speechTask;
            }

            return wait ? newTask : Task.CompletedTask;
        }*/

        private static SpeechSynthesizer speechInterrupted;
        public static void SpeakInterrupted(string script, int volume = 100, int rate = 0, VoiceGender gender = VoiceGender.Female)
        {
            if (speechInterrupted != null)
            {
                if (speechInterrupted.State == SynthesizerState.Speaking)
                {
                    speechInterrupted.SpeakAsyncCancelAll();
                    speechInterrupted.Dispose();
                }
            }

            speechInterrupted = new SpeechSynthesizer();
            speechInterrupted.Volume = volume;
            speechInterrupted.Rate = rate;
            speechInterrupted.SelectVoiceByHints(gender);
            speechInterrupted.SpeakAsync(script);
        }
    }
}
