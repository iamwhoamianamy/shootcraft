using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shootcraft.src.blocks;

using OpenTK;

namespace shootcraft.src.structures
{
   public class Tree : Structure
   {
      public Tree(int width, int height) : base(width, height) { }
      public static void Insert(Tree tree, Vector2 shift)
      {
         for (int i = 0; i < tree.Height; i++)
         {
            for (int j = 0; j < tree.Width; j++)
            {
               if (tree.blocks[i][j] != null)
               {
                  Block block = (Block)Activator.CreateInstance(tree.blocks[i][j].GetType(), tree.blocks[i][j].pos + shift);
                  World.SetBlock(block);
               }
            }
         }
      }

   }
}
