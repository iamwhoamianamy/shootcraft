using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shootcraft.src.blocks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace shootcraft.src
{
   public class Chunk
   {
      private Block[][] _blocks;

      public int Index { get; private set; }
      public float StartX { get; private set; }

      public static int blockCountX = 4;
      public static int blockCountY = 60;

      public Chunk(int index)
      {
         Index = index;
         StartX = index * Chunk.blockCountX * Block.width;

         _blocks = new Block[blockCountY][];

         for (int i = 0; i < blockCountY; i++)
         {
            _blocks[i] = new Block[blockCountX];
         }

         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               Position block_pos = new Position((int)(StartX + j * Block.width),
                                                 (int)(i * Block.width));

               if(block_pos.Y < 150)
                  _blocks[i][j] = new DirtBlock(block_pos);
               else
                  _blocks[i][j] = new AirBlock(block_pos);
            }
         }
      }

      public void DrawBorders()
      {
         GL.Color4(Color4.Black);

         GL.Begin(PrimitiveType.LineLoop);

         GL.Vertex2(StartX, 0);
         GL.Vertex2(StartX, blockCountY * Block.width);
         GL.Vertex2(StartX + blockCountX * Block.width, blockCountY * Block.width);
         GL.Vertex2(StartX + blockCountX * Block.width, 0);

         GL.End();
      }

      public bool ContainsBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)(pos.X / Block.width) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Min(blockCountY - 1, Math.Max(pos.Y / Block.width, 0)) + offsetY;

         if (0 <= block_x_id && block_x_id < blockCountX &&
             0 <= block_y_id && block_y_id < blockCountY)
            return true;
         else
            return false;
      }

      public void SetBlock(Vector2 pos, Block block, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)(pos.X / Block.width) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Min(blockCountY - 1, Math.Max(pos.Y / Block.width, 0)) + offsetY;

         if (block_x_id > 0 || block_y_id > 0)
         _blocks[block_y_id][block_x_id] = block;
      }

      public Block GetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)(pos.X / Block.width) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Min(blockCountY - 1, Math.Max(pos.Y / Block.width, 0)) + offsetY;
         
         if(block_x_id < 0 || block_y_id < 0)
            return _blocks[0][0];
         else
            return _blocks[block_y_id][block_x_id];
      }

      public void DrawAllBlocks()
      {
         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               _blocks[i][j].Draw();
            }
         }
      }
   }
}
