﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace shootcraft.src
{
   public class Rectangle
   {
      public Vector2 leftTop;
      public Vector2 rightTop;
      public Vector2 rightBot;
      public Vector2 leftBot;

      public Vector2 center;

      public float Left => leftTop.X;
      public float Right => rightTop.X;
      public float Top => leftTop.Y;
      public float Bot => leftBot.Y;

      public Rectangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBot, Vector2 leftBot)
      {
         this.leftTop = leftTop;
         this.rightTop = rightTop;
         this.rightBot = rightBot;
         this.leftBot = leftBot;

         center = (leftTop + rightTop + rightBot + leftBot) / 4;
      }

      public Rectangle(Vector2 center, float width, float height)
      {
         leftTop = center + new Vector2(-width / 2, height / 2);
         rightTop = center + new Vector2(width / 2, height / 2);
         leftBot = center + new Vector2(-width / 2, -height / 2);
         rightBot = center + new Vector2(width / 2, -height / 2);

         this.center = center;
      }

      public Vector2 this[int i]
      {
         get
         {
            switch (i % 4)
            {
               case 0: return leftTop;
               case 1: return rightTop;
               case 2: return rightBot;
               case 3: return leftBot;
               default: return Vector2.Zero;
            }
         }
         set
         {
            switch (i % 4)
            {
               case 0: leftTop = value; break;
               case 1: rightTop = value; break;
               case 2: rightBot = value; break;
               case 3: leftBot = value; break;
            }
         }
      }

      public Rectangle(Vector2 center, float width) : this(center, width, width) { }

      public bool Contain(Vector2 point)
      {
         return Left < point.X && point.X < Right &&
                Bot < point.Y && point.Y < Top;
      }
      public bool Intersect(Rectangle rect)
      {
         return Contain(rect.leftTop) || Contain(rect.rightTop) ||
                Contain(rect.leftBot) || Contain(rect.rightBot) ||
                rect.Contain(leftTop) || rect.Contain(rightTop) ||
                rect.Contain(leftBot) || rect.Contain(rightBot);
      }
   }
}