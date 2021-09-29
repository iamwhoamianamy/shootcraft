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

      public static float height = 40.0f;
      public static float width = 10.0f;

      private float speed = 4.0f;
      private float jumpMomentum = 100.0f;

      public bool IsStanding { get; private set; }

      public Player(Vector2 pos)
      {
         this.pos = pos;
         this.vel = Vector2.Zero;
         this.acc = Vector2.Zero;

         IsStanding = false;
      }

      public void UpdatePosition(ChunkHandler chunkHandler, float ellapsed)
      {
         Block center_lower_block = chunkHandler.GetBlock(pos, 0, -1);
         Block left_lower_block = chunkHandler.GetBlock(pos, -1, -1);
         Block right_lower_block = chunkHandler.GetBlock(pos, 1, -1);

         if (center_lower_block.GetType() != typeof(AirBlock) &&
             pos.Y - height / 4 < center_lower_block.pos.Y + Block.width || false
             /*left_lower_block.GetType() != typeof(AirBlock) &&
             pos.X - width / 3 < left_lower_block.pos.X + Block.width*/)
            IsStanding = true;
         else
            IsStanding = false;

         if (IsStanding)
         {
            pos.Y = center_lower_block.pos.Y + Block.width + height / 4;
            vel.Y = 0;
            acc.Y = 0;
         }
         Block left_upper_block = chunkHandler.GetBlock(pos, -1, 1);
         Block right_upper_block = chunkHandler.GetBlock(pos, 1, 1);

         Block left_middle_block = chunkHandler.GetBlock(pos, -1, 0);
         Block right_middle_block = chunkHandler.GetBlock(pos, 1, 0);

         Block upper_block = chunkHandler.GetBlock(pos, 0, 2);

         if (left_middle_block.GetType() != typeof(AirBlock) ||
             left_upper_block.GetType() != typeof(AirBlock))
         {
            if (pos.X - width / 2 < left_middle_block.pos.X + Block.width ||
                pos.X - width / 2 < left_upper_block.pos.X + Block.width)
            {
               pos.X = left_middle_block.pos.X + Block.width + width / 2;
               vel.X = 0;
               acc.X = 0;
            }
         }

         if (right_middle_block.GetType() != typeof(AirBlock) ||
             right_upper_block.GetType() != typeof(AirBlock))
         {
            if (pos.X + width / 2 > right_middle_block.pos.X ||
                pos.X + width / 2 > right_upper_block.pos.X)
            {
               pos.X = right_middle_block.pos.X - width / 2;
               vel.X = 0;
               acc.X = 0;
            }
         }

         if (upper_block.GetType() != typeof(AirBlock))
         {
            if (pos.Y + height / 4 * 3 > upper_block.pos.Y)
            {
               pos.Y = upper_block.pos.Y - height / 4 * 3;
               vel.Y = 0;
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

      public void GoLeft()
      {
         pos += new Vector2(-speed, 0.0f);
      }

      public void GoRight()
      {
         pos += new Vector2(speed, 0.0f);
      }

      public void Jump()
      {
         if (IsStanding)
            ApplyMomentum(new Vector2(0.0f, jumpMomentum));
      }
   }
}
