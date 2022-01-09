using shootcraft.src;
using System;
using System.Collections.Generic;
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
   /// Логика взаимодействия для SavesWindow.xaml
   /// </summary>
   public partial class SavesWindow : Window
   {
      public SavesWindow()
      {
         InitializeComponent();
         Loaded += (sender, e) =>
    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
      }

      private void Save_Button_Click(object sender, RoutedEventArgs e)
      {
         if (name.Text.Length <= 0)
            SavesHandler.worldName = "default";
         else
            SavesHandler.worldName = name.Text;
         SavesHandler.DecompressJsons(SavesHandler.worldName);
         SavesHandler.SaveToJson(((MainWindow)Application.Current.MainWindow).GetPlayer(), SavesHandler.worldName);
         World.SaveToJson(SavesHandler.worldName);
         SavesHandler.CompressJsons(SavesHandler.worldName);
         SavesHandler.AddLastPlayedInfo();
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
}
