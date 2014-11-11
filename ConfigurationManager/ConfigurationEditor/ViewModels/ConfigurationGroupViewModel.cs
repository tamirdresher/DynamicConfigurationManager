using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using ConfigurationEditor.ViewModels.Properties;
using ConfigurationManager;
using ConfigurationManager.Interfaces;

namespace ConfigurationEditor.ViewModels
{
    public class ConfigurationGroupViewModel:ConfigurationElementViewModel
    {
        public IConfigurationGroup ConfigGroup { get; set; }

        public ConfigurationGroupViewModel(IConfigurationGroup configGroup)
        {
            ConfigGroup = configGroup;
            Children=new BindableCollection<ConfigurationElementViewModel>();
        }

        public void AddChild(ConfigurationElementViewModel child)
        {
            Children.Add(child);
            child.ErrorsChanged += (o, args) => { RaiseErrorsChanged(args); };
            child.PropertyChanged += (o, args) => { NotifyOfPropertyChange(args.PropertyName); };
        }
        public BindableCollection<ConfigurationElementViewModel> Children { get; set; }
        public override string Name
        {
            get { return ConfigGroup.Name; }
        }

        public override bool IsValid
        {
            get
            {
                bool isValid = true;
                foreach (var child in Children)
                {
                    isValid &= child.IsValid;
                }
                return isValid;
            }
        }

        public override string this[string columnName]
        {
            get { return ""; }
        }

        public override string Error
        {
            get { return string.Concat(Children.Select(c => c.Error)); }
        }

        public override IEnumerable GetErrors(string propertyName)
        {
            return Children.SelectMany(c => c.GetErrors(propertyName).OfType<object>());
        }

        public override bool HasErrors
        {
            get { return !IsValid; }
        }

    }
}