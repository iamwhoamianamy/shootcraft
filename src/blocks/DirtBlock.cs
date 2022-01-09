using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src.blocks
{
   public class DirtBlock : Block
   {
      public const double growthRate = 0.01;
      public DirtBlock() : base() { }
      public DirtBlock(Vector2 pos) : base(pos) { }

      public override void Update()
      {
         if(World.RNG.NextDouble() < growthRate)
         {
            for (int i = 1; i < Chunk.blockCountY; i++)
            {
               Block block = World.GetBlock(pos, 0, i);

               if (block is not null && block is not AirBlock)
                  return;
            }

            World.BlocksToUpdate.Add(new GrassBlock(pos));
         }
      }
   }
}
