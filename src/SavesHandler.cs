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
      private static string path = "../../saves/";

      static public void SaveToJson(ChunkHandler chunkHandler, Player player, string name)
      {
         string jsonWorld, jsonPlayer;
         var settings = new JsonSerializerSettings();
         settings.TypeNameHandling = TypeNameHandling.Objects;
         jsonWorld = JsonConvert.SerializeObject(chunkHandler, Formatting.Indented, settings);
         File.WriteAllText(path + name + "/worlddata.json", jsonWorld);
         settings.Converters.Add(new Vector2Converter());
         jsonPlayer = JsonConvert.SerializeObject(player, Formatting.Indented, settings);
         File.WriteAllText(path + name + "/playerdata.json", jsonPlayer);
      }

      static public ChunkHandler RestoreWorldFromJson(string name)
      {
         string jsonString;
         jsonString = File.ReadAllText(path + name + "/worlddata.json");
         var settings = new JsonSerializerSettings();
         //settings.Converters.Add(new BlockConverter());
         settings.TypeNameHandling = TypeNameHandling.Objects;
         ChunkHandler chunkHandler = JsonConvert.DeserializeObject<ChunkHandler>(jsonString, settings);
         chunkHandler.RestoreChunks();
         return chunkHandler;
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
