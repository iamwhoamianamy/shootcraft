using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shootcraft.src.blocks;

using OpenTK;

namespace shootcraft.src.structures
{
   public class House : Structure
   {
      public House(int width, int height) : base(width, height) { }
      public static void Insert(House house, Vector2 shift)
      {
         for (int i = 0; i < house.Height; i++)
         {
            for (int j = 0; j < house.Width; j++)
            {
               if (house.blocks[i][j] != null)
               {
                  Block block = (Block)Activator.CreateInstance(house.blocks[i][j].GetType(), house.blocks[i][j].pos + shift);
                  World.SetBlock(block);
               }
            }
         }
      }

   }
}
