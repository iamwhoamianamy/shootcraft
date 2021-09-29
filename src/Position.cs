using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src
{
   public struct Position
   {
      public int X;
      public int Y;

      public Position(int X, int Y)
      {
         this.X = X;
         this.Y = Y;
      }
      public Position(float X, float Y)
      {
         this.X = (int)X;
         this.Y = (int)Y;
      }

      public static Position operator +(Position lhs, Position rhs) =>
         new Position(lhs.X + rhs.X, lhs.Y + rhs.Y);

      public static Position operator -(Position lhs, Position rhs) =>
         new Position(lhs.X - rhs.X, lhs.Y - rhs.Y);

      public static Position operator *(Position lhs, float scale) =>
         new Position(lhs.X * scale, lhs.Y  * scale);

      //public static bool operator <(Position lhs, Position rhs) =>
      //   lhs.X < rhs.X || lhs.Y < rhs.Y;

      //public static bool operator >(Position lhs, Position rhs) =>
      //   lhs.X > rhs.X || lhs.Y > rhs.Y;

      //public static bool operator ==(Position lhs, Position rhs) =>
      //   lhs.X == rhs.X && lhs.Y == rhs.Y;

      //public static bool operator !=(Position lhs, Position rhs) =>
      //   lhs.X != rhs.X && lhs.Y != rhs.Y;
   }
}
