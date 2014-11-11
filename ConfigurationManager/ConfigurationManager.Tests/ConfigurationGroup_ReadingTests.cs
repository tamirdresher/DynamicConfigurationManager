using DynamicConfigurationManager;
using NUnit.Framework;

namespace ConfigurationManager.Tests
{
    [TestFixture]
    public class ConfigurationGroup_ReadingTests
    {
        [Test]
        public void Indexer_Contains1InnerGroup_ReturnsInnerGroup()
        {
            var configurationGroup = new ConfigurationGroup("Group1");
            configurationGroup.ConfigurationElements.Add(new ConfigurationGroup("Group2"));

            var innerGrp = configurationGroup["Group2"];


            Assert.AreEqual("Group2", innerGrp.Name);
        }
        
        [Test]
        public void Indexer_Contains1InnerGroupWithSpacesInName_ReturnsInnerGroup()
        {
            var configurationGroup = new ConfigurationGroup("Group1");
            var innerGroupName = "A Group 2";
            configurationGroup.ConfigurationElements.Add(new ConfigurationGroup(innerGroupName));

            var innerGrp = configurationGroup[innerGroupName];


            Assert.AreEqual(innerGroupName, innerGrp.Name);
        }
    }
}