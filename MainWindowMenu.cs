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

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using shootcraft.src;
using shootcraft.src.blocks;

using Keyboard = OpenTK.Input.Keyboard;
using Key = OpenTK.Input.Key;

namespace shootcraft
{
   public partial class MainWindow : Window
   {
      public void RestartTimers()
      {
         drawingTimer.Start();
         worldUpdatingTimer.Start();
      }

      private void MenuItem_SaveWorld_Click(object sender, RoutedEventArgs e)
      {
         SavesHandler.DecompressJsons(SavesHandler.worldName);
         SavesHandler.SaveToJson(player, SavesHandler.worldName);
         World.SaveToJson(SavesHandler.worldName);
         SavesHandler.CompressJsons(SavesHandler.worldName);
         SavesHandler.AddLastPlayedInfo();
      }

      private void MenuItem_SaveWorldAs_Click(object sender, RoutedEventArgs e)
      {
         worldUpdatingTimer.Stop();
         drawingTimer.Stop();
         SavesWindow window = new SavesWindow();
         window.Show();
      }

      private void MenuItem_LoadWorldAs_Click(object sender, RoutedEventArgs e)
      {
         worldUpdatingTimer.Stop();
         drawingTimer.Stop();
         LoadWindow window = new LoadWindow();
         window.Show();
      }

      private void MenuItem_GenerateWorldAs_Click(object sender, RoutedEventArgs e)
      {
         SavesHandler.DecompressJsons(SavesHandler.worldName);
         SavesHandler.SaveToJson(player, SavesHandler.worldName);
         World.SaveToJson(SavesHandler.worldName);
         SavesHandler.CompressJsons(SavesHandler.worldName);

         World.Init();
         World.SetVisibleChunks(player.pos, player.fow);
         player = new Player(new Vector2(0, 40.0f));
         World.UpdateLighting();
      }

   }
}
