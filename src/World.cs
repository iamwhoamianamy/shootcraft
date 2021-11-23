using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src
{
   public static class World
   {
      private static Dictionary<int, Chunk> chunks;
      private static NoiseGenerator noiseGenerator;

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
            Chunk chunk = new Chunk(chunkId, noiseGenerator);
            chunks.Add(chunkId, chunk);

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
   }
}
