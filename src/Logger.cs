using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace shootcraft.src
{
   public class Logger
   {
      private static Logger instance;
      private StreamWriter sw;

      private Logger()
      {
         sw = new StreamWriter("log.txt");
      }

      public static Logger Get()
      {
         if (instance == null)
            instance = new Logger();

         return instance;
      }

      public static void Log(string text)
      {
         Get().sw.WriteLine($"<{DateTime.Now}> - {text}");
      }

      public static void Close()
      {
         Get().sw.Close();
      }
   }
}
