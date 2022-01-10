using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using Newtonsoft.Json;

namespace shootcraft.src.blocks
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WaterBlock : Block, IPassableBlock
   {
      public const float viscosity = 0.1f;
      public const double evaporationRate = 0.5;
      public const int maxSaturation = 16;
      [JsonProperty]
      public int saturation = maxSaturation;
      [JsonProperty]
      public bool isConnectedToSource = true;
      [JsonProperty]
      public bool isSource = true;

      public WaterBlock() : base() { }
      public WaterBlock(Vector2 pos) : base(pos) { }
      public WaterBlock(Vector2 pos, int saturation) : base(pos)
      {
         this.saturation = saturation;

         if(saturation < maxSaturation)
         {
            isConnectedToSource = true;
            isSource = false;
         }
      }

      public WaterBlock(WaterBlock block) : base(block.pos)
      {
         saturation = block.saturation;
         isConnectedToSource = block.isConnectedToSource;
         isSource = block.isSource;
      }

      private bool CheckForConnectionToSourceAtSide(Block sideBlock)
      {
         return sideBlock is not null && sideBlock is WaterBlock swb &&
                 (swb.isSource || (swb.isConnectedToSource && saturation < swb.saturation));
      }

      private bool CheckForConnectionToSourceAtTop()
      {
         Block upperBlock = World.GetBlock(pos, +0, +1);

         return upperBlock is not null && upperBlock is WaterBlock uwb &&
                (uwb.isSource || uwb.isConnectedToSource);
      }

      private bool CheckForConnectionToSource()
      {
         Block leftBlock = World.GetBlock(pos, -1, +0);
         Block rightBlock = World.GetBlock(pos, +1, +0);

         return CheckForConnectionToSourceAtSide(leftBlock) ||
                CheckForConnectionToSourceAtSide(rightBlock) ||
                CheckForConnectionToSourceAtTop();
      }

      private void UpdateSaturation()
      {
         Block leftBlock = World.GetBlock(pos, -1, +0);
         Block rightBlock = World.GetBlock(pos, +1, +0);
         Block upperBlock = World.GetBlock(pos, +0, +1);

         if (leftBlock is not null && leftBlock is WaterBlock lwb)
         {
            saturation = Math.Max(lwb.saturation - 1, saturation);
         }

         if (rightBlock is not null && rightBlock is WaterBlock rwb)
         {
            saturation = Math.Max(rwb.saturation - 1, saturation);
         }

         if (upperBlock is not null && upperBlock is WaterBlock uwb)
         {
            saturation = WaterBlock.maxSaturation;
         }
      }

      private bool CheckForColumn()
      {
         Block leftBlock = World.GetBlock(pos, -1, +0);
         Block rightBlock = World.GetBlock(pos, +1, +0);

         return leftBlock is not null && leftBlock is not WaterBlock &&
                 rightBlock is not null && rightBlock is not WaterBlock;
      }

      public override void Update()
      {
         if(saturation <= 0)
         {
            World.BlocksToUpdate.Add(new AirBlock(pos));
         }
         else
         {
            if (!isSource)
            {
               if (CheckForConnectionToSource() && isConnectedToSource)
               {
                  WaterBlock newMe = new WaterBlock(this);
                  newMe.UpdateSaturation();
                  World.BlocksToUpdate.Add(newMe);
               }
               else
               {
                  if (CheckForColumn())
                  {
                     WaterBlock newMe = new WaterBlock(this);
                     newMe.EvaporateColumn();
                     World.BlocksToUpdate.Add(newMe);
                  }
                  else
                  {
                     WaterBlock newMe = new WaterBlock(this);
                     newMe.Evaporate();
                     World.BlocksToUpdate.Add(newMe);
                  }
               }
            }
         }
      }

      private void EvaporateColumn()
      {
         saturation -= maxSaturation / 2;
      }

      private void Evaporate()
      {
         if (World.RNG.NextDouble() < evaporationRate)
            saturation--;
      }

      public override void Draw()
      {
         Block leftBlock = World.GetBlock(pos, -1, +0);
         Block rightBlock = World.GetBlock(pos, +1, +0);

         float lSat = saturation;
         float rSat = saturation;

         if (leftBlock is not null && leftBlock is WaterBlock lwb)
            lSat = Math.Max(lwb.saturation, saturation);

         if (rightBlock is not null && rightBlock is WaterBlock rwb)
            rSat = Math.Max(rwb.saturation, saturation);

         float left = pos.X - 0.5f;
         float right = pos.X + 0.5f;

         float bot = pos.Y - 0.5f;

         float topLeft = bot + 1.0f / maxSaturation * lSat;
         float topRight = bot + 1.0f / maxSaturation * rSat;

         GL.Color4(Color4.Blue);

         GL.Begin(PrimitiveType.Quads);

         GL.Vertex2(new Vector2(left, topLeft));
         GL.Vertex2(new Vector2(right, topRight));
         GL.Vertex2(new Vector2(right, bot));
         GL.Vertex2(new Vector2(left, bot));

         GL.End();
      }
   }
}