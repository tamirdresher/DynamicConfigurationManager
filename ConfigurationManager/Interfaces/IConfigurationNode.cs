using System;
using System.Collections.Generic;

namespace DynamicConfigurationManager.Interfaces
{
    public interface IConfigurationNode: IConfigurationGroup
    {
        Version Version { get; set; }
        List<IConfigurationProperty> Properties { get; set; }
        void ValidateConfig(IConfigurationManager configurationManager);
    }
}