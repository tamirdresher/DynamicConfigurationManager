using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicConfigurationManager;
using NUnit.Framework;

namespace ConfigurationManager.IntergrationTests
{
    [TestFixture]
    public class ReadingPropertiesTests
    {

        [Test]
        public void ReadWithConfigManagerAsDynamic()
        {
            var cm = new DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeTestNodeDummy() });
            cm.OpenConfiguration(new Version(1, 0), "");

            var resolution = cm.AsDynamic().Level1.Level2.Level3.SomeTestNodeDummy.Resolution;
            var deg = cm.AsDynamic().Level1.Level2.Level3.SomeTestNodeDummy.Degree;
            var degName = cm.AsDynamic().Level1.Level2.Level3.SomeTestNodeDummy.Name;
            var enumProp = cm.AsDynamic().Level1.Level2.Level3.SomeTestNodeDummy.EnumProperty;

            Assert.IsNotNull(resolution);
            Assert.IsNotNull(deg);
            Assert.IsNotNull(degName);
            Assert.IsNotNull(enumProp);

        }
        [Test]
        public void ConfigManager_GetConfigNode_UsingIndexer()
        {
            var cm = new DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeTestNodeDummy() });
            cm.OpenConfiguration(new Version(1, 0), "");

            var dynSomeConfig = cm.GetConfigNode<SomeTestNodeDummy>();
            var resolution = dynSomeConfig["Resolution"];
            var deg = dynSomeConfig["Degree"];
            var enumProp = dynSomeConfig["EnumProperty"];

            Assert.IsNotNull(resolution);
            Assert.IsNotNull(deg);
            Assert.IsNotNull(enumProp);
        }

        [Test]
        public void ConfigManager_GetConfigNode_UsingConfigNodeAsDynamic()
        {
            var cm = new DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeTestNodeDummy() });
            cm.OpenConfiguration(new Version(1, 0), "");

            dynamic dynSomeConfig = cm.GetConfigNode<SomeTestNodeDummy>();
            var resolution = dynSomeConfig.Resolution;
            var deg = dynSomeConfig.Degree;

            //this is a real property and not a ConfigurationProperty
            var degName = dynSomeConfig.Name;

            var enumProp = dynSomeConfig.EnumProperty;

            Assert.IsNotNull(resolution);
            Assert.IsNotNull(deg);
            Assert.IsNotNull(degName);
            Assert.IsNotNull(enumProp);
        }

        [Test]
        public void ConfigManager_GetConfigNode_UsingRegularProperty()
        {
            var cm = new DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeTestNodeDummy() });
            cm.OpenConfiguration(new Version(1, 0), "");

            SomeTestNodeDummy dynSomeConfig = cm.GetConfigNode<SomeTestNodeDummy>();
            var resolution = dynSomeConfig.Resolution;
           
            Assert.IsNotNull(resolution);
            
        }

        [Test]
        public void ConfigManager_UsingAsDynamic_NonExistingPath_ThrowsKeyNotFoundException()
        {
            var cm = new DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeTestNodeDummy() });
            cm.OpenConfiguration(new Version(1, 0), "");

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var nonExistValue = cm.AsDynamic().Level1.NotExistLevel;
            });

        }
    }
}
