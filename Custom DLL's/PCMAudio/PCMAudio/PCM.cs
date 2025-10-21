using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCMAudio
{
    public static class PCM
    {
        public enum WaveForms : int
        {
            Sine = 0,
            Square = 1,
            Saw = 2,
            Triangle = 3,
            Noise = 4
        }

        public static BinaryWriter GenerateWave(Stream memoryStream, float[] freqs, int duration,
            int tempo, double volume, int SampleRate, short NumChannels, short BitsPerSample, WaveForms[] waveForms)
        {
            var bw = new BinaryWriter(memoryStream);

            byte[] ChunkID = Encoding.ASCII.GetBytes("RIFF");
            int NumSamples = SampleRate * duration;
            int Subchunk2Size = NumSamples * NumChannels * BitsPerSample / 8;
            int ChunkSize = 32 + Subchunk2Size;
            byte[] Format = Encoding.ASCII.GetBytes("WAVE");
            byte[] Subchunk1ID = Encoding.ASCII.GetBytes("fmt ");
            int Subchunk1Size = 16;
            short AudioFormat = 1;
            int ByteRate = SampleRate * NumChannels * BitsPerSample / 8;
            short BlockAlign = (short)(NumChannels * BitsPerSample / 8);
            byte[] Subchunk2ID = Encoding.ASCII.GetBytes("data");
            double amplitude = volume * short.MaxValue;

            bw.Write(ChunkID);
            bw.Write(ChunkSize);
            bw.Write(Format);
            bw.Write(Subchunk1ID);
            bw.Write(Subchunk1Size);
            bw.Write(AudioFormat);
            bw.Write(NumChannels);
            bw.Write(SampleRate);
            bw.Write(ByteRate);
            bw.Write(BlockAlign);
            bw.Write(BitsPerSample);
            bw.Write(Subchunk2ID);
            bw.Write(Subchunk2Size);

            short[] data = new short[NumSamples * NumChannels];

            int nextFreqIndex = 0;
            float freq = freqs[nextFreqIndex];
            int tempoCount = 0;


            foreach (var waveForm in waveForms)
            {
                for (int i = 0; i < NumSamples * NumChannels; i++)
                {
                    if (tempoCount == tempo)
                    {
                        nextFreqIndex++;
                        if (nextFreqIndex == freqs.Length)
                            nextFreqIndex = 0;
                        freq = freqs[nextFreqIndex];
                        tempoCount = 0;
                    }

                    data[i] += (short)(Waves.DetermineWave(amplitude, freq, i, waveForm, SampleRate) / waveForms.Length);

                    tempoCount++;
                }
            }

            byte[] buffer = new byte[NumSamples * NumChannels * sizeof(short)];

            Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);

            bw.Write(buffer);

            return bw;
        }

        public static class Waves
        {
            private static readonly Random rand = new Random();
            public static short DetermineWave(double amplitude, float freq, int sampleIndex, WaveForms waveForm, int SampleRate)
            {
                short wave = 0;
                double t = sampleIndex / (double)SampleRate;
                double angle = 2 * Math.PI * freq * t;

                switch (waveForm)
                {
                    case WaveForms.Sine:
                        wave = Convert.ToInt16(amplitude * Math.Sin(angle));
                        break;

                    case WaveForms.Square:
                        wave = Convert.ToInt16(amplitude * Math.Sign(Math.Sin(angle)));
                        break;

                    case WaveForms.Saw:
                        wave = Convert.ToInt16(amplitude * (2.0 * (t * freq - Math.Floor(0.5 + t * freq))));

                        break;

                    case WaveForms.Triangle:
                        wave = Convert.ToInt16(amplitude * (2.0 * Math.Abs(2.0 *
                                (t * freq - Math.Floor(t * freq + 0.5))) - 1.0));

                        break;
                    case WaveForms.Noise:

                        double noise = rand.NextDouble() * 2.0 - 1.0;
                        wave = (short)((amplitude * 0.00001) * short.MaxValue * noise);

                        break;
                }

                return wave;
            }
        }
    }
}
