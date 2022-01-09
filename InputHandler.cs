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
         int x = e.X;
         int y = glControl.Height - e.Y;

         CalcPlayersCursorPosition(x, y);
         CalcInventoryActiveCellId(x, y);
      }

      private void CalcInventoryActiveCellId(int x, int y)
      {
         player.MyInventory.ActiveCell = Inventory.CellIdByPos(x, y);
      }

      private void CalcPlayersCursorPosition(int x, int y)
      {
         float newX = x;
         newX /= globalScaling;

         newX -= screenCenterGame.X;
         newX /= scale;
         newX += screenCenterGame.X;

         newX -= translation.X;
         player.cursor.pos.X = newX;

         float newY = y;
         newY /= globalScaling;

         newY -= screenCenterGame.Y;
         newY /= scale;
         newY += screenCenterGame.Y;

         newY -= translation.Y;
         player.cursor.pos.Y = newY;
      }

      private void glControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         if(player.IsEnventoryOpened)
         {
            MouseClickInventoryOpened(e);
         }   
         else
         {
            MouseClickInventoryClosed(e);
         }
      }

      private void MouseClickInventoryClosed(System.Windows.Forms.MouseEventArgs e)
      {
         Block block = player.GetBlockUnderCursor();

         if (!(block is null))
         {
            switch (e.Button)
            {
               case System.Windows.Forms.MouseButtons.Left:
               {
                  World.SetBlockAndUpdateLight(new AirBlock(block.pos));

                  break;
               }
               case System.Windows.Forms.MouseButtons.Right:
               {
                  player.ApplyHoldingItem();
                  break;
               }
            }
         }
      }

      private void MouseClickInventoryOpened(System.Windows.Forms.MouseEventArgs e)
      {
         if(player.MyInventory.ActiveCell is not null)
         {
            player.HoldingItem = player.MyInventory.Cells[player.MyInventory.ActiveCell.Value].Item;
         }
      }

      private void glControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
      {
         const float scalingFactor = 1.1f;
         scale *= e.Delta > 0 ? scalingFactor : 1 / scalingFactor;
      }

      private void ControllPlayer()
      {
         if (Keyboard.GetState().IsKeyDown(Key.LShift))
            player.IsRunning = true;
         else
            player.IsRunning = false;

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

         switch (e.KeyCode)
         {
            case System.Windows.Forms.Keys.E:
            {
               player.IsEnventoryOpened = !player.IsEnventoryOpened;
               break;
            }
         }
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