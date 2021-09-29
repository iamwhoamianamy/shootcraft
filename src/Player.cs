using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using shootcraft.src.blocks;

namespace shootcraft.src
{
   public class Player
   {
      public Vector2 pos;
      private Vector2 vel;
      private Vector2 acc;

      public static float height = 20.0f;
      public static float width = 5.0f;

      public bool IsStanding { get; private set; }

      public Player(Vector2 pos)
      {
         this.pos = pos;
         this.vel = Vector2.Zero;
         this.acc = Vector2.Zero;

         IsStanding = false;
      }

      public void UpdatePosition(ChunkHandler ch, float ellapsed)
      {
         Chunk current_chunk = ch.GetChunk(pos);
         Block lower_block = current_chunk.GetBlock(pos, 0, -1);

         if (lower_block.GetType() == typeof(AirBlock))
            IsStanding = false;
         else
            IsStanding = true;

         if (IsStanding)
         {
            if (pos.Y - height / 4 < lower_block.pos.Y + Block.width)
            {
               //if(current_block.GetType() == typeof(AirBlock))
                  pos.Y = lower_block.pos.Y + Block.width + height / 4;

               if (vel.Y < 0)
                  vel.Y = 0;

               if (acc.Y < 0)
                  acc.Y = 0;
            }
         }

         Chunk left_chunk, right_chunk;

         if (current_chunk.ContainsBlock(pos, -1, 0))
            left_chunk = current_chunk;
         else
            left_chunk = ch.GetChunk(current_chunk.Index - 1);

         if (current_chunk.ContainsBlock(pos, 1, 0))
            right_chunk = current_chunk;
         else
            right_chunk = ch.GetChunk(current_chunk.Index + 1);

         Block left_lower_block = left_chunk.GetBlock(pos, -1, 0);
         Block right_lower_block = right_chunk.GetBlock(pos, 1, 0);

         Block left_upper_block = left_chunk.GetBlock(pos, -1, 1);
         Block right_upper_block = right_chunk.GetBlock(pos, 1, 1);

         Block upper_block = current_chunk.GetBlock(pos, 0, 2);

         bool is_left_blocked;
         bool is_right_blocked;
         bool is_upper_blocked;

         if (left_lower_block.GetType() == typeof(AirBlock) &&
             left_upper_block.GetType() == typeof(AirBlock))
            is_left_blocked = false;
         else
            is_left_blocked = true;

         if (right_lower_block.GetType() == typeof(AirBlock) &&
             right_upper_block.GetType() == typeof(AirBlock))
            is_right_blocked = false;
         else
            is_right_blocked = true;

         if (upper_block.GetType() == typeof(AirBlock))
            is_upper_blocked = false;
         else
            is_upper_blocked = true;

         if (is_left_blocked)
         {
            if (pos.X - width / 2 < left_lower_block.pos.X + Block.width ||
                pos.X - width / 2 < left_upper_block.pos.X + Block.width)
            {
               pos.X = left_lower_block.pos.X + Block.width + width / 2;

               if (vel.X < 0)
                  vel.X = 0;

               if (acc.X < 0)
                  acc.X = 0;
            }
         }

         if (is_right_blocked)
         {
            if (pos.X + width / 2 > right_lower_block.pos.X ||
                pos.X + width / 2 > right_upper_block.pos.X)
            {
               pos.X = right_lower_block.pos.X - width / 2;

               if (vel.X > 0)
                  vel.X = 0;

               if (acc.X > 0)
                  acc.X = 0;
            }
         }

         if (is_upper_blocked)
         {
            if (pos.Y + height / 4 * 3 > upper_block.pos.Y)
            {
               pos.Y = upper_block.pos.Y - height / 4 * 3;

               if (vel.Y > 0)
                  vel.Y = 0;

               if (acc.Y > 0)
                  acc.Y = 0;
            }
         }

         vel += acc * ellapsed;
         pos += vel * ellapsed;
         acc = Vector2.Zero;
      }

      public void ApplyForce(Vector2 force)
      {
         acc += force;
      }

      public void ApplyMomentum(Vector2 force)
      {
         vel += force;
      }
      public void Draw()
      {
         GL.Color4(Color4.Red);

         GL.Begin(PrimitiveType.Quads);

         GL.Vertex2(pos.X - width / 2, pos.Y - height / 4);
         GL.Vertex2(pos.X + width / 2, pos.Y - height / 4);
         GL.Vertex2(pos.X + width / 2, pos.Y + height / 4 * 3);
         GL.Vertex2(pos.X - width / 2, pos.Y + height / 4 * 3);

         GL.End();
      }
   }
}
