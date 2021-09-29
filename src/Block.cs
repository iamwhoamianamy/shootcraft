using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace shootcraft.src
{
   public abstract class Block
   {
      public Position pos;
      public Color4 color;

      public static float width = 20.0f;

      public Block(Position pos, Color4 color)
      {
         this.pos = pos;
         this.color = color;
      }

      public void DrawBorders()
      {
         GL.Color4(Color4.Black);

         GL.Begin(PrimitiveType.LineLoop);

         GL.Vertex2(pos.X, pos.Y);
         GL.Vertex2(pos.X + width, pos.Y);
         GL.Vertex2(pos.X + width, pos.Y + width);
         GL.Vertex2(pos.X, pos.Y + width);

         GL.End();
      }

      public void Draw()
      {
         GL.Color4(color);

         GL.Begin(PrimitiveType.Quads);

         GL.Vertex2(pos.X, pos.Y);
         GL.Vertex2(pos.X + width, pos.Y);
         GL.Vertex2(pos.X + width, pos.Y + width);
         GL.Vertex2(pos.X, pos.Y + width);

         GL.End();
      }
   }
}
