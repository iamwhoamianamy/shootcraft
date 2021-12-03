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
      private Block[][] blocks;

      [JsonProperty]
      public int Index { get; private set; }
      [JsonProperty]
      public int StartX { get; private set; }

      public static int blockCountX = 8;
      public static int blockCountY = 60;

      public Chunk(int index)
      {
         InitChunk(index);
         GenerateBasicTerrain();
         SpawnSand();
         SpawnGrass();
      }

      public void InitChunk(int index)
      {
         Index = index;
         StartX = index * blockCountX;
         blocks = new Block[blockCountY][];

         for (int i = 0; i < blockCountY; i++)
            blocks[i] = new Block[blockCountX];
      }

      public void GenerateBasicTerrain()
      {
         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               Vector2 blockPos = new Vector2(StartX + j + 0.5f, i + 0.5f);
               int perlinSurface = World.PerlinValueForX(blockPos.X);

               if (blockPos.Y < perlinSurface - 4.0f)
               {
                  blocks[i][j] = new StoneBlock(blockPos);
               }
               else if (blockPos.Y < perlinSurface)
               {
                  blocks[i][j] = new DirtBlock(blockPos);
               }
               else if ((int)blockPos.Y < World.waterLevel)
               {
                  blocks[i][j] = new WaterBlock(blockPos);
               }
               else
                  blocks[i][j] = new AirBlock(blockPos);
            }
         }
      }

      private void SpawnGrass()
      {
         for (int x = 0; x < blockCountX; x++)
         {
            int perlinValue = World.PerlinValueForX(StartX + x) - 1;

            if (blocks[perlinValue][x] is DirtBlock)
               SetBlock(new GrassBlock(blocks[perlinValue][x].pos));
         }
      }
      
      private Vector2 BlockPosFromPlace(int x, int y)
      {
         return new Vector2(StartX + x + 0.5f, y + 0.5f);
      }

      private void SpawnSand()
      {
         int w = World.sandLayerWidth;
         int h = World.sandLayerHeight;

         for (int cy = 0; cy < blockCountY; cy++)
         {
            for (int cx = 0; cx < blockCountX; cx++)
            {
               for (int ty = cy - h / 2; ty < cy + h / 2 + 1; ty++)
               {
                  for (int tx = cx - w / 2; tx < cx + w / 2 + 1; tx++)
                  {
                     int perlinValue = World.PerlinValueForX(StartX + tx);

                     if (perlinValue < ty && perlinValue < World.waterLevel)
                     {
                        Block block = GetBlock(cx, cy);
                        float d = Vector2.DistanceSquared(new Vector2(tx, ty), new Vector2(cx, cy));

                        if (!(block is null) && block is DirtBlock && d * d < w * w + h * h)
                           SetBlock(new SandBlock(new Vector2(StartX + cx + 0.5f, cy + 0.5f)));
                     }
                  }
               }
            }
         }
      }

      public void RestoreBlocks(int index)
      {
         Index = index;
         StartX = index * blockCountX;

         for (int y = 0; y < blockCountY; y++)
         {
            for (int x = 0; x < blockCountX; x++)
            {
               blocks[y][x].pos = BlockPosFromPlace(x, y);
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

      public bool DoContainBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         int block_x_id = (int)(pos.X) - Index * blockCountX + offsetX;
         int block_y_id = (int)Math.Min(blockCountY - 1, Math.Max(pos.Y, 0)) + offsetY;

         if (0 <= block_x_id && block_x_id < blockCountX &&
             0 <= block_y_id && block_y_id < blockCountY)
            return true;
         else
            return false;
      }

      public void SetBlock(Block block)
      {
         if (block.pos.X - StartX >= 0 && block.pos.X - StartX < blockCountX &&
             block.pos.Y >= 0 && block.pos.Y < blockCountY)
            blocks[(int)block.pos.Y][(int)(block.pos.X - StartX)] = block;
      }

      public Block GetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         return GetBlock(pos + new Vector2(offsetX, offsetY));
      }

      public Block GetBlock(Vector2 pos)
      {
         int x = (int)Math.Floor(pos.X) - Index * blockCountX;
         int y = (int)Math.Floor(pos.Y);

         return GetBlock(x, y);
      }

      public Block GetBlock(int x, int y)
      {
         if (x < 0 || x >= blockCountX ||
             y < 0 || y >= blockCountY)
            return null;

         return blocks[y][x];
      }

      public void DrawAllBlocks()
      {
         GL.Color4(Color4.White);

         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               //_blocks[i][j].DrawColor();
               blocks[i][j].DrawTexture();
            }
         }
      }
   }
}
