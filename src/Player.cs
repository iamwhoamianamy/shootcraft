using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using shootcraft.src.blocks;

using Newtonsoft.Json;

namespace shootcraft.src
{
   [JsonObject(MemberSerialization.OptIn)]
   public class Player
   {
      [JsonProperty(ItemConverterType = typeof(Vector2Converter))]
      private Vector2 _pos;
      public Vector2 pos
      {
         get { return _pos; }
         set
         {
            _pos = value;
            BuildHull();
         }
      }

      [JsonProperty(ItemConverterType = typeof(Vector2Converter))]
      private Vector2 vel;
      [JsonProperty(ItemConverterType = typeof(Vector2Converter))]
      private Vector2 acc;

      public static float height = 1.75f;
      public static float width = 0.5f;
      public float speed = 0.6f;
      public float runningSpeed = 7.5f;
      public float jumpMomentum = 4.0f;
      public float accessRadius = 7.5f;

      public Cursor cursor;
      public Rectangle hull;
      private List<Block> surroundingBlocks;

      public bool IsStanding { get; private set; }
      public bool IsWaterLogged { get; private set; }

      [JsonProperty]
      public Color4 color;

      public Player(Vector2 pos)
      {
         this.pos = pos;
         this.vel = Vector2.Zero;
         this.acc = Vector2.Zero;

         cursor = new Cursor();

         IsStanding = false;
         IsWaterLogged = false;

         color = Color4.Blue;

         SetSurroundingBlocks();
      }

      public void BuildHull()
      {
         hull = new Rectangle(
         pos + new Vector2(-width / 2, height / 2),
         pos + new Vector2(width / 2, height / 2),
         pos + new Vector2(width / 2, -height / 2),
         pos + new Vector2(-width / 2, -height / 2));
      }

      public void UpdateLocation(float ellapsed)
      {
         vel += acc * ellapsed;
         pos += vel * ellapsed;
         acc = Vector2.Zero;
      }

      public void CheckForStanding()
      {
         List<Block> blocks = new List<Block>();

         blocks.Add(World.GetBlock(pos, -1, -1));
         blocks.Add(World.GetBlock(pos, 0, -1));
         blocks.Add(World.GetBlock(pos, 1, -1));

         foreach (var block in blocks)
         {
            float distanceY = Math.Abs(pos.Y - height / 4 - (block.pos.Y + 1.0f));
            float distanceX = Math.Abs(pos.X - block.pos.X);

            if (block.GetType() != typeof(AirBlock) && block.GetType() != typeof(WaterBlock) &&
                distanceY < 1e-1 && distanceX < 0.5f + width / 2)
            {
               IsStanding = true;
               return;
            }
            else
               IsStanding = false;
         }
      }

      public void CheckForWater()
      {
        SetSurroundingBlocks();

         foreach (var block in surroundingBlocks)
         {
            if ((block is WaterBlock) && hull.Intersect(block.GetRectangle()))
            {
               IsWaterLogged = true;
               color = Color4.Yellow;
               acc *= IsWaterLogged ? WaterBlock.viscosity : 1.0f;
               return;
            }
         }

         color = Color4.Blue;
         IsWaterLogged = false;
      }

      private void SetSurroundingBlocks()
      {
         surroundingBlocks = new List<Block>();

         for (int i = -1; i < 2; i++)
         {
            for (int j = -2; j < 4; j++)
            {
               surroundingBlocks.Add(World.GetBlock(hull.center, i, j));
            }
         }
      }

      #region trash

      //public void ResolveCollisionEfremov(World World, float ellapsed)
      //{
      //   Vector2 v = pos - new Vector2(0, height / 4);

      //   Block left_upper_block = World.GetBlock(v, -1, 1);
      //   Block right_upper_block = World.GetBlock(v, 1, 1);

      //   Block left_middle_block = World.GetBlock(v, -1, 0);
      //   Block right_middle_block = World.GetBlock(v, 1, 0);

      //   Block upper_block = World.GetBlock(v, 0, 2);

      //   if (left_middle_block.GetType() != typeof(AirBlock) ||
      //       left_upper_block.GetType() != typeof(AirBlock))
      //   {
      //      if (pos.X - width / 2 < left_middle_block.pos.X + Block.width / 2 ||
      //          pos.X - width / 2 < left_upper_block.pos.X + Block.width / 2)
      //      {
      //         pos = new Vector2(left_middle_block.pos.X + Block.width / 2 + width / 2, pos.Y);
      //         vel.X = 0;
      //         acc.X = 0;
      //      }
      //   }

