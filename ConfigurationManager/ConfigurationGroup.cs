﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicConfigurationManager.Interfaces;
using Newtonsoft.Json;

namespace DynamicConfigurationManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigurationGroup : DynamicObject, IConfigurationGroup
    {
        private List<IConfigurationElement> _configurationElements;

        public ConfigurationGroup()
            : this("")
        {

        }
        public ConfigurationGroup(string name)
        {
            Name = name;
            //ConfigurationElements=new List<IConfigurationElement>();
        }

        [JsonProperty]
        public virtual string Name { get; set; }

        [JsonProperty]
        public List<IConfigurationElement> ConfigurationElements
        {
            get { return _configurationElements ?? (_configurationElements = new List<IConfigurationElement>()); }
            set { _configurationElements = value; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (base.TryGetMember(binder, out result))
            {
                return true;
            }
            
            var configGroup = GetConfigurationElement(binder.Name);
            if (configGroup != null)
            {
                result = configGroup;
                return true;
            }

            //else
            //{
            //    var configurationGroup = new ConfigurationGroup(binder.Name);
            //    ConfigurationElements.Add(configurationGroup);
            //    result = configurationGroup;
            //    return true;
            //}
            throw new KeyNotFoundException(string.Format("Cant find configuration element with the name:{0} under ConfigurationGroup:{1}", binder.Name, Name));


        }

        private IConfigurationElement GetConfigurationElement(string name)
        {
            GetMemberBinder binder;
            return ConfigurationElements.FirstOrDefault(c => c.Name == name);
        }

        public virtual dynamic this[string configElementName]
        {
            get { return GetConfigurationElement(configElementName); }
        }
    }
}