using System.Collections.Generic;
using DynamicConfigurationManager;
using DynamicConfigurationManager.ConfigurationProperties;
using DynamicConfigurationManager.Interfaces;

namespace SampleConfigLib
{
    public class SomeConfiguration : ConfigurationNode
    {
        public SomeConfiguration()
            : base("SomeConfiguration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            return new List<IConfigurationProperty>
            {
                new StringProperty("Resolution","Resolution", "hello"),
                new NumericProperty<double>("Degree","Degree", 5.2, 100, 0),
                new EnumProperty<SomeEnum>("EnumProperty","EnumProperty", SomeEnum.Val2)
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

        public double Degree 
        {
            get
            {
                return (double)this["Degree"];
            }
            set
            {
                this["Degree"] = value;
            }
        }
        public SomeEnum EnumProperty
        {
            get
            {
                return (SomeEnum)this["EnumProperty"];
            }
            set
            {
                this["EnumProperty"] = value;
            }
        }

        public override object DescribePath(dynamic pathDescriber)
        {
            var x = pathDescriber.Level1.Level2.Level3.SomeConfiguration;
            return x;
        }
    }
}