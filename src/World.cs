using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using shootcraft.src.structures;

using Newtonsoft.Json;

namespace shootcraft.src
{
   public static class World
   {
      public static Random RNG { get; private set; }
      public static PerlinNoise perlinNoise;
      private static Dictionary<int, Chunk> chunks;
      private static Dictionary<int, Chunk> visibleChunks;
      public static List<Block> BlocksToUpdate { get; private set; }

      public const int sandLayerWidth = 8;
      public const int sandLayerHeight = 6;

      public const int waterLevel = 16;

      public const float gForce = 4.5f;

      public static Color4 DaySkyColor = new Color4(198, 250, 255, 255);

      public static Color4 SkyColor
      { 
         get
         {
            return DaySkyColor;
         }
      }

      public static void Init()
      {
         chunks = new Dictionary<int, Chunk>();
         visibleChunks = new Dictionary<int, Chunk>();
         perlinNoise = new PerlinNoise(10000, 10, 9, 0.00001);
         RNG = new Random();
         BlocksToUpdate = new List<Block>();
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

      public static int PerlinValueForX(float x)
      {
         return perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(x)];
      }
      
      public static void UpdateLighting()
      {
         foreach (var chunk in visibleChunks.Values)
         {
            chunk.ResetLighting();
         }

         foreach (var chunk in visibleChunks.Values)
         {
            chunk.UpdateLighting();
         }
      }

      public static Chunk TryGetChunk(Vector2 pos, int blockOffsetX = 0)
      {
         return TryGetChunk(PosToChunkId(pos + new Vector2(blockOffsetX, 0)));
      }

      public static Chunk TryGetChunk(int chunkId)
      {
         if (chunks.ContainsKey(chunkId))
         {
            return chunks[chunkId];
         }
         else
         {
            return null;
         }
      }

      public static Block TryGetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         Chunk chunk = TryGetChunk(pos, offsetX);

         if(chunk is null)
         {
            return null;
         }
         else
         {
            return GetBlock(pos, offsetX, offsetY);
         }
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

            if (perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] > 16 && perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)] % 2 == 0)
            {
               Tree.Insert(StructureHandler.trees[RNG.Next(0, 4)], new Vector2(chunk.StartX + 4.0f, perlinNoise.values[perlinNoise.Length / 2 + (int)Math.Floor(chunk.StartX + 4.0f)]));
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
      public static void SetBlockAndUpdateLight(Block block)
      {
         GetChunk(block.pos).SetBlockAndUpdateLight(block);
      }

      public static void SetVisibleChunks(Vector2 pos, int fow)
      {
         int center_chunk_id = PosToChunkId(pos);
         visibleChunks = new Dictionary<int, Chunk>();

         for (int i = center_chunk_id - fow; i <= center_chunk_id + fow; i++)
         {
            Chunk chunk = GetChunk(i);
            visibleChunks.Add(i, chunk);
         }
      }

      public static void DrawVisibleChunks()
      {
         foreach (var chunk in visibleChunks.Values)
         {
            chunk.DrawAllBlocks();
         }
      }

      public static void UpdateVisibleChunks()
      {
         SetBlocksToUpdate();
         UpdateBlocks();
      }
      
      private static void SetBlocksToUpdate()
      {
         BlocksToUpdate = new List<Block>();

         foreach (var chunk in visibleChunks.Values)
         {
            chunk.SetBlocksToUpdate();
         }
      }

      private static void UpdateBlocks()
      {
         foreach (var block in BlocksToUpdate)
         {
            SetBlock(block);
         }

         if (BlocksToUpdate.Count != 0)
            UpdateLighting();
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
         RNG = new Random();

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
