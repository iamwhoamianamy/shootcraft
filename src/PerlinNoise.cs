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

      [JsonProperty]
      public int Length { get; private set; }
      [JsonProperty]
      public double Shift { get; private set; }
      [JsonProperty]
      public int OctavesCount { get; private set; }
      [JsonProperty]
      public double Bias { get; private set; }

      public PerlinNoise() { }

      public PerlinNoise(int length, double shift, int octavesCount, double bias)
      {
         values = new List<int>();
         Shift = shift;
         OctavesCount = octavesCount;
         Bias = bias;
         Length = length;

         GenerateNoise();
      }

      private void GenerateNoise()
      {
         List<double> noiseSeed = new List<double>();

         Random rand = new Random();

         for (int i = 0; i < Length; i++)
            noiseSeed.Add(rand.NextDouble());

         for (int x = 0; x < noiseSeed.Count; x++)
         {
            double noise = 0.0;
            double scaleAcc = 0.0;
            double scale = 1.0;

            for (int o = 0; o < OctavesCount; o++)
            {
               int pitch = noiseSeed.Count >> o;
               int sample1 = x / pitch * pitch;
               int sample2 = (sample1 + pitch) % noiseSeed.Count;

               double blend = (double)(x - sample1) / pitch;

               double sample = (1.0 - blend) * noiseSeed[sample1] + blend * noiseSeed[sample2];

               scaleAcc += scale;
               noise += sample * scale;
               scale /= Bias;
            }
           
            values.Add((int)(noise / scaleAcc * 15) + (int)Shift);
         }
      }

   }
}
