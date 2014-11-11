using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConfigurationManager.Interfaces
{
    public interface IConfigurationGroup : IConfigurationElement
    {
        [JsonProperty]
        List<IConfigurationElement> ConfigurationElements { get; set; }

        dynamic this[string configElementName] { get; }
    }
}