using System;
using System.Collections;
using System.Collections.Generic;
using ConfigurationManager;
using ConfigurationManager.ConfigurationProperties;

namespace ConfigurationEditor.ViewModels.Properties
{
    public class ListPropertyViewModel<TItem> : ConfigurationPropertyViewModel<ListPropertyViewModel<TItem>> 
    {
        private readonly ListProperty<TItem> _property;

        public ListPropertyViewModel(ListProperty<TItem> property)
        {
            _property = property;
        }

        public IEnumerable<TItem> Values { get { return _property.ListItems; } }
        public override string Name
        {
            get { return _property.Name; }
        }

        public override IConfigurationProperty ConfigProp
        {
            get { return _property; }
        }
    }

    class ListPropertyViewModel : ConfigurationPropertyViewModel
    {
        public ListPropertyViewModel(IConfigurationProperty prop)
        {
            InnerViewModel = CreateEnumVM(prop);
            InnerViewModel.ErrorsChanged += (s, e) => RaiseErrorsChanged(e);
            InnerViewModel.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        public ConfigurationPropertyViewModel InnerViewModel { get; set; }

        private ConfigurationPropertyViewModel CreateEnumVM(IConfigurationProperty prop)
        {
            var numericVmType = typeof(ListPropertyViewModel<>).MakeGenericType(prop.GetType().GenericTypeArguments[0]);
            return Activator.CreateInstance(numericVmType, prop) as ConfigurationPropertyViewModel;
        }

        public override string this[string columnName]
        {
            get { return InnerViewModel[columnName]; }
        }

        public override string Error
        {
            get { return InnerViewModel.Error; }
        }

        public override IEnumerable GetErrors(string propertyName)
        {
            return InnerViewModel.GetErrors(propertyName);
        }

        public override bool HasErrors
        {
            get { return InnerViewModel.HasErrors; }
        }

        public override string Name
        {
            get { return InnerViewModel.Name; }
        }

        public override bool IsValid
        {
            get { return InnerViewModel.IsValid; }
        }

        public override IConfigurationProperty ConfigProp
        {
            get { return InnerViewModel.ConfigProp; }
        }
    }
}