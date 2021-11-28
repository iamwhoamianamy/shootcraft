using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shootcraft.src.blocks;

using OpenTK;

namespace shootcraft.src.structures
{
   class Tree : Structure
   {
      public Tree(int width, int height) : base(width, height) { }
      public static Tree SmallTree(Vector2 shift)
      {
         int width = 3;
         int height = 5;
         Tree smallTree = new Tree(width, height);

         for (int i = 0; i < 2; i++)
         {
            smallTree.blocks[i][1] = new WoodBlock(new Vector2(1.5f, i + 0.5f) + shift);
         }
         for (int i = 2; i < 5; i++)
         {
            smallTree.blocks[i][1] = new LeavesBlock(new Vector2(1.5f, i + 0.5f) + shift);
         }
         for (int i = 2; i < 4; i++)
         {
            smallTree.blocks[i][0] = new LeavesBlock(new Vector2(0.5f, i + 0.5f) + shift);
            smallTree.blocks[i][2] = new LeavesBlock(new Vector2(2.5f, i + 0.5f) + shift);
         }

         return smallTree;
      }
   }
}
