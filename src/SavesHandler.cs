using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace shootcraft.src
{
   static class SavesHandler
   {
      public static string path = "../../saves/";
      public static string worldName = "autosave";

      public static void SaveToJson(Player player, string name)
      {
         Directory.CreateDirectory(path);
         Directory.CreateDirectory(path + name);
         string jsonString;
         var settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;
         settings.Converters.Add(new Vector2Converter());
         jsonString = JsonConvert.SerializeObject(player, Formatting.Indented, settings);
         File.WriteAllText(path + name + "/playerdata.json", jsonString);
      }



      public static Player RestorePlayerFromJson(string name)
      {
         string jsonString;
         jsonString = File.ReadAllText(path + name + "/playerdata.json");
         var settings = new JsonSerializerSettings();
         settings.Converters.Add(new Vector2Converter());
         settings.TypeNameHandling = TypeNameHandling.Objects;
         Player player = JsonConvert.DeserializeObject<Player>(jsonString, settings);
         player.BuildHull();
         return player;
      }

      static public void CompressJsons(string name)
      {
         ZipFile.CreateFromDirectory(path + name, path + name + ".zip");
         Directory.Delete(path + name, true);
      }

      static public void DecompressJsons(string name)
      {
         if(File.Exists(path + name + ".zip"))
         {
            ZipFile.ExtractToDirectory(path + name + ".zip", path + name);
            File.Delete(path + name + ".zip");
         }
      }

      static public void AddLastPlayedInfo()
      {
         File.WriteAllText("../../resources/LastPlayed.txt", worldName);
      }

      //static public void SetWorldNameToLastPlayed()
      //{
      //   worldName = File.ReadAllText("../../resources/LastPlayed.txt");
      //}

   }
}
