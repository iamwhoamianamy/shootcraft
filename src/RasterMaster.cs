using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src
{
   public static class RasterMaster
   {
      public const int circleRadius = 8;
      public static List<Vector2> circle;

      static RasterMaster()
      {
         circle = new List<Vector2>();
         FormSircle();
      }

      public static List<Vector2> GetShiftedCircle(Vector2 shift)
      {
         List<Vector2> circleShifted = new List<Vector2>();

         for (int i = 0; i < circle.Count; i++)
         {
            circleShifted.Add(circle[i] + shift);
         }

         return circleShifted;
      }

      private static void FormSircle()
      {
         int steps = 60;
         float stepAngle = DegToRad(360 / steps);

         for (int i = 0; i < steps; i++)
         {
            float x = circleRadius * (float)Math.Cos(stepAngle * i);
            float y = circleRadius * (float)Math.Sin(stepAngle * i);

            circle.Add(new Vector2(x, y));
         }
      }

      public static float DegToRad(float deg)
      {
         return (float)(deg * Math.PI / 180);
      }

      public static List<(Block, float)> RasterLine(Vector2 from, Vector2 to)
      {
         List<(Block, float)> blocks = new List<(Block, float)>();

         Vector2 rayDir = (to - from).Normalized();

         Vector2 rayUnitStepSize = new Vector2((float)Math.Sqrt(1 + (rayDir.Y / rayDir.X) * (rayDir.Y / rayDir.X)),
            (float)Math.Sqrt(1 + (rayDir.X / rayDir.Y) * (rayDir.X / rayDir.Y)));

         Vector2 mapCheck = from;
         Vector2 rayLength1D;
         Vector2 step;

         // Establish Starting Conditions
         if (rayDir.X < 0)
         {
            step.X = -1;
            rayLength1D.X = (from.X - mapCheck.X) * rayUnitStepSize.X;
         }
         else
         {
            step.X = 1;
            rayLength1D.X = (mapCheck.X + 1 - from.X) * rayUnitStepSize.X;
         }

         if (rayDir.Y < 0)
         {
            step.Y = -1;
            rayLength1D.Y = (from.Y - mapCheck.Y) * rayUnitStepSize.Y;
         }
         else
         {
            step.Y = 1;
            rayLength1D.Y = (mapCheck.Y + 1 - from.Y) * rayUnitStepSize.Y;
         }

         // Perform "Walk" until collision or range check
         float maxDistance = Vector2.DistanceSquared(from, to);
         float distance = 0.0f;

         while (distance * distance < maxDistance)
         {
            // Walk along shortest path
            if (rayLength1D.X < rayLength1D.Y)
            {
               mapCheck.X += step.X;
               distance = rayLength1D.X;
               rayLength1D.X += rayUnitStepSize.X;
            }
            else
            {
               mapCheck.Y += step.Y;
               distance = rayLength1D.Y;
               rayLength1D.Y += rayUnitStepSize.Y;
            }

            blocks.Add((World.TryGetBlock(mapCheck), distance));
         }

         return blocks;
      }
   }
}
