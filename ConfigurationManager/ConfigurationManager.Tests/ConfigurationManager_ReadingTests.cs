using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurationManager.ConfigurationProperties;
using FakeItEasy;
using NUnit.Framework;

namespace ConfigurationManager.Tests
{
    [TestFixture]
    public class ConfigurationManager_ReadingTests
    {
        public class ThreeLevelsDeepConfigNodeWithStringProp : ConfigurationNode
        {
            public const string StringPropDefaultValue = "SomeText";
            public ThreeLevelsDeepConfigNodeWithStringProp()
                : base("ThreeLevelsDeepConfigNodeWithStringProp")
            {
            }

            public override IEnumerable<IConfigurationProperty> CreateProperties()
            {
                yield return new StringProperty("StringProp", "this is the string prop", StringPropDefaultValue);
            }

            public string AtringProp { get { return (string)this["StringProp"]; } set { this["StringProp"] = value; } }
            public override object DescribePath(dynamic pathDescriber)
            {
                return pathDescriber.Level1.Level2.Level3;
            }
        }
        [Test]
        public void OpenConfiguration_ReadStringPropWithIndexers_ReturnsPropDefaultValue()
        {
           
            var configurationManager = new ConfigurationManager(new []{new ThreeLevelsDeepConfigNodeWithStringProp()});
            configurationManager.OpenConfiguration(new Version(1, 0),""/*empty json input*/);

            Assert.AreEqual(ThreeLevelsDeepConfigNodeWithStringProp.StringPropDefaultValue, configurationManager["Level1"]["Level2"]["Level3"]["ThreeLevelsDeepConfigNodeWithStringProp"]["StringProp"]);
      
        }
    }
}