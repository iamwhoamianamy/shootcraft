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
         player.cursor.pos.X = e.X - translation.X;
         player.cursor.pos.Y = glControl.Height - e.Y - translation.Y;

      }

      private void glControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         Vector2 block_pos = player.cursor.pos;

         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            Block cursor_block = World.GetBlock(block_pos);
            World.SetBlock(block_pos, new AirBlock(cursor_block.pos));
         }
         else
         {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
               Block cursor_block = World.GetBlock(block_pos);
               World.SetBlock(block_pos, new DirtBlock(cursor_block.pos));
            }
         }
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