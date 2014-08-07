using System;
using System.Collections.Generic;
using ConfigurationManager.ConfigurationProperties;
using FakeItEasy;
using NUnit.Framework;

namespace ConfigurationManager.Tests.ConfigurationProperties
{
    [TestFixture]
    public class ConfigurationPropertyTests
    {
        [Test]
        public void Update_NewPropetyHasDifferentVersionOverideIsFalse_OldValueIsKept()
        {
            var configName = "SomeName";
            int someOldValue = 7;
            var oldConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);

            var oldConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", 0, 20, 10)
            {
                Version = new Version(1, 0),
                Value = someOldValue
            };
            var newDefaultValue = 0;
            var newMaximum = 20;
            var newMinimum = 10;
            var newVersion = new Version(2, 0);
            var newConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", newDefaultValue, newMaximum, newMinimum) { Version = newVersion };

            A.CallTo(() => oldConfigurationNode.CreateProperties()).Returns(new[] { oldConfigurationProperty });
            A.CallTo(() => newConfigurationNode.CreateProperties()).Returns(new[] { newConfigurationProperty });

            oldConfigurationNode.Version = new Version(1, 0, 0, 0);
            newConfigurationNode.Version = newVersion;

            oldConfigurationProperty.Update(newConfigurationProperty, oldConfigurationNode);

            var configurationProperty = oldConfigurationNode.Properties[0] as NumericProperty<int>;
            Assert.AreEqual(someOldValue, configurationProperty.Value);
        }
        [Test]
        public void Update_NewPropetyHasDifferentVersionOverideIsTrue_DefaultValueReplacedTheOldValue()
        {
            var configName = "SomeName";
            int someOldValue = 7;
            var oldConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);

            var oldConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", 0, 20, 10)
            {
                Version = new Version(1, 0),
                Value = someOldValue
            };
            var newDefaultValue = 0;
            var newMaximum = 20;
            var newMinimum = 10;
            var newVersion = new Version(2, 0);
            var newConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", newDefaultValue, newMaximum, newMinimum)
            {
                Version = newVersion,
                OverrideOldValue = true
            };

            A.CallTo(() => oldConfigurationNode.CreateProperties()).Returns(new[] { oldConfigurationProperty });
            A.CallTo(() => newConfigurationNode.CreateProperties()).Returns(new[] { newConfigurationProperty });

            oldConfigurationNode.Version = new Version(1, 0, 0, 0);
            newConfigurationNode.Version = newVersion;

            oldConfigurationProperty.Update(newConfigurationProperty, oldConfigurationNode);

            var configurationProperty = oldConfigurationNode.Properties[0] as NumericProperty<int>;
            Assert.AreEqual(newDefaultValue, configurationProperty.Value);
        }
        [Test]
        public void Update_NewPropetyHasDifferentVersion_PropertyIsChangedInNode()
        {
            var configName = "SomeName";

            var oldConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var newConfigurationNode = ConfigurationNodeTestHelper.CreateConfigurationNodeFake(configName);
            var oldConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", 0, 20, 10) { Version = new Version(1, 0) };
            var newDefaultValue = 0;
            var newMaximum = 20;
            var newMinimum = 10;
            var newVersion = new Version(2, 0);
            var newConfigurationProperty = new NumericProperty<int>("aNumericProperty", "hello description", newDefaultValue, newMaximum, newMinimum) { Version = newVersion };

            A.CallTo(() => oldConfigurationNode.CreateProperties()).Returns(new[] { oldConfigurationProperty });
            A.CallTo(() => newConfigurationNode.CreateProperties()).Returns(new[] { newConfigurationProperty });

            oldConfigurationNode.Version = new Version(1, 0, 0, 0);
            newConfigurationNode.Version = newVersion;

            oldConfigurationProperty.Update(newConfigurationProperty, oldConfigurationNode);

            var configurationProperty = oldConfigurationNode.Properties[0] as NumericProperty<int>;
            Assert.AreEqual(newVersion, configurationProperty.Version);
            Assert.AreEqual(newMaximum, configurationProperty.Maximum);
            Assert.AreEqual(newMinimum, configurationProperty.Minimum);

        }
    }
}