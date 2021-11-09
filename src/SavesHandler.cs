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

      static public void SaveToJson(ChunkHandler chunkHandler, string name)
      {
         string jsonString;
         var settings = new JsonSerializerSettings();
         settings.Converters.Add(new BlockConverter());
         jsonString = JsonConvert.SerializeObject(chunkHandler, Formatting.Indented, settings);

         File.WriteAllText(path + name + ".json", jsonString);
      }

      static public ChunkHandler RestoreFromJson(string name)
      {
         string jsonString;
         jsonString = File.ReadAllText(path + name + ".json");
         var settings = new JsonSerializerSettings();
         settings.Converters.Add(new BlockConverter());
         ChunkHandler chunkHandler = JsonConvert.DeserializeObject<ChunkHandler>(jsonString);
         chunkHandler.RestoreChunks();
         return chunkHandler;

      }

   }
}
