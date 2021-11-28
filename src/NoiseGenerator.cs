using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src
{
   public class NoiseGenerator
   {
      public int length = 10000;
      public List<int> perlinNoise;

      int nOctaveCount = 9;
      double fScalingBias = 0.05;

      public NoiseGenerator()
      {
         List<double> noiseSeed;
         noiseSeed = new List<double>();
         perlinNoise = new List<int>();
         Random rand = new Random();

         for (int i = 0; i < length; i++)
            noiseSeed.Add(rand.NextDouble());

         PerlinNoise1D(noiseSeed, nOctaveCount, fScalingBias);

         for (int i = 0; i < length; i++)
            Console.WriteLine(perlinNoise[i].ToString());
      }

      private void PerlinNoise1D(List<double> noiseSeed, int octavesCount, double bias)
      {
         for (int x = 0; x < length; x++)
         {
            double noise = 0.0;
            double scaleAcc = 0.0;
            double scale = 1.0;

            for (int o = 0; o < octavesCount; o++)
            {
               int pitch = length >> o;
               int sample1 = (x / pitch) * pitch;
               int sample2 = (sample1 + pitch) % length;

               double blend = (double)(x - sample1) / (double)pitch;

               double sample = (1.0 - blend) * noiseSeed[sample1] + blend * noiseSeed[sample2];

               scaleAcc += scale;
               noise += sample * scale;
               scale = scale / bias;
            }

            perlinNoise.Add((int)((noise / scaleAcc) * 15) + 10);
         }
      }

   }
}
