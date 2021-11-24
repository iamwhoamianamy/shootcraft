using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OpenTK;

using shootcraft.src.blocks;

namespace shootcraft.src
{
   public class Vector2Converter : JsonConverter
   {
      public override bool CanConvert(Type objectType)
      {
         return objectType == typeof(Vector2);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
         Vector2 vector;
         var jsonObject = JObject.Load(reader);
         var properties = jsonObject.Properties().ToList();
         vector.X = Single.Parse(properties[1].Value.ToString());
         vector.Y = Single.Parse(properties[2].Value.ToString());
         return vector;
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         var vector = (Vector2)value;
         serializer.Serialize(writer, new { vector.X, vector.Y });
      }
   }
}
