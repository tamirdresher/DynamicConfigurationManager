using System;

namespace DynamicConfigurationManager.Interfaces
{
    public interface IConfigurationManager
    {
        string DefaultConfigurationFileName { get; }
        /// <summary>
        /// open the configuration from the configFile path, using the executable version as the new config version
        /// </summary>
        /// <param name="configFile">the path to the config file</param>
        /// <returns>
        /// True- config was updated
        /// False - otherwise
        /// </returns>
        bool OpenConfiguration(string configFile);
        /// <summary>
        /// open the configuration from the configFile path, using the <paramref name="version"/> to check for updates
        /// </summary>
        /// <param name="configFile">the path to the config file</param>
        /// <param name="version">the new version of the config</param>
        /// <returns>
        /// True- config was updated
        /// False - otherwise
        /// </returns>
        bool OpenConfiguration(string configFile, Version version);
        TConfigNode GetConfigNode<TConfigNode>() where TConfigNode : ConfigurationNode, new();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">the new version of the config</param>
        /// <param name="configContent">the config text to load</param>
        /// <returns>
        /// True- config was updated
        /// False - otherwise
        /// </returns>
        bool OpenConfiguration(Version version, string configContent);

        AppConfiguration AppConfiguration { get; }
        /// <summary>
        /// Saves the configuration to the configFile
        /// </summary>
        /// <param name="configFile">path to the configFile</param>
        void Save(string configFile);
    }
}