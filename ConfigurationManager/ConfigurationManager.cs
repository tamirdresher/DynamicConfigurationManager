using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using DynamicConfigurationManager.Interfaces;
using DynamicConfigurationManager.Json;
using Newtonsoft.Json;

namespace DynamicConfigurationManager
{
    public class ConfigurationManager : DynamicObject, IConfigurationManager
    {
        private readonly IEnumerable<IConfigurationNode> _configurationNodes;

        public ConfigurationManager(IEnumerable<IConfigurationNode> configurationNodes)
        {
            _configurationNodes = configurationNodes;
        }

        public string DefaultConfigurationFileName { get { return "config.json"; } }

        public bool OpenConfiguration(string configFile)
        {
            return OpenConfiguration(configFile, Assembly.GetEntryAssembly().GetName().Version);
        }

        public bool OpenConfiguration(string configFile, Version version)
        {
            string json = "";
            if (File.Exists(configFile))
            {
                json = File.ReadAllText(configFile);
            }
            if (!OpenConfiguration(version, json)) return false;

            Save(configFile);

            return true;
        }

        public bool OpenConfiguration(Version version, string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                Binder = new LooseTypesBinder(),
                ObjectCreationHandling = ObjectCreationHandling.Replace,

                Converters = new[] { new ErrorConfigurationNodeConverter() }
            };
            AppConfiguration =
                JsonConvert.DeserializeObject<AppConfiguration>(json,
                    jsonSerializerSettings) ?? new AppConfiguration();

            //if (AppConfiguration.Version == version)
            //{
            //    return false;
            //}
            AppConfiguration.Version = version;
            foreach (var configNode in _configurationNodes)
            {
                configNode.Version = version;
                configNode.ValidateConfig(this);
            }
            return true;
        }

        public AppConfiguration AppConfiguration { get; private set; }
        public void Save(string configFile)
        {
            var ignoreClassesResolver = new IgnorableSerializerContractResolver();
            ignoreClassesResolver.Ignore(typeof(ErrorConfigurationNode), new ErrorConfigurationNodeConverter());

            var updatedConfig = JsonConvert.SerializeObject(AppConfiguration,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                    ContractResolver = ignoreClassesResolver,
                    Formatting = Formatting.Indented
                });
            File.WriteAllText(configFile, updatedConfig);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var configGroup = AppConfiguration.ConfigurationElements.FirstOrDefault(c => c.Name == binder.Name);
            if (configGroup != null)
            {
                result = configGroup;
                return true;
            }
            result = null;
            return false;
        }

        public dynamic this[string configGroupName]
        {
            get
            {
                var configGroup = AppConfiguration.ConfigurationElements.FirstOrDefault(c => c.Name == configGroupName);
                return configGroup;
            }
        }
        public dynamic AsDynamic()
        {
            return this;
        }

        public TConfigNode GetConfigNode<TConfigNode>() where TConfigNode : ConfigurationNode, new()
        {
            ConfigurationNode configNode = new TConfigNode();
            var pathDescriber = AsDynamic();
            var pathToNode = configNode.DescribePath(pathDescriber)[configNode.Name];
            return (TConfigNode)pathToNode;
            // configNode;
        }
    }
}