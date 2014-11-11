using System;
using System.Collections.Generic;

namespace ConfigurationManager.Interfaces
{
    public interface IConfigurationNode: IConfigurationGroup
    {
        Version Version { get; set; }
        List<IConfigurationProperty> Properties { get; set; }
        void ValidateConfig(IConfigurationManager configurationManager);
    }
}