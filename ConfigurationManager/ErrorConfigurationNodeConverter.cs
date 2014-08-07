using System;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConfigurationManager
{
    public class ErrorConfigurationNodeConverter : JsonConverter
    {
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public object Create(Type objectType)
        {
            return new ErrorConfigurationNode();
        }

       
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // base.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //var o = base.ReadJson(reader, objectType, existingValue, serializer);
            var o=new ErrorConfigurationNode();
            var errorConfigurationNode = o as ErrorConfigurationNode;
            errorConfigurationNode.Name = "ERROR - " + errorConfigurationNode.Name;
            return o;
        }

        public override bool CanConvert(Type objectType)
        {
           
            return objectType == typeof(ErrorConfigurationNode);
        }
    }
}