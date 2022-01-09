using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace shootcraft.src
{
   public static class GUI
   {
      public static Vector2 ScreenCenter { get; private set; }
      public static float ScreenWidth { get; private set; }
      public static float ScreenHeight { get; private set; }
      public static void SetDimensions(float screenWidth, float screenHeight)
      {
         ScreenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
         ScreenWidth = screenWidth;
         ScreenHeight = screenHeight;

         Inventory.DrawingRectangle = new Rectangle(ScreenCenter, 6 * screenWidth / 10, 3 * screenWidth / 10);
      }

      
   }
}
