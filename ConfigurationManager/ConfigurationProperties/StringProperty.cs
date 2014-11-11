namespace DynamicConfigurationManager.ConfigurationProperties
{
    public class StringProperty:ConfigurationProperty<string>
    {
        public StringProperty(string name,string description, string defaultValue = "")
            : base(name, description, defaultValue)
        {
        }

    
    }
}