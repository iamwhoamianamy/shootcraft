using shootcraft.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace shootcraft
{
   /// <summary>
   /// Логика взаимодействия для LoadWindow.xaml
   /// </summary>
   public partial class LoadWindow : Window
   {
      List<Save> saves = new List<Save>();
      public LoadWindow()
      {
         InitializeComponent();
         ListSaves();
         ListViewSaves.ItemsSource = saves;
      }

      public void ListSaves()
      {
         DirectoryInfo dirInfo = new DirectoryInfo(SavesHandler.path);
         FileInfo[] fileEntries = dirInfo.GetFiles();
         foreach(FileInfo file in fileEntries)
         {
            Save save = new Save();
            save.Name = System.IO.Path.ChangeExtension(file.Name, null);
            save.Date = file.LastWriteTime;
            saves.Add(save);
         }
      }

      private void Load_Button_Click(object sender, RoutedEventArgs e)
      {
         var save = (Save)ListViewSaves.SelectedItem;
         SavesHandler.worldName = save.Name;
         SavesHandler.DecompressJsons(SavesHandler.worldName);
         World.RestoreFromJson(SavesHandler.worldName);
         ((MainWindow)Application.Current.MainWindow).SetPlayer(SavesHandler.RestorePlayerFromJson(SavesHandler.worldName));
         SavesHandler.CompressJsons(SavesHandler.worldName);
         ((MainWindow)Application.Current.MainWindow).RestartTimers();
         Close();
      }

      private void Cancel_Button_Click(object sender, RoutedEventArgs e)
      {
         Close();
         ((MainWindow)Application.Current.MainWindow).RestartTimers();
      }

      private void Window_Closed(object sender, EventArgs e)
      {
         ((MainWindow)Application.Current.MainWindow).RestartTimers();
      }
   }



   public class Save
   {
      public string Name { get; set; }
      public DateTime Date { get; set; }
   }
}
