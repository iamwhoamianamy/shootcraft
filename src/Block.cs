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

namespace shootcraft.src
{
   [JsonObject(MemberSerialization.OptIn)]
   public class Block : IBlockRegisterable
   {
      public Vector2 pos;

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

         GL.Color4(Color4.Black);
         GL.Begin(PrimitiveType.LineLoop);

         for (int i = 0; i < 4; i++)
         {
            GL.Vertex2(rect[i]);
         }

         GL.End();
      }

      public virtual void Draw()
      {
         GL.Color4(Color4.White);

         Rectangle rect = GetRectangle();

         GL.BindTexture(TextureTarget.Texture2D,
            TexturesHandler.blockTextures[GetType()]);

         GL.Enable(EnableCap.Texture2D);
         GL.Begin(PrimitiveType.Quads);

         GL.TexCoord2(new Vector2(0, 0));
         GL.Vertex2(rect.rightTop.X, rect.rightTop.Y);

         GL.TexCoord2(new Vector2(1, 0));
         GL.Vertex2(rect.leftTop.X, rect.leftTop.Y);

         GL.TexCoord2(new Vector2(1, 1));
         GL.Vertex2(rect.leftBot.X, rect.leftBot.Y);

         GL.TexCoord2(new Vector2(0, 1));
         GL.Vertex2(rect.rightBot.X, rect.rightBot.Y);

         GL.End();
         GL.Disable(EnableCap.Texture2D);
      }

      public Rectangle GetRectangle()
      {
         return new Rectangle(pos, 1.0f);
      }

      public virtual void Update() { }

      public virtual void ForcedUpdate() { }
   }
}
