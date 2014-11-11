using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DynamicConfigurationManager.Interfaces;
using Newtonsoft.Json;

namespace DynamicConfigurationManager.ConfigurationProperties
{
    public abstract class ConfigurationProperty<T> : INotifyPropertyChanged,IConfigurationProperty<T>
    {
        private T _value;
        private Version _lastUpdatedOn;
        private Version _version;

        protected ConfigurationProperty(string name,string description, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name cant be empty", "name");
            }
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("description cant be empty","description");
            }
            Name = name;
            Description = description;
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        public string Name { get; set; }
        [JsonProperty(Order = 4)]

        public string Description { get; set; }
        public T DefaultValue { get; set; }
        [JsonProperty(Order = 1)]
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
                LastUpdatedOn = Version;
            }
        }
        [JsonProperty(Order = 3)]

        public Version Version
        {
            get { return _version; }
            set
            {
                _version = value;
                if (LastUpdatedOn==null)
                {
                    LastUpdatedOn = value;
                }
            }
        }
        [JsonProperty(Order = 2)]

        public Version LastUpdatedOn
        {
            get { return _lastUpdatedOn; }
            set
            {
                _lastUpdatedOn = value;
                OnPropertyChanged();
                
            }
        }

        public bool OverrideOldValue { get; set; }

        public virtual void Update(IConfigurationProperty newConfigurationProperty, ConfigurationNode parentConfigurationNode)
        {
            if (newConfigurationProperty == null)
            {
                //this means that the property doesnt exist anymore in the containing ConfigurationNode 
                parentConfigurationNode.Properties.Remove(this);
                return;
            }
            var newProp = newConfigurationProperty as ConfigurationProperty<T>;
            Validate(newProp,parentConfigurationNode);
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object val)
        {
            Value = (T) val;
        }

        protected virtual void Validate(ConfigurationProperty<T> newProp, ConfigurationNode configurationNode)
        {
            if (newProp == null)
            {
                //the given new property is not in the same type as we are. we cant work with it
                throw new ArgumentException(GetType().ToString()+" can only work with property of the same type", "newProp");
            }
            
            if (this.Version == newProp.Version)
            {
                //same version so no need to change it
                return;
            }
            
            if (!newProp.OverrideOldValue)
            {
                //we need to keep the old value - but since the newprop might have other properties that changed
                //such as Min and Max, we copy the old-value to the new prop
                newProp.Value = Value;
            }
            
            configurationNode.Properties.Remove(this);
            configurationNode.Properties.Add(newProp);
        }


        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}