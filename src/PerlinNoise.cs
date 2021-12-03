using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Newtonsoft.Json;

namespace shootcraft.src
{
   [JsonObject(MemberSerialization.OptIn)]
   public class PerlinNoise
   {
      [JsonProperty]
      public List<int> values;

      public int Length => values.Count;

      public PerlinNoise() { }

      public PerlinNoise(int length, int octavesCount = 9, double bias = 0.05)
      {
         List<double> noiseSeed;
         noiseSeed = new List<double>();
         values = new List<int>();
         Random rand = new Random();

         for (int i = 0; i < length; i++)
            noiseSeed.Add(rand.NextDouble());

         PerlinNoise1D(noiseSeed, octavesCount, bias);
      }

      private void PerlinNoise1D(List<double> noiseSeed, int octavesCount, double bias)
      {
         for (int x = 0; x < noiseSeed.Count; x++)
         {
            double noise = 0.0;
            double scaleAcc = 0.0;
            double scale = 1.0;

            for (int o = 0; o < octavesCount; o++)
            {
               int pitch = noiseSeed.Count >> o;
               int sample1 = x / pitch * pitch;
               int sample2 = (sample1 + pitch) % noiseSeed.Count;

               double blend = (double)(x - sample1) / pitch;

               double sample = (1.0 - blend) * noiseSeed[sample1] + blend * noiseSeed[sample2];

               scaleAcc += scale;
               noise += sample * scale;
               scale /= bias;
            }
           
            values.Add((int)(noise / scaleAcc * 15) + 10);
         }
      }

   }
}
