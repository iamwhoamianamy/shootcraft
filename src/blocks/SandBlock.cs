using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace shootcraft.src.blocks
{
   class SandBlock : Block
   {
      public SandBlock() : base() { }
      public SandBlock(Vector2 pos) : base(pos) { }

      public override void Update()
      {
         Block blockUnder = World.GetBlock(pos, 0, -1);
         if (blockUnder is not null && blockUnder is IPassableBlock)
         {
            World.BlocksToUpdate.Add(new AirBlock(pos));
            World.BlocksToUpdate.Add(new SandBlock(blockUnder.pos));
         }
      }
   }
}