      //   if (right_middle_block.GetType() != typeof(AirBlock) ||
      //       right_upper_block.GetType() != typeof(AirBlock))
      //   {
      //      if (pos.X + width / 2 > right_middle_block.pos.X - Block.width / 2 ||
      //          pos.X + width / 2 > right_upper_block.pos.X - Block.width / 2)
      //      {
      //         pos = new Vector2(right_middle_block.pos.X - width / 2 - Block.width / 2, pos.Y);
      //         vel.X = 0;
      //         acc.X = 0;
      //      }
      //   }

      //   Block center_lower_block = World.GetBlock(v, 0, -1);
      //   Block left_lower_block = World.GetBlock(v, -1, -1);
      //   Block right_lower_block = World.GetBlock(v, 1, -1);

      //   if (center_lower_block.GetType() != typeof(AirBlock) &&
      //       pos.Y - height / 4 < center_lower_block.pos.Y + Block.width || false)
      //      //left_lower_block.GetType() != typeof(AirBlock) &&
      //      //pos.Y - height / 4 < left_lower_block.pos.Y + Block.width &&
      //      //center_lower_block.GetType() == typeof(AirBlock) &&
      //      //pos.X - width / 2 < left_lower_block.pos.X + Block.width / 2 ||
      //      //right_lower_block.GetType() != typeof(AirBlock) &&
      //      //pos.Y - height / 4 < right_lower_block.pos.Y + Block.width / 2 &&
      //      //pos.X + width / 2 > right_lower_block.pos.X &&
      //      //center_lower_block.GetType() == typeof(AirBlock))
      //      IsStanding = true;
      //   else
      //      IsStanding = false;

      //   if (IsStanding)
      //   {
      //      pos = new Vector2(pos.X, center_lower_block.pos.Y + Block.width + height / 4);
      //      vel.Y = 0;
      //      acc.Y = 0;
      //   }


      //   if (upper_block.GetType() != typeof(AirBlock))
      //   {
      //      if (pos.Y + height / 4 * 3 > upper_block.pos.Y)
      //      {
      //         pos = new Vector2(pos.X, upper_block.pos.Y - height / 4 * 3);
      //         vel.Y = 0;
      //         acc.Y = 0;
      //      }
      //   }

      //}

      //public void ResolveCollisionSAT(World World, float ellapsed, int xAxe = 1, int yAxe = 1)
      //{
      //   int repeats = 1;
      //   float iteration_duration = ellapsed / repeats;

      //   for (int r = 0; r < repeats; r++)
      //   {
      //      List<Block> blocks = GetSurroundBlocks(World);

      //      for (int block_i = 0; block_i < blocks.Count; block_i++)
      //      {
      //         Rectangle rect = blocks[block_i].GetRectangle();

      //         for (int i = 0; i < 4; i++)
      //         {
      //            rect[i] -= rect.center;
      //            rect[i] *= 1.00f;
      //            rect[i] += rect.center;
      //         }

      //         if (blocks[block_i].GetType() == typeof(AirBlock))
      //            continue;

      //         ResolveCollision(hull, rect);
      //      }
      //   }
      //}

      //public void ResolveCollision(Rectangle r1, Rectangle r2, int xAxe = 1, int yAxe = 1)
      //{
      //   float overlap = float.MaxValue;

      //   Rectangle poly1 = r1;
      //   Rectangle poly2 = r2;

      //   for (int shape = 0; shape < 2; shape++)
      //   {
      //      if (shape == 1)
      //      {
      //         poly1 = r2;
      //         poly2 = r1;
      //      }

      //      for (int a = 0; a < 4; a++)
      //      {
      //         int b = (a + 1) % 4;
      //         Vector2 axisProj = new Vector2(
      //            -(poly1[b].Y - poly1[a].Y),
      //              poly1[b].X - poly1[a].X);

      //         // Optional normalisation of projection axis enhances stability slightly
      //         float deb = (float)Math.Sqrt(axisProj.X * axisProj.X + axisProj.Y * axisProj.Y);
      //         axisProj /= deb;

