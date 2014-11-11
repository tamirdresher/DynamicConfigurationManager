using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicConfigurationManager;
using FakeItEasy;
using NUnit.Framework;

namespace ConfigurationManager.Tests
{
    [TestFixture]
    public class ConfigurationManagerTests
    {
        [Test]
        public void OpenConfiguration_TwoLevelConfigGroupsInJson_ConfigContainsTwoLevelGroupsInConfigObject()
        {
            string jsonConfig = @"{
                                    ""$type"": ""DynamicConfigurationManager.AppConfiguration, DynamicConfigurationManager"",
                                    ""ConfigurationElements"": [
                                      {
                                        ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                        ""Name"": ""Level1"",
                                        ""ConfigurationElements"": [
                                          {
                                            ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                            ""Name"": ""Level2"",         
                                          }
                                        ]
                                      }
                                    ],
                                    ""Version"": {
                                      ""$type"": ""System.Version, mscorlib"",
                                      ""Major"": 1
                                    }
                                  }";

            var configurationManager = new DynamicConfigurationManager.ConfigurationManager(Enumerable.Empty<ConfigurationNode>());
            configurationManager.OpenConfiguration(new Version(1, 0), jsonConfig);

            Assert.AreEqual("Level1",configurationManager.AppConfiguration.ConfigurationElements[0].Name);
            var secondLevel = configurationManager.AppConfiguration.ConfigurationElements[0] as ConfigurationGroup;
            Assert.AreEqual("Level2",secondLevel.ConfigurationElements[0].Name);
        }

        [Test]
        public void OpenConfiguration_ConfigVersionChanged_ConfigVersionUpdated()
        {
            string jsonConfig = @"{
                                    ""$type"": ""DynamicConfigurationManager.AppConfiguration, DynamicConfigurationManager"",
                                    ""ConfigurationElements"": [
                                      {
                                        ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                        ""Name"": ""Level1"",
                                        ""ConfigurationElements"": [
                                          {
                                            ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                            ""Name"": ""Level2"",         
                                          }
                                        ]
                                      }
                                    ],
                                    ""Version"": {
                                      ""$type"": ""System.Version, mscorlib"",
                                      ""Major"": 1
                                    }
                                  }";

            var klaConfigurationManager = new DynamicConfigurationManager.ConfigurationManager(Enumerable.Empty<ConfigurationNode>());
            var newVersion = new Version(2, 0);
            var configUpdated = klaConfigurationManager.OpenConfiguration(newVersion, jsonConfig);

            Assert.IsTrue(configUpdated);
            Assert.AreEqual(newVersion,klaConfigurationManager.AppConfiguration.Version);
        }

        [Test]
        public void OpenConfiguration_ConfigHasTwoLevelOfGroupsVersionIsTheSame_ConfigIsChanged()
        {
            string jsonConfig = @"{
                                    ""$type"": ""DynamicConfigurationManager.AppConfiguration, DynamicConfigurationManager"",
                                    ""ConfigurationElements"": [
                                      {
                                        ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                        ""Name"": ""Level1"",
                                        ""ConfigurationElements"": [
                                          {
                                            ""$type"": ""DynamicConfigurationManager.ConfigurationGroup, DynamicConfigurationManager"",
                                            ""Name"": ""Level2"",         
                                          }
                                        ]
                                      }
                                    ],
                                    ""Version"": {
                                      ""$type"": ""System.Version, mscorlib"",
                                      ""Major"": 1
                                    }
                                  }";

            var configurationManager = new DynamicConfigurationManager.ConfigurationManager(Enumerable.Empty<ConfigurationNode>());
            var newVersion = new Version(1, 0,0,0);
            var configUpdated = configurationManager.OpenConfiguration(newVersion, jsonConfig);

            Assert.IsTrue(configUpdated);
            var configurationElements = configurationManager.AppConfiguration.ConfigurationElements.OfType<ConfigurationGroup>().ToList();
            Assert.AreEqual(1, configurationElements.Count());
            Assert.AreEqual(1, configurationElements.Single().ConfigurationElements.Count);
        }

        [Test]
        public void OpenConfiguration_ConfigIsEmpty_EmptyConfigIsCreatedWithProvidedVersion()
        {
            string jsonConfig = @"";

            var configurationManager = new DynamicConfigurationManager.ConfigurationManager(Enumerable.Empty<ConfigurationNode>());
            var newVersion = new Version(2, 0);
            var configUpdated = configurationManager.OpenConfiguration(newVersion, jsonConfig);

            Assert.IsTrue(configUpdated);
            Assert.AreEqual(newVersion, configurationManager.AppConfiguration.Version);
        }

       }
}
