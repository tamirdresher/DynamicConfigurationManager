using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurationManager.ConfigurationProperties;
using FakeItEasy;
using FakeItEasy.Core;
using NUnit.Framework;

namespace ConfigurationManager.Tests
{
    [TestFixture]
    public class ConfigurationNodeTests
    {

        [Test]
        public void Update_SameVersion_NoChangeMadeOnProperties()
        {
            var configName = "SomeName";


            var oldConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var configurationProperty = A.Fake<IConfigurationProperty>();
            var configurationManager = A.Fake<IConfigurationManager>();


            A.CallTo(() => configurationProperty.Name).Returns("prop1");
            A.CallTo(() => oldConfigurationNode.CreateProperties()).Returns(new[] { configurationProperty });
            A.CallTo(() => configurationManager.AppConfiguration).Returns(new AppConfiguration()
            {
                ConfigurationElements = new List<IConfigurationElement>()
                {
                    oldConfigurationNode
                }
            });
            A.CallTo(() => newConfigurationNode.DescribePath(A<object>._))
                .Invokes(x =>
                {
                    var arg = x.Arguments.Get<dynamic>(0).SomeName;
                });
            oldConfigurationNode.Version = new Version(1, 0, 0, 0);
            newConfigurationNode.Version = new Version(1, 0, 0, 0);

            newConfigurationNode.ValidateConfig(configurationManager);

            A.CallTo(oldConfigurationNode).Where(x => x.Method.Name == "UpdateNodeValues").MustNotHaveHappened();
        }

        [Test]
        public void Update_NewVersion_VersionUpdated()
        {
            var configName = "SomeName";

            var oldConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationProperty = A.Fake<IConfigurationProperty>();
            var oldConfigurationProperty = A.Fake<IConfigurationProperty>();
            var configurationManager = A.Fake<IConfigurationManager>();

            A.CallTo(() => newConfigurationProperty.Name).Returns("prop1");
            A.CallTo(() => oldConfigurationProperty.Name).Returns("prop1");

            A.CallTo(() => oldConfigurationNode.CreateProperties()).Returns(new[] { oldConfigurationProperty });
            A.CallTo(() => newConfigurationNode.CreateProperties()).Returns(new[] { newConfigurationProperty });
            A.CallTo(() => configurationManager.AppConfiguration).Returns(new AppConfiguration()
            {
                ConfigurationElements = new List<IConfigurationElement>()
                {
                    oldConfigurationNode
                }
            });

            A.CallTo(() => newConfigurationNode.DescribePath(A<object>._))
                .Invokes(x =>
                {
                    //var arg = "Level1"; //x.Arguments.Get<dynamic>(0).SomeName;
                });

            var oldVersion = new Version(1, 0, 0, 0);
            oldConfigurationNode.Version = oldVersion;
            var newVersion = new Version(2, 0);
            newConfigurationNode.Version = newVersion;

            newConfigurationNode.ValidateConfig(configurationManager);

            var configurationElement = configurationManager.AppConfiguration.ConfigurationElements[0] as ConfigurationNode;


            Assert.AreEqual(newVersion, configurationElement.Version);
            A.CallTo(() => oldConfigurationProperty.Update(newConfigurationProperty, oldConfigurationNode)).MustHaveHappened();

        }
    }


}