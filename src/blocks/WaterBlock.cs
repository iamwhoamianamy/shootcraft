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
   }
}