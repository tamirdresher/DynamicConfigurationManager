using FakeItEasy;

namespace ConfigurationManager.Tests
{
    public class ConfigurationNodeTestHelper
    {
        public static ConfigurationNode CreateConfigurationNodeFake(string configName)
        {
            var configurationNodeFake = A.Fake<ConfigurationNode>(o =>
            {
                o.WithArgumentsForConstructor(new object[] { configName });
                o.OnFakeCreated(fake =>
                {
                    A.CallTo(fake).Where(c => !c.Method.IsFinal && !c.Method.IsAbstract)
                        .CallsBaseMethod();
                    
                });
            });
            configurationNodeFake.Name = configName;
            return configurationNodeFake;
        }
    }
}