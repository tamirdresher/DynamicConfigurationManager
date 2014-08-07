using System.Collections.Generic;
using ConfigurationManager;
using ConfigurationManager.ConfigurationProperties;

namespace SampleConfigLib
{
    public class ArolModuleConfigDemo:ConfigurationNode
    {
        public ArolModuleConfigDemo()
            : base("ArolModuleConfigDemo")
        {
        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            yield return new StringProperty("RunMode","this is the run mode","coarse");
            yield return new NumericProperty<double>("NAmin","wow",0.0,1.0,0.0,"NA unit");
           // yield return new EnumProperty<Apodizer>("Apodizer", "this is apodizer!!!", Apodizer.TH);
            yield return new ListProperty<float>("WaveLength", "leksfj", new[] {405f, 420},405);
        }

        public double NAmin { get { return (double)this["NAmin"]; } set { this["NAmin"] = value; } }
        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.MTD.AROL.ArolModuleConfigDemo;
        }
    }

    public enum Apodizer 
    {
        TH,
        FA,
        QP
    }
}