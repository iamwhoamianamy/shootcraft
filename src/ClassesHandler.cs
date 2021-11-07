using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace shootcraft.src
{
   public class ClassesHandler
   {
      private static List<Type> blocks;

      public static List<Type> Blocks => blocks;

      static ClassesHandler()
      {
         blocks = new List<Type>();

         var assembly = typeof(ClassesHandler).GetTypeInfo().Assembly;

         foreach (var t in assembly.GetTypes())
         {
            if (t.GetInterfaces().Contains(typeof(IBlockRegisterable)))
            {
               blocks.Add((Activator.CreateInstance(t) as Block).GetType());
            }
         }
      }

      public static void Init() { }

   }
}
