using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src.blocks
{
   public class WaterBlock : Block
   {
      public const float viscosity = 0.1f;
      public WaterBlock() : base() { }
      public WaterBlock(Vector2 pos) : base(pos) { }

      public override void Update()
      {
         List<Block> blocks = new List<Block>();

         blocks.Add(World.TryGetBlock(pos, -1, 0));
         blocks.Add(World.TryGetBlock(pos, +1, 0));
         blocks.Add(World.TryGetBlock(pos, 0, -1));

         foreach (var block in blocks)
         {
            if (!(block is null) && block is AirBlock)
            {
               World.SetBlock(new WaterBlock(block.pos));
            }
         }

         //for (float y = pos.Y - 1; y < pos.Y + 1; y++)
         //{
         //   for (float x = pos.X - 1; x < pos.X + 2; x++)
         //   {
         //      Vector2 blockPos = new Vector2(x, y);

         //      if(World.TryGetBlock(blo))
         //   }
         //}
      }
   }
}