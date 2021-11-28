using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shootcraft.src.blocks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using Newtonsoft.Json;

namespace shootcraft.src
{
   [JsonObject(MemberSerialization.OptIn)]
   public class Chunk
   {
      [JsonProperty]
      private Block[][] _blocks;

      [JsonProperty]
      public int Index { get; private set; }
      [JsonProperty]
      public int StartX { get; private set; }

      public static int blockCountX = 8;
      public static int blockCountY = 60;

      public Chunk(int index)
      {
         InitChunk(index);
         ParamChunk(World.perlinNoise);
      }

      public void InitChunk(int index)
      {
         Index = index;
         StartX = index * blockCountX;
         _blocks = new Block[blockCountY][];
         for (int i = 0; i < blockCountY; i++)
            _blocks[i] = new Block[blockCountX];
      }

      public void ParamChunk(PerlinNoise perlinNoise)
      {
         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               Vector2 blockPos = new Vector2(StartX + j + 0.5f, i + 0.5f);
               int PerlinSurface = World.PerlinValueForX(blockPos.X);

               if (blockPos.Y < PerlinSurface)
               {
                  _blocks[i][j] = new DirtBlock(blockPos);
               }
               else if ((int)(blockPos.Y) < 16)
               {
                  _blocks[i][j] = new WaterBlock(blockPos);

                  for (int y = -1; y < 1; y++)
                  {
                     for (int x = -2; x < 4; x++)
                     {
                        int xIdx = j + x;
                        int yIdx = i + y;

                        if (xIdx >= 0 && xIdx < blockCountX &&
                            yIdx >= 0 && yIdx < blockCountY &&
                           _blocks[yIdx][xIdx]?.GetType() == typeof(DirtBlock))
                           _blocks[yIdx][xIdx] = new SandBlock(_blocks[i + y][j + x].pos);
                     }
                  }
               }
               else
                  _blocks[i][j] = new AirBlock(blockPos);
            }
         }

         for (int x = 0; x < blockCountX; x++)
         {
            int perlinValue = World.PerlinValueForX(StartX + x) - 1;

            if(_blocks[perlinValue][x] is DirtBlock)
               _blocks[perlinValue][x] = new GrassBlock(_blocks[perlinValue][x].pos);
         }
      }

      public void RestoreBlocks(int index)
      {
         Index = index;
         StartX = index * blockCountX;

         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               _blocks[i][j].pos = new Vector2(StartX + j + 0.5f, i + 0.5f);
            }
         }
      }

      public void DrawBorders()
      {
         GL.Color4(Color4.Black);

         GL.Begin(PrimitiveType.LineLoop);

         GL.Vertex2(StartX, 0);
         GL.Vertex2(StartX, blockCountY);
         GL.Vertex2(StartX + blockCountX, blockCountY);
         GL.Vertex2(StartX + blockCountX, 0);

         GL.End();
      }

      public bool ContainsBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)(pos.X) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Min(blockCountY - 1, Math.Max(pos.Y, 0)) + offsetY;

         if (0 <= block_x_id && block_x_id < blockCountX &&
             0 <= block_y_id && block_y_id < blockCountY)
            return true;
         else
            return false;
      }

      public void SetBlock(Vector2 pos, Block block, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)Math.Floor(pos.X) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Floor(Math.Min(blockCountY - 1, Math.Max(pos.Y, 0))) + offsetY;

            _blocks[block_y_id][block_x_id] = block;
      }

      public Block GetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)Math.Floor(pos.X) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Floor(Math.Min(blockCountY - 1, Math.Max(pos.Y, 0))) + offsetY;

         block_y_id = Math.Min(Math.Max(0, block_y_id), blockCountY - 1);

         return _blocks[block_y_id][block_x_id];
      }

      public void DrawAllBlocks()
      {
         GL.Color4(Color4.White);

         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               //_blocks[i][j].DrawColor();
               _blocks[i][j].DrawTexture();

            }
         }
      }
   }
}
