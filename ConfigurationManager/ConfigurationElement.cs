﻿using System.ComponentModel;
using DynamicConfigurationManager.Interfaces;
using Newtonsoft.Json.Linq;

namespace DynamicConfigurationManager
{
    public class ConfigurationElementImpl<T> : IConfigurationElement
    {
        public ConfigurationElementImpl()
        {
            SomeEnumVal=SomeEnum.Val3;
        }

        public T TValue { get; set; }
        public int ANumber { get; set; }
        [DefaultValue(SomeEnum.Val3)]
        public SomeEnum SomeEnumVal { get; set; }
       

        public void Read(JObject json)
        {
            
        }

        public void Write(JObject json)
        {
            
            
        }

        public string Name { get; private set; }
    }

    public enum SomeEnum
    {
        Val1,
        Val2,
        Val3
    }
}
