using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using shootcraft.src.blocks;

namespace shootcraft.src
{
   public class BlockConverter : JsonConverter
   {
      public override bool CanConvert(Type objectType)
      {
         return objectType == typeof(Block);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
         var jsonObject = JObject.Load(reader);
         var properties = jsonObject.Properties().ToList();
         if (properties[0].Value.ToString() == "air")
            return new AirBlock("air");
         if (properties[0].Value.ToString() == "dirt")
            return new DirtBlock("dirt");
         throw new NotImplementedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         var block = (Block)value;
         serializer.Serialize(writer, new { block.type });
      }
   }
}
