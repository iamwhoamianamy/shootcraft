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

   }
}
