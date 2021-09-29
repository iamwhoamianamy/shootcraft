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

      private Vector2 mousePos;
      private bool isMouseOver = false;
      private bool isMouseDown = false;

      private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         mousePos.X = e.X;
         mousePos.Y = glControl.Height - e.Y;

         Title = e.X.ToString() + " " + e.Y.ToString();

      }

      private void glControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         Chunk cursor_chunk = chunkHandler.GetChunk(mousePos);
         cursor_chunk.DrawBorders();
         Block cursor_block = cursor_chunk.GetBlock(mousePos);
         cursor_chunk.SetBlock(mousePos, new AirBlock(cursor_block.pos));
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
         }
         else
            if (Keyboard.GetState().IsKeyDown(Key.D))
         {
            player.GoRight();
         }
      }

      private void glControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
      {

      }
   }
}