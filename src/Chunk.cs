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

      public const int blockCountX = 16;
      public const int blockCountY = 80;


      public Chunk(int index)
      {
         InitChunk(index);
         GenerateBasicTerrain();
         SpawnSand();
         SpawnGrass();
         UpdateLighting();
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

      public void SetBlockAndUpdateLight(Block block)
      {
         SetBlock(block);
         World.UpdateLighting();
         //World.UpdateLightingInBlockChunk(block);
      }

      public void SetBlock(Block block)
      {
         if (block.pos.X - StartX >= 0 && block.pos.X - StartX < blockCountX &&
             block.pos.Y >= 0 && block.pos.Y < blockCountY)
         {
            blocks[(int)block.pos.Y][(int)(block.pos.X - StartX)] = block;
            
         }
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
         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               //_blocks[i][j].DrawColor();
               blocks[i][j].Draw();
            }
         }
      }

      public void SetBlocksToUpdate()
      {
         for (int i = 0; i < blockCountY; i++)
         {
            for (int j = 0; j < blockCountX; j++)
            {
               blocks[i][j].Update();
            }
         }
      }

      public void ResetLighting()
      {
         foreach (var array in blocks)
         {
            foreach (var block in array)
            {
               block.LightLevel = 0;
            }
         }
      }

      public void UpdateLighting()
      {
         var lightSources = GetSunLightSources();
         DiffuseLight(lightSources);
      }

      private void DiffuseLight(List<Block> lightSources)
      {
         int diffuseRadius = Block.maxLightLevel;

         foreach (var source in lightSources)
         {
            for (int y = - diffuseRadius; y < diffuseRadius + 1; y++)
            {
               for (int x = - diffuseRadius; x < diffuseRadius + 1; x++)
               {
                  Block block = World.TryGetBlock(source.pos, x, y);

                  if(block is not null)
                  {
                     int dist = (int)Math.Round(Vector2.Distance(source.pos, block.pos));
                     int lightLevel = Block.maxLightLevel - dist;

                     if (block.LightLevel < lightLevel)
                        block.LightLevel = lightLevel;
                  }
               }
            }
         }
      }

      private List<Block> GetSunLightSources()
      {
         List<Block> sunLightSources = new List<Block>();

         for (int x = 0; x < blockCountX; x++)
         {
            for (int y = blockCountY - 1; y > 0; y--)
            {
               Vector2 pos = blocks[y][x].pos;

               //Block leftBlock = World.TryGetBlock(pos, -1, 0);
               //Block rightBlock = World.TryGetBlock(pos, +1, 0);

               //Block leftTopBlock = World.TryGetBlock(pos, -1, 1);
               //Block rightTopBlock = World.TryGetBlock(pos, +1, 1);

               //Block leftBotBlock = World.TryGetBlock(pos, -1, -1);
               //Block rightBotBlock = World.TryGetBlock(pos, +1, -1);

               Block botBlock = World.TryGetBlock(pos, 0, -1);

               //if (leftBlock is not null && leftBlock is AirBlock &&
               //   (leftTopBlock is not null && leftTopBlock is not AirBlock ||
               //    leftBotBlock is not null && leftBotBlock is not AirBlock))
               //{
               //   sunLightSources.Add(leftBlock);
               //   leftBlock.LightLevel = Block.maxLightLevel;
               //}

               if (botBlock is not AirBlock)
               {
                  sunLightSources.Add(blocks[y][x]);
                  blocks[y][x].LightLevel = Block.maxLightLevel;
                  break;
               }
            }
         }

         return sunLightSources;
      }
   }
}
