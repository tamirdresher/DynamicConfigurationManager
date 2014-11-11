using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using ConfigurationEditor.ViewModels.Properties;
using DynamicConfigurationManager.ConfigurationProperties;
using DynamicConfigurationManager.Interfaces;

namespace ConfigurationEditor.ViewModels
{
    public class ConfigurationNodeViewModel:ConfigurationGroupViewModel//ConfigurationElementViewModel
    {
        public IConfigurationNode Node { get; set; }

        public ConfigurationNodeViewModel(IConfigurationNode node):base(node)
        {
            Node = node;
            //Children=new BindableCollection<ConfigurationPropertyViewModel>();
            CreatePropertyVm(node);
        }
        bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        private void CreatePropertyVm(IConfigurationNode node)
        {
            foreach (var prop in node.Properties)
            {
                ConfigurationPropertyViewModel configPropertyViewModel;
                if (IsSubclassOfRawGeneric(typeof(NumericProperty<>), prop.GetType()))
                {
                    configPropertyViewModel = new NumericPropertyViewModel(prop);
                }
                else if (prop is StringProperty)
                {
                    configPropertyViewModel = new StringPropertyViewModel(prop as StringProperty);
                }
                else if (IsSubclassOfRawGeneric(typeof(ListProperty<>), prop.GetType()))
                {
                    configPropertyViewModel = new ListPropertyViewModel(prop);
                }
                else
                {
                    configPropertyViewModel = new NotSupportedPropertyViewModel(prop);
                }
                AddChild(configPropertyViewModel);

            }

        }
        public void AddChild(ConfigurationPropertyViewModel child)
        {
            Children.Add(child);
            child.PropertyChanged += (o, args) => { NotifyOfPropertyChange(args.PropertyName); };
            child.ErrorsChanged += (o, args) => { RaiseErrorsChanged( new DataErrorsChangedEventArgs("Name")); };
        }
        public override string Name
        {
            get { return Node.Name; }
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