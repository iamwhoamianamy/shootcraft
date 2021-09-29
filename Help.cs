using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameOfParticles
{
   public static class Help
   {
      public static Random random;

      public static float W { get; set; }
      public static float H { get; set; }

      static Help()
      {
         random = new Random();
      }

      public static void SetDim(float width, float height)
      {
         W = width;
         H = height;
      }

      public static float AngleBetween(Vector2 vec1, Vector2 vec2)
      {
         //int sign = Math.Sign(Vector2.PerpDot(vec1, vec2)) * -1;
         //double dInDeg = MathHelper.RadiansToDegrees(d);
         return (float)Math.Acos(Vector2.Dot(vec1, vec2) / (vec1.Length * vec2.Length));
      }

      public static void DrawRect(float x, float y, float w, float h)
      {
         GL.Begin(BeginMode.LineLoop);

         GL.Vertex2(x - w / 2, y - h / 2);
         GL.Vertex2(x + w / 2, y - h / 2);
         GL.Vertex2(x + w / 2, y + h / 2);
         GL.Vertex2(x - w / 2, y + h / 2);

         GL.End();
      }

      public static void DrawRect(Vector2 loc, Vector2 dim)
      {
         DrawRect(loc.X, loc.Y, dim.X, dim.Y);
      }

      public static float RayLineIntersect(Vector2 a, Vector2 b, Vector2 o, Vector2 d)
      {
         Vector2 v1 = o - a;
         Vector2 v2 = b - a;
         Vector2 v3 = new Vector2(-d.Y, d.X);

         float dot = Vector2.Dot(v2, v3);

         if (Math.Abs(dot) < 0.000001)
            return -1f;
         else
         {
            float t1 = Vector2.PerpDot(v2, v1) / dot;
            float t2 = Vector2.Dot(v1, v3) / dot;

            if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
               return t1;
         }

         return -1f;
      }

      public static Vector2 VecFromAng(double angDeg)
      {
         //return RotateVector(new Vector2(1, 0), MathHelper.DegreesToRadians(angDeg)).Normalized();

         Vector2 res = new Vector2(1, 0);

         res.X = (float)Math.Cos(MathHelper.DegreesToRadians(angDeg));
         res.Y = (float)Math.Sin(MathHelper.DegreesToRadians(angDeg));

         return res;
      }

      public static Vector2 RotateVector(Vector2 vec, double angRad)
      {
         Vector2 res = new Vector2(vec.X, vec.Y);
         res.X = res.X * (float)Math.Cos(angRad) - res.Y * (float)Math.Sin(angRad);
         res.Y = res.X * (float)Math.Sin(angRad) + res.Y * (float)Math.Cos(angRad);

         return res;
      }

      public static Vector2 Limit(this Vector2 vec, float length)
      {
         if(vec.LengthSquared >= length * length)
         {
            vec.Normalize();
            vec *= length;
         }

         return vec;
      }

      public static float RandInRange(float left, float right)
      {
         return (float)(Help.random.NextDouble() * (right - left) + left);
      }

      public static Color4 CreateRandomColor()
      {
         return new Color4
         {
            R = (float)Help.random.NextDouble(),
            G = (float)Help.random.NextDouble(),
            B = (float)Help.random.NextDouble(),
            A = (float)Help.random.NextDouble()
         };
      }

      public static float InverseSqrt(float x)
      {
         float xhalf = 0.5f * x;
         int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
         i = 0x5f3759df - (i >> 1);
         x = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
         x = x * (1.5f - xhalf * x * x);
         return x;
      }
   }
}
