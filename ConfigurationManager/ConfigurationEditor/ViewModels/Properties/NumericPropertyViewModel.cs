using System;
using System.Collections;
using Caliburn.Micro;
using DynamicConfigurationManager.ConfigurationProperties;
using DynamicConfigurationManager.Interfaces;
using FluentValidation;

namespace ConfigurationEditor.ViewModels.Properties
{
    class NumericPropertyViewModel<T> : ConfigurationPropertyViewModel<NumericPropertyViewModel<T>> where T : struct, IComparable
    {
        private readonly NumericProperty<T> _prop;

        public NumericPropertyViewModel(NumericProperty<T> prop)
        {
            _prop = prop;
            ConfigurationPropertyValidator.RuleFor(vm => vm.Value)
                .Must((val) =>
                {
                    if (Prop.Maximum.HasValue && Prop.Maximum.Value.CompareTo(val) < 0)
                    {
                        return false;

                    }
                    if (Prop.Minimum.HasValue && Prop.Minimum.Value.CompareTo(val) > 0)
                    {
                        return false;
                    }
                    return true;
                })
                .WithMessage(Prop.Name + " should be less than " + Prop.Maximum.Value)
                .Must((val) =>
                {

                    if (Prop.Minimum.HasValue && Prop.Minimum.Value.CompareTo(val) > 0)
                    {
                        return false;
                    }
                    return true;
                })
                .WithMessage(Prop.Name + " should be greater than " + Prop.Minimum.Value);
        }

        public T Value
        {
            get { return Prop.Value; }
            set
            {
                Prop.Value = value;
                NotifyOfPropertyChange(() => Value);
                NotifyOfPropertyChange(() => IsValid);
            }
        }

        public NumericProperty<T> Prop
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

    class NumericPropertyViewModel : ConfigurationPropertyViewModel
    {
        public NumericPropertyViewModel(IConfigurationProperty prop)
        {
            InnerViewModel = CreateNumericVM(prop);
            InnerViewModel.ErrorsChanged += (s, e) => RaiseErrorsChanged(e);
            InnerViewModel.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        public ConfigurationPropertyViewModel InnerViewModel { get; set; }

        private ConfigurationPropertyViewModel CreateNumericVM(IConfigurationProperty prop)
        {
            var numericVmType = typeof(NumericPropertyViewModel<>).MakeGenericType(prop.GetType().GenericTypeArguments[0]);
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