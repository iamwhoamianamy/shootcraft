using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using shootcraft.src.structures;
using shootcraft.src.blocks;

namespace shootcraft.src

{
   public static class StructureHandler
   {
      private static readonly string path = "../../resources/structs/";

      public static Dictionary<int, Type> treeBlocks = new Dictionary<int, Type>();
      public static List<string> treeNames = new List<string>();
      public static List<List<string>> treeTokens = new List<List<string>>();
      public static List<Tree> trees = new List<Tree>();

      //public static Dictionary<int, Type> houseBlocks = new Dictionary<int, Type>();
      //public static List<string> houseNamse = new List<string>();
      //public static List<List<string>> houseTokens = new List<List<string>>();
      //public static List<Houses> houses = new List<Houses>();


      static StructureHandler()
      {
         InitBlocks();
         InitNames();
         InitTokens();
         TreesFromTokens(treeTokens);
      }

      public static void InitBlocks()
      {
         InitTreeBlocks();
         //InitHouseBlocks();
      }

      public static void InitTreeBlocks()
      {
         treeBlocks[1] = typeof(WoodBlock);
         treeBlocks[2] = typeof(LeavesBlock);
      }

      //public static void InitHouseBlocks()
      //{
      //   houseBlocks[1] = typeof(WoodBlock);
      //   houseBlocks[2] = typeof(PlanksBlock);
      //}

      public static void InitNames()
      {
         NamesFromFile(treeNames, path + "Tree.txt");
         //FromFile(treesName, path + "House.txt");
      }

      public static List<string> NamesFromFile(List<string> list, string path)
      {
         using (StreamReader sr = new StreamReader(path))
         {
            while (!sr.EndOfStream)
            {
               list.Add(sr.ReadLine());
            }
         }
         return list;
      }
      public static void InitTokens()
      {
         TokensFromFile(treeTokens, treeNames);
         //TokensFromFile(houseTokens, houseNames);
      }

      public static List<List<string>> TokensFromFile(List<List<string>> tokens, List<string> names)
      {
         for (int i = 0; i < names.Count(); i++)
         {
            tokens.Add(new List<string>());
            using (StreamReader sr = new StreamReader(path + names[i]))
            {
               while (!sr.EndOfStream)
               {
                  tokens[i].Add(sr.ReadLine());
               }
            }
         }
         return tokens;
      }

      public static List<Tree> TreesFromTokens(List<List<string>> tokens)
      {
         for (int i = 0; i < treeNames.Count(); i++)
         {
            Tree tree = new Tree(tokens[i][0].Length, tokens[i].Count());

            for (int j = 0; j < tokens[i][0].Length; j++) //x
            {
               for (int l = 0; l < tokens[i].Count(); l++) //y
               {
                  int token = int.Parse(tokens[i][tokens[i].Count - 1 - l][j].ToString());

                  if (token != 0)
                  {
                     Type type = treeBlocks[token];
                     Vector2 pos = new Vector2(j + 0.5f, l + 0.5f);
                     tree.blocks[l][j] = (Block)Activator.CreateInstance(type, pos);
                  }
               }
            }

            trees.Add(tree);
         }
         return trees;
      }

   }
}
