using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace shootcraft.src
{
   static class SavesHandler
   {
      public static string path = "../../saves/";

      static public void SaveToJson(Player player, string name)
      {
         string jsonString;
         var settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;
         settings.Converters.Add(new Vector2Converter());
         jsonString = JsonConvert.SerializeObject(player, Formatting.Indented, settings);
         File.WriteAllText(path + name + "/playerdata.json", jsonString);
      }



      static public Player RestorePlayerFromJson(string name)
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
   }
}
