using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using static PCMAudio.PCM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace RogueAntivirusPatched.Classes
{
    public class Beats
    {
        Random rand;
        public SoundPlayer soundPlayer;

        public float[] melody_its_beginning = { 30f, 40f, 50f, 40f, 30f, 20f, 20f, 20f, 60f, 70f, 80f };

        public float[] melody_bliss =
        {
            50f, 50f, 40f, 40f, 40f,
            50f, 60f, 61f, 62f, 70f, 70f, 70f,
            50f, 50f, 50f, 30f, 30f, 30f,
            80f, 80f, 70f, 70f, 60f, 60f, 60f,
            70f, 70f, 80f, 80f, 80f, 70f, 60f, 50f
        };

        public float[] melody_getting_into =
        {
            200f, 200f, 220f, 220f, 240f, 240f, 220f, 220f,
            250f, 250f, 250f, 250f, 260f, 260f, 260f, 260f,
            270f, 270f, 270f, 270f, 280f, 280f, 280f, 280f
        };

        public float[] melody_messy()
        {
            rand = new Random();

            var listFreqs = new List<float>();

            for (int i = 0; i < rand.Next(100, 300); i++)
            {
                listFreqs.Add(rand.Next(30, 400));
            }

            return listFreqs.ToArray();
        }

        public float[] gibbersih()
        {
            rand = new Random();

            var listFreqs = new List<float>();

            for (int i = 0; i < rand.Next(100, 500); i++)
            {
                listFreqs.Add(rand.Next(255));
            }

            return listFreqs.ToArray();
        }

        public async Task PCMAudio(float[] freqs, WaveForms[] waves, int temp, double amplitude, int duration)
        {
            await Task.Run(() =>
            {
                var memory = new MemoryStream();
                var bw = GenerateWave(memory, freqs, duration, temp, amplitude, 16000, 2, 16, waves);

                memory.Position = 0;

                soundPlayer = new SoundPlayer(memory);
                soundPlayer.PlayLooping();

                soundPlayer.Dispose();
                bw.Dispose();
                memory.Dispose();
            });
        }
    }
}
