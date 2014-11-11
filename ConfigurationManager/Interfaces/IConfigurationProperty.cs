using System;

namespace DynamicConfigurationManager.Interfaces
{
    public interface IConfigurationProperty : IConfigurationElement
    {
        Version Version { set; get; }
        Version LastUpdatedOn { get; set; }
        bool OverrideOldValue { get;  }
        void Update(IConfigurationProperty newProp, ConfigurationNode parentConfigurationNode);
        object GetValue();
        void SetValue(object val);
    }

    public interface IConfigurationProperty<T> : IConfigurationProperty
    {
        T DefaultValue { get; set; }
        T Value { get; set; }
    }
}