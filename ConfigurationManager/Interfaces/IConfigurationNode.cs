using System;
using System.Collections.Generic;

namespace ConfigurationManager
{
    public interface IConfigurationNode: IConfigurationElement
    {
        Version Version { get; set; }
        List<IConfigurationProperty> Properties { get; set; }
        void ValidateConfig(IConfigurationManager configurationManager);
    }
}