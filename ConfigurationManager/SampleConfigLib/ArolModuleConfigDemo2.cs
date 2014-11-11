using System.Collections.Generic;
using DynamicConfigurationManager;
using DynamicConfigurationManager.ConfigurationProperties;
using DynamicConfigurationManager.Interfaces;

namespace SampleConfigLib
{
    public class ArolModuleConfigDemo2 : ConfigurationNode
    {
        public ArolModuleConfigDemo2()
            : base("ArolModuleConfigDemo2")
        {
        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            yield return new StringProperty("RunMode", "this is the run mode", "coarse");
            yield return new NumericProperty<double>("NAmin", "wow", 0.0, 1.0, 0.0, "NA unit");
            yield return new EnumProperty<Apodizer>("Apodizer", "this is apodizer!!!", Apodizer.TH);
            yield return new ListProperty<float>("WaveLength", "leksfj", new[] { 405f, 420 }, 405);
        }

        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.MTD.AROL.ArolModuleConfigDemo2;
        }
    }
}