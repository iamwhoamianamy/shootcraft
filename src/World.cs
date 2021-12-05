using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK;
using shootcraft.src.structures;

using Newtonsoft.Json;

namespace shootcraft.src
{
   public static class World
   {
      private static Dictionary<int, Chunk> chunks;
      public static PerlinNoise perlinNoise;

      public const int sandLayerWidth = 8;
      public const int sandLayerHeight = 6;

      public const int waterLevel = 16;

      public const float gForce = 4.5f;

      public static void Init()
      {
         chunks = new Dictionary<int, Chunk>();
         perlinNoise = new PerlinNoise(10000, 10, 9, 0.00001);
      }

      public static void RestoreChunks()
      {
         foreach (KeyValuePair<int, Chunk> kvp in chunks)
         {
            kvp.Value.RestoreBlocks(kvp.Key);
         }
      }

      public static int PosToChunkId(Vector2 pos)
      {
         return (int)Math.Floor(pos.X / Chunk.blockCountX);
      }

      public static Chunk GetChunk(Vector2 pos, int blockOffsetX = 0)
      {
         return GetChunk(PosToChunkId(pos + new Vector2(blockOffsetX, 0)));
      }

      public static int PerlinValueForX(float x)
      {
         return perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(x)];
      }


      public static Chunk GetChunk(int chunkId)
      {
         if (!chunks.ContainsKey(chunkId))
         {
            Chunk chunk = new Chunk(chunkId);
            chunks.Add(chunkId, chunk);

            if (perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] > 16 && perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] % 2 == 0)
            {
               Tree tree = Tree.SmallTree(new Vector2(chunk.StartX + 4.0f, perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)]));

               for (int i = 0; i < tree.Height; i++)
               {
                  for (int j = 0; j < tree.Width; j++)
                  {
                     if (tree.blocks[i][j] != null)
                        SetBlock(tree.blocks[i][j]);
                  }
               }
            }
            Logger.Log($"Created chunk {chunkId}");

            return chunk;
         }
         else
            return chunks[chunkId];
      }

      public static Block GetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         return GetChunk(pos, offsetX).GetBlock(pos, offsetX, offsetY);
      }

      public static void SetBlock(Block block)
      {
         GetChunk(block.pos).SetBlock(block);
      }

      public static void DrawVisibleChunks(Vector2 pos, int fow)
      {
         int center_chunk_id = PosToChunkId(pos);

         for (int i = center_chunk_id - fow; i <= center_chunk_id + fow; i++)
         {
            Chunk chunk = GetChunk(i);
            chunk.DrawAllBlocks();
         }
      }

      public static void SaveToJson(string name)
      {
         string jsonString;
         var settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;

         jsonString = JsonConvert.SerializeObject(chunks, Formatting.Indented, settings);
         File.WriteAllText(SavesHandler.path + name + "/worlddata.json", jsonString);

         jsonString = JsonConvert.SerializeObject(perlinNoise, Formatting.Indented, settings);
         File.WriteAllText(SavesHandler.path + name + "/noisedata.json", jsonString);
      }

      public static void RestoreFromJson(string name)
      {
         string jsonString;
         var settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;

         jsonString = File.ReadAllText(SavesHandler.path + name + "/noisedata.json");
         perlinNoise = JsonConvert.DeserializeObject<PerlinNoise>(jsonString, settings);

         jsonString = File.ReadAllText(SavesHandler.path + name + "/worlddata.json");
         chunks = JsonConvert.DeserializeObject<Dictionary<int, Chunk>>(jsonString, settings);

         RestoreChunks();
      }
   }
}
