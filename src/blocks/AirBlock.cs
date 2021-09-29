using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src.blocks
{
   public class AirBlock : Block
   {
      public AirBlock(Position pos) : base(pos, OpenTK.Graphics.Color4.AliceBlue) { }
   }
}
