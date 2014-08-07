using Caliburn.Micro;
using ConfigurationManager;

namespace ConfigurationEditor.ViewModels.Properties
{
    public class NotSupportedPropertyViewModel : ConfigurationPropertyViewModel<NotSupportedPropertyViewModel>
    {
        public IConfigurationProperty Prop { get; set; }

        public NotSupportedPropertyViewModel(IConfigurationProperty prop)
        {
            Prop = prop;
        }

        public override string Name
        {
            get { return Prop.Name; }
        }

        public override IConfigurationProperty ConfigProp
        {
            get { return Prop; }
        }
    }
}