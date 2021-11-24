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
      public DirtBlock() : base() { }
      public DirtBlock(Vector2 pos) : base(pos, OpenTK.Graphics.Color4.Pink) { }
   }
}