      //         // Work out min and max 1D points for r1
      //         float min_r1 = float.MaxValue, max_r1 = -float.MaxValue;
      //         for (int p = 0; p < 4; p++)
      //         {
      //            float q = poly1[p].X * axisProj.X + poly1[p].Y * axisProj.Y;
      //            min_r1 = Math.Min(min_r1, q);
      //            max_r1 = Math.Max(max_r1, q);
      //         }

      //         // Work out min and max 1D points for r2
      //         float min_r2 = float.MaxValue, max_r2 = -float.MaxValue;
      //         for (int p = 0; p < 4; p++)
      //         {
      //            float q = poly2[p].X * axisProj.X + poly2[p].Y * axisProj.Y;
      //            min_r2 = Math.Min(min_r2, q);
      //            max_r2 = Math.Max(max_r2, q);
      //         }

      //         // Calculate actual overlap along projected axis, and store the minimum
      //         overlap = Math.Min(Math.Min(max_r1, max_r2) - Math.Max(min_r1, min_r2), overlap);

      //         if (!(max_r2 >= min_r1 && max_r1 >= min_r2))
      //            return;
      //      }
      //   }

      //   // If we got here, the objects have collided, we will displace r1
      //   // by overlap along the vector between the two object centers
      //   Vector2 d = new Vector2(r2.center.X - r1.center.X, r2.center.Y - r1.center.Y);
      //   float s = (float)Math.Sqrt(d.X * d.X + d.Y * d.Y);
      //   Vector2 t = overlap * d / s;

      //   t.X *= xAxe;
      //   t.Y *= yAxe;

      //   //if (Math.Abs(r1.center.Y - r1.center.Y) - height / 2 <
      //   //    Math.Abs(r1.center.X - r1.center.X) - width / 2)
      //   //   t.Y = 0;
      //   //else
      //   //   t.X = 0;

      //   //t.X = 0;

      //   pos -= t;
      //}

      //public void ResolveCollisionDiagonal(World World, float ellapsed)
      //{
      //   int repeats = 1;
      //   float iteration_duration = ellapsed / repeats;

      //   for (int r = 0; r < repeats; r++)
      //   {
      //      List<Block> blocks = GetSurroundBlocks(World);
      //      bool is_any_collisions = false;

      //      for (int b = 0; b < blocks.Count; b++)
      //      {
      //         Rectangle rect = blocks[b].GetRectangle();

      //         for (int i = 0; i < 4; i++)
      //         {
      //            rect[i] -= rect.center;
      //            rect[i] *= 0.95f;
      //            rect[i] += rect.center;
      //         }

      //         if (blocks[b].GetType() == typeof(AirBlock))
      //            continue;

      //         Rectangle r1 = hull;
      //         Rectangle r2 = rect;

      //         for (int shape = 0; shape < 2; shape++)
      //         {
      //            if (shape == 1)
      //            {
      //               r1 = rect;
      //               r2 = hull;
      //            }

      //            // Check diagonals of this polygon...
      //            for (int p = 0; p < 4; p++)
      //            {
      //               Vector2 line_r1s = r1.center;
      //               Vector2 line_r1e = r1[p];

      //               Vector2 displacement = Vector2.Zero;

      //               // ...against edges of this polygon
      //               for (int q = 0; q < 4; q++)
      //               {
      //                  Vector2 line_r2s = r2[q];
      //                  Vector2 line_r2e = r2[q + 1];

      //                  // Standard "off the shelf" line segment intersection
      //                  float h = (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r1e.Y) - (line_r1s.X - line_r1e.X) * (line_r2e.Y - line_r2s.Y);
      //                  float t1 = ((line_r2s.Y - line_r2e.Y) * (line_r1s.X - line_r2s.X) + (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r2s.Y)) / h;
      //                  float t2 = ((line_r1s.Y - line_r1e.Y) * (line_r1s.X - line_r2s.X) + (line_r1e.X - line_r1s.X) * (line_r1s.Y - line_r2s.Y)) / h;

      //                  if (t1 >= 0.0f && t1 < 1.0f && t2 >= 0.0f && t2 < 1.0f)
      //                  {
      //                     displacement += (1.0f - t1) * (line_r1e - line_r1s);
      //                  }
      //               }

      //               if (displacement.LengthSquared > 1e-1)
      //               {
      //                  is_any_collisions = true;
      //                  pos += displacement * (shape == 0 ? -1 : +1);
      //               }
      //               //BuildHull();
      //            }
      //         }
      //      }

