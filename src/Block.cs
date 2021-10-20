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
      public Vector2 pos;
      public Color4 color;

      public static float width = 20.0f;

      //public Block(Block block)
      //{
      //   pos = block.pos;
      //}

      public Block(Vector2 pos, Color4 color)
      {
         this.pos = pos;
         this.color = color;
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

      public void Draw()
      {
         Rectangle rect = GetRectangle();

         GL.Color4(color);
         GL.Begin(PrimitiveType.Quads);

         for (int i = 0; i < 4; i++)
         {
            GL.Vertex2(rect[i]);
         }

         GL.End();
      }

      public Rectangle GetRectangle()
      {
         return new Rectangle(pos, width);
      }
   }
}
