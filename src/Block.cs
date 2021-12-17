using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using Newtonsoft.Json;
using shootcraft.src.blocks;

namespace shootcraft.src
{
   [JsonObject(MemberSerialization.OptIn)]
   public class Block : IBlockRegisterable
   {
      public Vector2 pos;
      public const int maxLightLevel = 8;

      private int lightLevel = 0;
      public int LightLevel
      {
         get
         {
            return lightLevel;
         }
         set
         {
            lightLevel = Math.Max(0, Math.Min(value, maxLightLevel));
         }
      }

      public Block()
      {
         pos = Vector2.Zero;
      }

      public Block(Vector2 pos)
      {
         this.pos = pos;
      }

      public void DrawBorders()
      {
         Rectangle rect = GetRectangle();

         GL.Begin(PrimitiveType.LineLoop);

         for (int i = 0; i < 4; i++)
         {
            GL.Vertex2(rect[i]);
         }

         GL.End();
      }

      public virtual void Draw()
      {

         Rectangle rect = GetRectangle();

         GL.BindTexture(TextureTarget.Texture2D,
            TexturesHandler.blockTextures[GetType()]);

         rect.DrawTexture();

         byte light = (byte)(255 / maxLightLevel * LightLevel);
         rect.DrawColor(new Color4(0, 0, 0, (byte)(255 - light)));
         //GL.BlendFunc(BlendingFactor.);
      }

      public Rectangle GetRectangle()
      {
         return new Rectangle(pos, 1.0f);
      }

      public virtual void Update() { }

      //public virtual void Destroy()
      //{
      //   World.SetBlock(new AirBlock(pos));
      //   World.UpdateLightingInBlockChunk(this);
      //}

      public virtual void ForcedUpdate() { }
   }
}
