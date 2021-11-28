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
      public static NoiseGenerator noiseGenerator;

      public static void Init()
      {
         chunks = new Dictionary<int, Chunk>();
         noiseGenerator = new NoiseGenerator();
      }

      //public void AddChunk(Chunk chunk)
      //{
      //   if(!chunks.ContainsKey(chunk.BegPos))
      //      chunks.Add(chunk.BegPos, chunk);
      //}

      public static void RestoreChunks()
      {
         foreach (KeyValuePair<int, Chunk> kvp in chunks)
         {
            kvp.Value.RestoreBlocks(kvp.Key);
         }
      }

      public static int PosToChunkId(Vector2 pos)
      {
         return (int)Math.Floor(pos.X / (Chunk.blockCountX));
      }

      public static Chunk GetChunk(Vector2 pos, int blockOffsetX = 0)
      {
         return GetChunk(PosToChunkId(pos + new Vector2(blockOffsetX, 0)));
      }

      public static Chunk GetChunk(int chunkId)
      {
         if (!chunks.ContainsKey(chunkId))
         {
            Chunk chunk = new Chunk(chunkId);
            chunks.Add(chunkId, chunk);

            if (noiseGenerator.perlinNoise[noiseGenerator.length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] > 16 && noiseGenerator.perlinNoise[noiseGenerator.length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] % 2 == 0)
            {
               Tree tree = Tree.SmallTree(new Vector2(chunk.StartX + 4.0f, noiseGenerator.perlinNoise[noiseGenerator.length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)]));

               for (int i = 0; i < tree.Height; i++)
               {
                  for (int j = 0; j < tree.Width; j++)
                  {
                     if (tree.blocks[i][j] != null)
                        SetBlock(tree.blocks[i][j].pos, tree.blocks[i][j]);
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

      public static void SetBlock(Vector2 pos, Block block, int offsetX = 0, int offsetY = 0)
      {
         GetChunk(pos, offsetX).SetBlock(pos, block, offsetX, offsetY);
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
         jsonString = JsonConvert.SerializeObject(noiseGenerator, Formatting.Indented, settings);
         File.WriteAllText(SavesHandler.path + name + "/noisedata.json", jsonString);
      }

      public static void RestoreWorldFromJson(string name)
      {
         string jsonString;
         var settings = new JsonSerializerSettings();
         //settings.Converters.Add(new BlockConverter());
         settings.TypeNameHandling = TypeNameHandling.Objects;
         jsonString = File.ReadAllText(SavesHandler.path + name + "/noisedata.json");
         noiseGenerator = JsonConvert.DeserializeObject<NoiseGenerator>(jsonString, settings);
         jsonString = File.ReadAllText(SavesHandler.path + name + "/worlddata.json");
         chunks = JsonConvert.DeserializeObject<Dictionary<int, Chunk>>(jsonString, settings);
         RestoreChunks();
      }
   }
}
