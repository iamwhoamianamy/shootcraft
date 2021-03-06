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
   public class Inventory
   {
      public const int cellCountX = 10;
      public const int cellCountY = 5;
      public List<InventoryCell> Cells { get; private set; }
      public static Rectangle DrawingRectangle { get; set; }
      public int? ActiveCell { get; set; }

      public Inventory()
      {
         Cells = new List<InventoryCell>();

         for (int i = 1; i < ClassesHandler.Blocks.Count; i++)
         {
            Block block = (Block)Activator.CreateInstance(ClassesHandler.Blocks[i]);
            Cells.Add(new InventoryCell(new Item(block), 1, i - 1));
         }

         for (int i = ClassesHandler.Blocks.Count - 1; i < cellCountX * cellCountY; i++)
         {
            Cells.Add(new InventoryCell(new Item(), 1, i));
         }
      }

      public void Draw()
      {
         for (int i = 0; i < Cells.Count; i++)
         {
            Cells[i].Draw();
         }

         if(ActiveCell is not null)
         {
            var rectangle = Cells[ActiveCell.Value].DrawingRectangle;
            rectangle.DrawOutline(new Color4(200, 42, 42, 255), 3);
         }
      }

      public static int? CellIdByPos(int x, int y)
      {
         int newX = (int)((x - DrawingRectangle.Left) / InventoryCell.Width);
         int newY = (int)((y - DrawingRectangle.Bot) / InventoryCell.Width);

         if (0 <= newX && 0 <= newY && newX < cellCountX && newY < cellCountY)
         {
            return newX + newY * cellCountX;
         }
         else
            return null;
      }
   }
}
