using System.Collections.Generic;
using DynamicConfigurationManager;
using DynamicConfigurationManager.ConfigurationProperties;
using DynamicConfigurationManager.Interfaces;

namespace ConfigurationManager.IntergrationTests
{
    public class SomeTestNodeDummy : ConfigurationNode
    {
        public SomeTestNodeDummy()
            : base("SomeTestNodeDummy")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            return new List<IConfigurationProperty>
            {
                new StringProperty("Resolution", "hello description","hello"),
                new NumericProperty<double>("Degree","hello description", 5.2, 100, 0),
                new EnumProperty<SomeEnum>("EnumProperty","hello description", SomeEnum.Val2)
            };
        }

        public string Resolution
        {
            get
            {
                return this["Resolution"].ToString();
            }
            set
            {
                this["Resolution"] = value;
            }
        }

        public override object DescribePath(dynamic pathDescriber)
        {
            var x = pathDescriber.Level1.Level2.Level3.SomeTestNodeDummy;
            return x;
        }
    }
}