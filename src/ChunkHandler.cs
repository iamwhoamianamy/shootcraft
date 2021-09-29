using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src
{
   public class ChunkHandler
   {
      private Dictionary<int, Chunk> chunks;

      public ChunkHandler()
      {
         chunks = new Dictionary<int, Chunk>();
      }

      //public void AddChunk(Chunk chunk)
      //{
      //   if(!chunks.ContainsKey(chunk.BegPos))
      //      chunks.Add(chunk.BegPos, chunk);
      //}

      public int PosToChunkId(Vector2 pos)
      {
         return (int)(pos.X / (Chunk.blockCountX * Block.width));
      }

      public Chunk GetChunk(Vector2 pos)
      {
         return GetChunk(PosToChunkId(pos));
      }

      public Chunk GetChunk(int chunkId)
      {
         if (!chunks.ContainsKey(chunkId))
         {
            Chunk chunk = new Chunk(chunkId);
            chunks.Add(chunkId, chunk);

            Logger.Log($"Created chunk {chunkId}");

            return chunk;
         }
         else
            return chunks[chunkId];
      }

      public void DrawVisibleChunks(Vector2 pos, int fow)
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
