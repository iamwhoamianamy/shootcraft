using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

using OpenTK.Graphics;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using shootcraft.src;
using shootcraft.src.blocks;

using Keyboard = OpenTK.Input.Keyboard;
using Key = OpenTK.Input.Key;

namespace shootcraft
{
   public partial class MainWindow : Window
   {

      private bool isMouseOver = false;
      private bool isMouseDown = false;

      private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         float x = e.X;

         x -= screenCenter.X;
         x /= scale;
         x += screenCenter.X;

         x -= translation.X;

         player.cursor.pos.X = x;

         float y = glControl.Height;
         y -= e.Y;

         y -= screenCenter.Y;
         y /= scale;
         y += screenCenter.Y;

         y -= translation.Y;
         player.cursor.pos.Y = y;
      }

      private void glControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         //Vector2 block_pos = player.cursor.pos;
         Vector2 cursor = player.GetBlockUnderCursor().pos;

         switch(e.Button)
         {
            case System.Windows.Forms.MouseButtons.Left:
            {
               World.SetBlock(cursor, new AirBlock(cursor));

               break;
            }
            case System.Windows.Forms.MouseButtons.Right:
            {
               World.SetBlock(cursor, new DirtBlock(cursor));

               break;
            }
         }
      }
      private void glControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         const float scalingFactor = 1.1f;
         scale *= e.Delta > 0 ? scalingFactor : 1 / scalingFactor;
      }

      private void ControllPlayer()
      {
         if (Keyboard.GetState().IsKeyDown(Key.Space))
         {
            player.Jump();
         }

         if (Keyboard.GetState().IsKeyDown(Key.A))
         {
            player.GoLeft();
            //player.ResolveCollisionSAT(World, ellapsed);
         }
         else
            if (Keyboard.GetState().IsKeyDown(Key.D))
         {
            player.GoRight();
            //player.ResolveCollisionSAT(World, ellapsed);
         }

         if (Keyboard.GetState().IsKeyDown(Key.S))
         {
            player.GoDown();
            //player.ResolveCollisionSAT(World, ellapsed);
         }
         else
            if (Keyboard.GetState().IsKeyDown(Key.W))
         {
            player.GoUp();
            //player.ResolveCollisionSAT(World, ellapsed);
         }
      }

      private void glControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
      {

         //switch(e.KeyCode)
         //{
         //   case System.Windows.Forms.Keys.W:
         //   {
         //      player.GoUp();
         //      break;
         //   }
         //   case System.Windows.Forms.Keys.A:
         //   {
         //      player.GoLeft();
         //      break;
         //   }
         //   case System.Windows.Forms.Keys.S:
         //   {
         //      player.GoDown();
         //      break;
         //   }
         //   case System.Windows.Forms.Keys.D:
         //   {
         //      player.GoRight();
         //      break;
         //   }
         //}
      }

      public Vector2 ScreenToWorld(Vector2 coords)
      {
         return coords - translation;
      }

      private void glControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
      {

      }



   }
}