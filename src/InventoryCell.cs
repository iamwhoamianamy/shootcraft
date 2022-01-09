using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace shootcraft.src
{
   public class InventoryCell
   {
      public Item Item { get; set; }
      public int Count { get; set; }
      public int Id { get; private set; }
      public int X => Id % Inventory.cellCountX;
      public int Y => (int)Math.Floor((float)Id / Inventory.cellCountX);

      public static float Width => Inventory.DrawingRectangle.Width / Inventory.cellCountX;
      public Rectangle DrawingRectangle
      {
         get
         {
            float width = Width;
            Vector2 cellCenter =
                  Inventory.DrawingRectangle.leftBot +
                  new Vector2(width / 2) +
                  new Vector2(width * X, width * Y);

            return new Rectangle(cellCenter, width);
         }
      }

      public InventoryCell(Item item, int count, int id)
      {
         this.Item = item;
         Count = count;
         Id = id;
      }

      public void Draw()
      {
         Rectangle rect = DrawingRectangle;

         DrawBackground(rect);
         Item.Draw(rect);
         rect.DrawOutline(Color4.Black, 3);
      }

      private void DrawBackground(Rectangle rect)
      {
         GL.Color4(new Color4(100, 42, 42, 255));

         GL.Begin(PrimitiveType.Quads);
         {
            GL.Vertex2(rect.leftTop);
            GL.Vertex2(rect.rightTop);
            GL.Vertex2(rect.rightBot);
            GL.Vertex2(rect.leftBot);
         }
         GL.End();
      }
   }
}
