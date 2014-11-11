using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ConfigurationEditor.Validation;
using DynamicConfigurationManager.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace ConfigurationEditor.ViewModels.Properties
{
    public abstract class ConfigurationPropertyViewModel : ConfigurationElementViewModel
    { 
        public abstract IConfigurationProperty ConfigProp { get; }
    }
    public abstract class ConfigurationPropertyViewModel<TPropertyVm> : ConfigurationPropertyViewModel where TPropertyVm : ConfigurationPropertyViewModel<TPropertyVm>
    {
        public ConfigurationPropertyViewModel()
        {
            ConfigurationPropertyValidator = new ConfigurationPropertyValidator<TPropertyVm>();
        }
        private IList<ValidationFailure> _failures = new List<ValidationFailure>(); 
        public AbstractValidator<TPropertyVm> ConfigurationPropertyValidator { get; set; }
        public override bool IsValid
        {
            get { return SelfValidate().IsValid; }
        }

        private ValidationResult SelfValidate()
        {
            return ConfigurationPropertyValidator.Validate(this as TPropertyVm);
        }

        public override string this[string columnName]
        {
            get
            {
                Validate();
                var errors = _failures;
                return CreateErrorsMsg(errors);
            }
        }

        public override string Error
        {
            get
            {
                Validate();
                var errors = _failures;
                return CreateErrorsMsg(errors);
            }
        }

        private static string CreateErrorsMsg(IEnumerable<ValidationFailure> errors)
        {
            if (errors!=null&&errors.Any())
            {
                var errorsMsg = string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage).ToArray());
                return errorsMsg;
            }
            return string.Empty;
        }

        private void Validate()
        {
            var errorsBefore = _failures.Select(f => f.PropertyName);
            var results = SelfValidate();
            _failures = results.Errors;

            // Here I'm just raising a single ErrorsChanged event, with no propety name.
            // An alternative would be to iterate through results.Errors and raise the event once for each property where there's a validation failure by doing:
            foreach (var error in results.Errors)
            {
                RaiseErrorsChanged( new DataErrorsChangedEventArgs(error.PropertyName));
            }
            foreach (var propNameWithErrorBefore in errorsBefore.Except(_failures.Select(f=>f.PropertyName )))
            {
                RaiseErrorsChanged(new DataErrorsChangedEventArgs(propNameWithErrorBefore));
                
            }
            //RaiseErrorsChanged( new DataErrorsChangedEventArgs(null));
           
        }

        public override IEnumerable GetErrors(string propertyName)
        {
            return _failures;
        }

        public override bool HasErrors
        {
            get { return _failures.Count > 0; }
        }

    }
}