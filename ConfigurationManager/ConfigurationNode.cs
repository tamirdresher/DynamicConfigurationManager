using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicConfigurationManager.Interfaces;
using Newtonsoft.Json;

namespace DynamicConfigurationManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ConfigurationNode : ConfigurationGroup, IConfigurationNode
    {
        private Version _version;
        private List<IConfigurationProperty> _properties;

        protected ConfigurationNode(string name)
        {
            Name = name;
        }
        public abstract IEnumerable<IConfigurationProperty> CreateProperties();
        public abstract object DescribePath(dynamic pathDescriber);
        //[JsonProperty]
        //public virtual string Name { get; set; }
        [JsonProperty]
        public Version Version
        {
            get { return _version; }
            set
            {
                _version = value;
                foreach (var configurationProperty in Properties)
                {
                    configurationProperty.Version = Version;
                }
            }
        }

        [JsonProperty]
        public List<IConfigurationProperty> Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new List<IConfigurationProperty>(CreateProperties());
                }
                return _properties;
            }
            set { _properties = value; }
        }

        [JsonIgnore]
        public object this[string propName]
        {
            get
            {
                var prop = Properties.SingleOrDefault(p => p.Name == propName);
                if (prop == null)
                {
                    throw new ArgumentException(string.Format("property with the name:'{0}' doesnt exist", propName));
                }
                return prop.GetValue();
            }
            set
            {
                var prop = Properties.SingleOrDefault(p => p.Name == propName);
                if (prop == null)
                {
                    throw new ArgumentException(string.Format("property with the name:'{0}' doesnt exist", propName));
                }
                prop.SetValue(value);
            }
        }
        public virtual void ValidateConfig(IConfigurationManager configurationManager)
        {
            var oldNode = CreateConfigElementIfNotExist(configurationManager);
            if (oldNode.Version != this.Version)
            {
                oldNode.UpdateNodeValues(this);
                var newConfigProperties = this.CreateProperties().ToList();

                oldNode.UpdateProperties(newConfigProperties);
            }
        }

        protected virtual void UpdateProperties(List<IConfigurationProperty> newConfigProperties)
        {
            var sameProps = Properties.Intersect(newConfigProperties,
                new LambdaComparer<IConfigurationProperty>((left, right) => left.Name == right.Name)).ToList();
            var deletedOldProp = Properties.Except(sameProps).ToList();
            foreach (var deletedProp in deletedOldProp)
            {
                Properties.Remove(deletedProp);
            }
            foreach (var newProp in newConfigProperties)
            {
                newProp.Version = Version;
                var oldProp = Properties.FirstOrDefault(p => p.Name == newProp.Name);
                if (oldProp == null)
                {
                    Properties.Add(newProp);
                }
                else
                {
                    oldProp.Update(newProp, this);
                }
            }
        }

        protected virtual void UpdateNodeValues(ConfigurationNode newNode)
        {
            this.Version = newNode.Version;
        }

        private ConfigurationNode CreateConfigElementIfNotExist(IConfigurationManager configurationManager)
        {
            var pathDescriber = new PathDescriber();
            DescribePath(pathDescriber);
            var pathParts = pathDescriber.Path.Split(new []{'.'},StringSplitOptions.RemoveEmptyEntries);
            IList<IConfigurationElement> elements = configurationManager.AppConfiguration.ConfigurationElements;
            foreach (var pathPart in pathParts)
            {
                var configGroup = elements.FirstOrDefault(e => e.Name == pathPart) as ConfigurationGroup;
                if (configGroup != null)
                {
                    elements = configGroup.ConfigurationElements;
                }
                else
                {
                    configGroup = new ConfigurationGroup(pathPart);
                    elements.Add(configGroup);
                    elements = configGroup.ConfigurationElements;
                }
            }
            var configurationGroup = elements.FirstOrDefault(e => e.Name == this.Name) as ConfigurationGroup;
            if (configurationGroup == null)
            {
                configurationGroup = this;
                elements.Add(this);
            }
            else if(!(configurationGroup is ConfigurationNode))//could be that some other config node was scanned first, so a config group was created instead of our config node
            {
                this.ConfigurationElements.AddRange(configurationGroup.ConfigurationElements);
                elements.Remove(configurationGroup);
                elements.Add(this);
                configurationGroup = this;
            }
            return configurationGroup as ConfigurationNode;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!base.TryGetMember(binder, out result))
            {
                if (this.Properties.Any(p=>p.Name==binder.Name))
                {
                    result = this[binder.Name];
                }
                else
                {
                    return false;
                    
                }
            }
            return true;
        }
        
    }
}