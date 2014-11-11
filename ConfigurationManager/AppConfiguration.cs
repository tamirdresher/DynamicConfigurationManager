using System;
using System.Collections.Generic;
using ConfigurationManager.Interfaces;

namespace ConfigurationManager
{
    public class AppConfiguration
    {
        public AppConfiguration()
        {
            ConfigurationElements=new List<IConfigurationElement>();
        }
        public List<IConfigurationElement> ConfigurationElements { get; set; }
        public Version Version { get; set; }
    }
}