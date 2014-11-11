using Caliburn.Micro;
using ConfigurationManager;
using ConfigurationManager.ConfigurationProperties;
using ConfigurationManager.Interfaces;

namespace ConfigurationEditor.ViewModels.Properties
{
    public class StringPropertyViewModel : ConfigurationPropertyViewModel<StringPropertyViewModel>
    {
        private readonly StringProperty _prop;

        public StringPropertyViewModel(StringProperty prop)
        {
            _prop = prop;
        }

        public string Value
        {
            get { return Prop.Value; }
            set
            {
                Prop.Value = value;
                NotifyOfPropertyChange(() => Value);
                NotifyOfPropertyChange(() => IsValid);

            }
        }

        public StringProperty Prop
        {
            get { return _prop; }
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