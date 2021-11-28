using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src
{
   public class Structure
   {
      public Block[][] blocks;
      public int Width { get; protected set; }
      public int Height { get; protected set; }
      public Structure(int width, int height)
      {
         Width = width;
         Height = height;
         blocks = new Block[Height][];

         for (int i = 0; i < Height; i++)
            blocks[i] = new Block[Width];
      }

      public static Structure FromFile(string file)
      {
         Structure s;
         List<string> blockSignatures = new List<string>();
         using (StreamReader sr = new StreamReader(file))
         {
            while (!sr.EndOfStream)
            {
               blockSignatures.Add(sr.ReadLine());
            }
         }
         s = new Structure(blockSignatures[0].Length, blockSignatures.Count);

         //for (int i = 0; i < s.Height; i++)
         //{
         //   for (int j = 0; j < s.Width; j++)
         //   {
         //      s.blocks[i][j] = 
         //   }
         //}
         return s;
      }

   }
}
