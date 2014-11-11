using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurationManager;
using ConfigurationManager.ConfigurationProperties;
using ConfigurationManager.Interfaces;

namespace SampleConfigLib
{
    public class AConfigurationInAnotherConfiguration : ConfigurationNode
    {
        public AConfigurationInAnotherConfiguration()
            : base("A Configuration In Another Configuration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","SomeTextValue", "ImportantValue"),
                new NumericProperty<double>("a double","a double", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","a int", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","ConsoleColors", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","StringListProperty", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.Level2.AnotherConfiguration;
        }
    }
    public class AnotherConfiguration : ConfigurationNode
    {
        public AnotherConfiguration()
            : base("AnotherConfiguration")
        {
           
        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","SomeTextValue", "ImportantValue"),
                new NumericProperty<double>("a double","a double", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","a int", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","ConsoleColors", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","StringListProperty", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.Level2;
        }
    }

    
    public class AnotherConfiguration3 : ConfigurationNode
    {
        public AnotherConfiguration3()
            : base("AnotherConfiguration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","SomeTextValue", "ImportantValue"),
                new NumericProperty<double>("a double","a double", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","a int", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","ConsoleColors", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","StringListProperty", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.Level2.Level3.AnotherConfiguration;
        }
    }
    public class AnotherConfiguration4 : ConfigurationNode
    {
        public AnotherConfiguration4()
            : base("AnotherConfiguration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","SomeTextValue", "ImportantValue"),
                new NumericProperty<double>("a double","a double", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","a int", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","a int", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","a int", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.AnotherConfiguration4;
        }
    }
    public class AnotherConfiguration5 : ConfigurationNode
    {
        public AnotherConfiguration5()
            : base("AnotherConfiguration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","hello description", "ImportantValue"),
                new NumericProperty<double>("a double","hello description", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","hello description", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","hello description", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","hello description", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1.AnotherConfiguration5;
        }
    }

    public class AnotherConfiguration6 : ConfigurationNode
    {
        public AnotherConfiguration6()
            : base("AnotherConfiguration")
        {

        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            var listItems = Enumerable.Range(1, 10).Select(i => "Item " + i).ToArray();
            return new List<IConfigurationProperty>
            {
                new StringProperty("SomeTextValue","a int", "ImportantValue"),
                new NumericProperty<double>("a double","a int", 15.2, 109.90, -1.5),
                new NumericProperty<int>("a int","a int", 26 ,51, 20),
                new EnumProperty<ConsoleColor>("ConsoleColors","a int", ConsoleColor.Blue),
                new ListProperty<string>("StringListProperty","a int", listItems,"Item 5")
            };
        }


        public override object DescribePath(dynamic pathDescriber)
        {
            return pathDescriber.Level1_1.AnotherConfiguration6;
        }
    }
}
