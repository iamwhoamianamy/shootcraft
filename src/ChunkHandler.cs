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
      private NoiseGenerator noiseGenerator;

      public ChunkHandler()
      {
         chunks = new Dictionary<int, Chunk>();
         noiseGenerator = new NoiseGenerator();
      }

      //public void AddChunk(Chunk chunk)
      //{
      //   if(!chunks.ContainsKey(chunk.BegPos))
      //      chunks.Add(chunk.BegPos, chunk);
      //}

      public int PosToChunkId(Vector2 pos)
      {
         return (int)Math.Floor(pos.X / (Chunk.blockCountX * Block.width));
      }

      public Chunk GetChunk(Vector2 pos, int blockOffsetX = 0)
      {
         return GetChunk(PosToChunkId(pos + new Vector2(Block.width * blockOffsetX, 0)));
      }

      public Chunk GetChunk(int chunkId)
      {
         if (!chunks.ContainsKey(chunkId))
         {
            Chunk chunk = new Chunk(chunkId, noiseGenerator.fPerlinNoise1D);
            chunks.Add(chunkId, chunk);

            Logger.Log($"Created chunk {chunkId}");

            return chunk;
         }
         else
            return chunks[chunkId];
      }

      public Block GetBlock(Vector2 pos, int offsetX = 0, int offsetY = 0)
      {
         return GetChunk(pos, offsetX).GetBlock(pos, offsetX, offsetY);
      }

      public void SetBlock(Vector2 pos, Block block, int offsetX = 0, int offsetY = 0)
      {
         GetChunk(pos, offsetX).SetBlock(pos, block, offsetX, offsetY);
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
