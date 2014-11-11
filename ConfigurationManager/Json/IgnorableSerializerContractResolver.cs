using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DynamicConfigurationManager.Json
{
    public class IgnorableSerializerContractResolver : DefaultContractResolver
    {
        protected readonly IDictionary<Type,JsonConverter> Ignores;

        public IgnorableSerializerContractResolver()
        {
            this.Ignores = new Dictionary<Type, JsonConverter>();
        }

        /// <summary>
        /// Explicitly ignore the given type
        /// </summary>
        /// <param name="type"></param>
        public void Ignore(Type type)
        {
            if (!Ignores.ContainsKey(type))
            {
                Ignores.Add(type,_ignoreClassesConverter);
                
            }


        } public void Ignore(Type type,JsonConverter converter)
        {
            Ignores.Add(type,converter);


        }

        
        private static readonly IgnoreClassesConverter _ignoreClassesConverter = new IgnoreClassesConverter();
        protected override JsonContract CreateContract(Type objectType)
        {
            var typeContract= base.CreateContract(objectType);
            if (Ignores.ContainsKey(objectType))
            {
                typeContract.Converter = Ignores[objectType];
            }
            else
            {
                typeContract.Converter = base.CreateContract(objectType).Converter;
            }
            return typeContract;
        }

        protected override JsonISerializableContract CreateISerializableContract(Type objectType)
        {
            return base.CreateISerializableContract(objectType);
        }
    }
}