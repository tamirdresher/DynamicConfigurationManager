using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicConfigurationManager.Interfaces
{
    public interface IConfigurationGroup : IConfigurationElement
    {
        [JsonProperty]
        List<IConfigurationElement> ConfigurationElements { get; set; }

        dynamic this[string configElementName] { get; }
    }
}