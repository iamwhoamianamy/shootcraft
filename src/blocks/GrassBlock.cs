using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src.blocks
{
   public class GrassBlock : Block
   {
      public GrassBlock() : base() { }
      public GrassBlock(Vector2 pos) : base(pos) { }

      public override void Update()
      {
         Block block = World.GetBlock(pos, 0, 1);

         if (!(block is null) && !(block is AirBlock))
            World.SetBlock(new DirtBlock(pos));
      }
   }
}