      //      if (is_any_collisions)
      //         color = Color4.Red;
      //      else
      //         color = Color4.Blue;

      //      //vel += acc * iteration_duration;
      //      //pos += vel * iteration_duration;
      //      //acc = Vector2.Zero;

      //      //BuildHull();
      //   }
      //}

      #endregion trash

      public void ResolveCollisionOnMoving(Vector2 shift)
      {
         int repeats = 60;
         Vector2 step = shift / repeats;

         for (int r = 0; r < repeats; r++)
         {
            Vector2 nextPos = hull.center + step;
            Rectangle newHull = new Rectangle(nextPos, width, height);

            SetSurroundingBlocks();

            foreach (var block in surroundingBlocks)
            {
               if (!(block is AirBlock) && !(block is WaterBlock) && newHull.Intersect(block.GetRectangle()))
                  return;
            }

            pos = nextPos;
         }
      }

      public void ResolveCollisionPrediction(float ellapsed)
      {
         int repeats = 100;
         float iter_duration = ellapsed / repeats;

         bool doIntersect = false;

         for (int r = 0; r < repeats; r++)
         {
            Vector2 nextVel = vel + acc * iter_duration;
            Vector2 nextPos = hull.center + vel * iter_duration;
            Rectangle newHull = new Rectangle(nextPos, width, height);

            SetSurroundingBlocks();

            foreach (var block in surroundingBlocks)
            {
               if (!(block is AirBlock) && !(block is WaterBlock) && newHull.Intersect(block.GetRectangle()))
               {
                  doIntersect = true;
                  break;
               }
            }

            if (doIntersect)
            {
               acc = Vector2.Zero;
               vel = Vector2.Zero;
               return;
            }
            else
            {
               vel = nextVel;
               pos = nextPos;
            }
         }
      }

      //public void ResolveCollisionPrediction(float ellapsed)
      //{
      //   int repeats = 30;
      //   float iter_duration = ellapsed / repeats;

      //   bool doIntersect = false;

      //   for (int r = 0; r < repeats; r++)
      //   {
      //      Vector2 nextPos = hull.center + vel * iter_duration + acc * iter_duration * iter_duration / 2;
      //      Rectangle newHull = new Rectangle(nextPos, width, height);

      //      SetSurroundingBlocks();

      //      foreach (var block in surroundingBlocks)
      //      {
      //         if (!(block is AirBlock) && !(block is WaterBlock) && newHull.Intersect(block.GetRectangle()))
      //         {
      //            doIntersect = true;
      //            break;
      //         }
      //      }

      //      if (doIntersect)
      //      {
      //         acc = Vector2.Zero;
      //         vel = Vector2.Zero;
      //         return;
      //      }
      //      else
      //      {
      //         pos = nextPos;
      //      }
      //   }
      //}

      public void ApplyForce(Vector2 force)
      {
         acc += force;
      }

      public void ApplyMomentum(Vector2 momentum)
      {
         vel += momentum;
      }

      public void Draw()
      {
         GL.Color4(color);

         GL.Begin(PrimitiveType.Quads);

         GL.Vertex2(hull.leftTop);
         GL.Vertex2(hull.rightTop);
         GL.Vertex2(hull.rightBot);
         GL.Vertex2(hull.leftBot);

         GL.End();
      }

      public void DrawCursor()
      {
         //if (Vector2.DistanceSquared(cursor.pos, pos) < 10000)
         cursor.Draw();
      }

      public Block GetBlockUnderCursor()
      {
         Vector2 cursPos = cursor.pos;
         cursPos -= pos;
         cursPos = cursPos.Limit(accessRadius);
         cursPos += pos;

         var cursorBlock = World.GetBlock(cursPos);

         return cursorBlock;
      }

      public void GoLeft()
      {
         ResolveCollisionOnMoving(new Vector2(-speed, 0.0f));
      }

      public void GoRight()
      {
         ResolveCollisionOnMoving(new Vector2(speed, 0.0f));
      }

      public void GoUp()
      {
         ResolveCollisionOnMoving(new Vector2(0.0f, speed));
      }

      public void GoDown()
      {
         ResolveCollisionOnMoving(new Vector2(0.0f, -speed));
      }

      public void Jump()
      {
         if (IsStanding)
            ApplyMomentum(new Vector2(0.0f, jumpMomentum));
      }
   }
}
