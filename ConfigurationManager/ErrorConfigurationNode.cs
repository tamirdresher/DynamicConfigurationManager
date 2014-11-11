using System.Collections.Generic;
using System.Linq;
using ConfigurationManager.Interfaces;
using Newtonsoft.Json;

namespace ConfigurationManager
{
    [JsonConverter(typeof(ErrorConfigurationNodeConverter))]
    public class ErrorConfigurationNode : ConfigurationNode
    {
        private string _name;

        public ErrorConfigurationNode() : base("")
        {
        }

        public override IEnumerable<IConfigurationProperty> CreateProperties()
        {
            return Enumerable.Empty<IConfigurationProperty>();
        }

        public override object DescribePath(dynamic pathDescriber)
        {
            return null;
        }

        public override string Name
        {
            get { return _name; }
            set { _name ="ERROR - "+ value; }
        }
    }
}