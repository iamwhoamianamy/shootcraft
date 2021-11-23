using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace shootcraft.src
{
   public class Cursor
   {
      public Vector2 pos;
      public float scale = 1.0f;
      public float size = 10.0f;

      public Cursor()
      {
         pos = Vector2.Zero;
      }
      public void Draw()
      {
         GL.Color4(Color4.Black);
         GL.Begin(PrimitiveType.Lines);

         GL.Vertex2(pos.X, pos.Y + scale * size);
         GL.Vertex2(pos.X, pos.Y - scale * size);

         GL.Vertex2(pos.X + scale * size, pos.Y);
         GL.Vertex2(pos.X - scale * size, pos.Y);

         GL.End();
      }
   }
}
