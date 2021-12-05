using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace shootcraft.src.blocks
{
   public class WaterBlock : Block
   {
      public const float viscosity = 0.1f;
      public const int maxSaturation = 8;
      public int saturation = maxSaturation;

      public WaterBlock() : base() { }
      public WaterBlock(Vector2 pos) : base(pos) { }
      public WaterBlock(Vector2 pos, int saturation) : base(pos)
      {
         this.saturation = saturation;
      }

      public override void Update()
      {
         Block lCentB = World.GetBlock(pos, -1, +0);
         Block rCentB = World.GetBlock(pos, +1, +0);
         Block cUpppB = World.GetBlock(pos, +0, +1);

         if (!(lCentB is null) && lCentB is WaterBlock)
         {
            saturation = Math.Max((lCentB as WaterBlock).saturation - 1, saturation);
         }

         if (!(rCentB is null) && rCentB is WaterBlock)
         {
            saturation = Math.Max((rCentB as WaterBlock).saturation - 1, saturation);
         }

         if (!(cUpppB is null) && cUpppB is WaterBlock)
         {
            if(saturation < (cUpppB as WaterBlock).saturation)
            {
               saturation = (cUpppB as WaterBlock).saturation;
            }
         }
      }

      public override void Draw()
      {
         Block lCentB = World.GetBlock(pos, -1, +0);
         Block rCentB = World.GetBlock(pos, +1, +0);

         float lSat = saturation;
         float rSat = saturation;

         if (!(lCentB is null) && lCentB is WaterBlock)
            lSat = Math.Max((lCentB as WaterBlock).saturation, saturation);

         if (!(rCentB is null) && rCentB is WaterBlock)
            rSat = Math.Max((rCentB as WaterBlock).saturation, saturation);

         float left = pos.X - 0.5f;
         float right = pos.X + 0.5f;
         float center = pos.X;

         float bot = pos.Y - 0.5f;

         float topLeft = bot + 1.0f / maxSaturation * lSat;
         float topRight = bot + 1.0f / maxSaturation * rSat;
         float topCenter = bot + 1.0f / maxSaturation * saturation;

         GL.Color4(Color4.Blue);

         GL.Begin(PrimitiveType.Quads);

         GL.Vertex2(new Vector2(left, topLeft));
         GL.Vertex2(new Vector2(right, topRight));
         GL.Vertex2(new Vector2(right, bot));
         GL.Vertex2(new Vector2(left, bot));

         GL.End();

         //float left = pos.X - 0.5f;
         //float right = pos.X + 0.5f;
         //float center = pos.X;

         //float bot = pos.Y - 0.5f;

         //float topLeft = bot + 1.0f / maxSaturation * leftSatur;
         //float topRight = bot + 1.0f / maxSaturation * rightSatur;
         //float topCenter = bot + 1.0f / maxSaturation * saturation;

         //GL.Color4(Color4.Blue);

         //GL.Begin(PrimitiveType.Quads);

         //GL.Vertex2(new Vector2(left, topLeft));
         //GL.Vertex2(new Vector2(right, topRight));
         //GL.Vertex2(new Vector2(right, bot));
         //GL.Vertex2(new Vector2(left, bot));

         //GL.End();
      }
   }
}