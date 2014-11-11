using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicConfigurationManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SampleConfigLib;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new AppConfiguration();
            configuration.ConfigurationElements.Add(new ConfigurationGroup("Level1")
            {
                ConfigurationElements =
                {
                    new ConfigurationGroup("Level2")
                    {
                        ConfigurationElements =
                        {
                            new ConfigurationGroup("Level3")
                            //{
                            //    ConfigurationElements =
                            //    {
                            //        new SomeConfiguration()
                            //    }
                            //}
                        }
                    }
                }
            });


            var memoryTraceWriter = new MemoryTraceWriter();
            //string output = JsonConvert.SerializeObject(klaConfiguration,
            //    new JsonSerializerSettings()
            //    {
            //        TypeNameHandling = TypeNameHandling.All,
            //        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            //        TraceWriter = memoryTraceWriter
            //    });
            var klaconfigJson = "config.json";
            //File.WriteAllText(klaconfigJson, output);
            //Console.WriteLine(output);
            //Console.WriteLine("Trace:"+memoryTraceWriter.ToString());

            var cm = new global::DynamicConfigurationManager.ConfigurationManager(new List<ConfigurationNode> { new SomeConfiguration(), new ArolModuleConfigDemo() });
            cm.OpenConfiguration(klaconfigJson);
            {
                var resolution = cm.AsDynamic().Level1.Level2.Level3.SomeConfiguration.Resolution;
                var deg = cm.AsDynamic().Level1.Level2.Level3.SomeConfiguration.Degree;
                var degName = cm.AsDynamic().Level1.Level2.Level3.SomeConfiguration.Name;
                var enumProp = cm.AsDynamic().Level1.Level2.Level3.SomeConfiguration.EnumProperty;
            }
            {
                dynamic dynSomeConfig = cm.GetConfigNode<SomeConfiguration>();
                var resolution = dynSomeConfig.Resolution;
                var deg = dynSomeConfig.Degree;
                var degName = dynSomeConfig.Name;
                var enumProp = dynSomeConfig.EnumProperty;
            }

            {
                var dynSomeConfig = cm.GetConfigNode<SomeConfiguration>();
                var resolution = dynSomeConfig["Resolution"];
                var deg = dynSomeConfig["Degree"];
                var enumProp = dynSomeConfig["EnumProperty"];
            }

            {
                var dynSomeConfig = cm.GetConfigNode<SomeConfiguration>();
                var resolution = dynSomeConfig.Resolution;
                dynSomeConfig.Resolution="bla";
                var deg = dynSomeConfig.Degree;
                var enumProp = dynSomeConfig.EnumProperty;
                cm.Save(klaconfigJson);


            }

            {
                dynamic dynSomeConfig = cm.GetConfigNode<ArolModuleConfigDemo>();
                var runmode = dynSomeConfig.RunMode;

                var runmode2=cm.AsDynamic().MTD.AROL.ArolModuleConfigDemo.RunMode;

                
                var runmode3 = cm.GetConfigNode<ArolModuleConfigDemo>()["RunMode"];

            }
            //var deserializeObject = JsonConvert.DeserializeObject<KLAConfiguration>(output,new JsonSerializerSettings(){TypeNameHandling = TypeNameHandling.Objects,DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});

        }
    }
}
