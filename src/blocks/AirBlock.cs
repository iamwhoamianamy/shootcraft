using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src.blocks
{
   public class AirBlock : Block
   {
      public AirBlock() : base() { }
      public AirBlock(Vector2 pos) : base(pos) { }

      public override void Draw()
      {

      }

      public override void Update()
      {
         bool doTransorm = false;
         int saturation = 0;

         Block lCentB = World.GetBlock(pos, -1, +0);
         Block rCentB = World.GetBlock(pos, +1, +0);

         Block lDownB = World.GetBlock(pos, -1, -1);
         Block rDownB = World.GetBlock(pos, +1, -1);

         Block cUpppB = World.GetBlock(pos, +0, +1);

         if(!(lCentB is null) && lCentB is WaterBlock &&
            !(lDownB is null) && !(lDownB is AirBlock || lDownB is WaterBlock))
         {
            doTransorm = true;
            saturation = Math.Max(saturation, (lCentB as WaterBlock).saturation - 1);
         }

         if (!(rCentB is null) && rCentB is WaterBlock &&
             !(rDownB is null) && !(rDownB is AirBlock || rDownB is WaterBlock))
         {
            doTransorm = true;
            saturation = Math.Max(saturation, (rCentB as WaterBlock).saturation - 1);
         }

         if(!(cUpppB is null) &&  cUpppB is WaterBlock)
         {
            doTransorm = true;
            saturation = WaterBlock.maxSaturation;
         }

         if (doTransorm && saturation > 0)
         {
            WaterBlock waterBlock = new WaterBlock(pos, saturation);

            World.SetBlock(waterBlock);
         }
      }
   }
}
