using System;
using Newtonsoft.Json;

namespace ConfigurationManager
{
    public class IgnoreClassesConverter : Newtonsoft.Json.Converters.CustomCreationConverter<object>
    {
        public override bool CanWrite
        {
            get { return true; }
        }

        public override object Create(Type objectType)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // base.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}