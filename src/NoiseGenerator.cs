using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src
{
   class NoiseGenerator
   {
      public int nOutputSize = 10000;
      public List<double> fNoiseSeed1D;
      public List<int> fPerlinNoise1D;

      int nOctaveCount = 9;
      double fScalingBias = 0.05f;

      public NoiseGenerator()
      {
         fNoiseSeed1D = new List<double>();
         fPerlinNoise1D = new List<int>();
         NoiseSeed();
         PerlinNoise1D(nOutputSize, fNoiseSeed1D, nOctaveCount, fScalingBias, fPerlinNoise1D);
         for (int i = 0; i < nOutputSize; i++)
            Console.WriteLine(fPerlinNoise1D[i].ToString());
      }

      public void NoiseSeed()
      {
         Random rand = new Random();
         for (int i = 0; i < nOutputSize; i++)
            fNoiseSeed1D.Add(rand.NextDouble());
      }

      public void PerlinNoise1D(int nCount, List<double> fSeed, int nOctaves, double fBias, List<int> fOutput)
      {
         for (int x = 0; x < nCount; x++)
         {
            double fNoise = 0.0f;
            double fScaleAcc = 0.0f;
            double fScale = 1.0f;

            for (int o = 0; o < nOctaves; o++)
            {
               int nPitch = nCount >> o;
               int nSample1 = (x / nPitch) * nPitch;
               int nSample2 = (nSample1 + nPitch) % nCount;

               double fBlend = (double)(x - nSample1) / (double)nPitch;

               double fSample = (1.0f - fBlend) * fSeed[nSample1] + fBlend * fSeed[nSample2];

               fScaleAcc += fScale;
               fNoise += fSample * fScale;
               fScale = fScale / fBias;
            }

            fOutput.Add((int)((fNoise / fScaleAcc) * 15));
         }
      }

   }
}
