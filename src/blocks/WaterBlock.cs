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
      public static float viscosity = 0.96f;
      public WaterBlock() : base() { }
      public WaterBlock(Vector2 pos) : base(pos) { }
   }
}