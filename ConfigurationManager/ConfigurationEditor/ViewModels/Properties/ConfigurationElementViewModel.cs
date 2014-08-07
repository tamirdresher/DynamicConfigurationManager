using System;
using System.Collections;
using System.ComponentModel;
using Caliburn.Micro;

namespace ConfigurationEditor.ViewModels.Properties
{
    public abstract class ConfigurationElementViewModel : Screen, IDataErrorInfo,INotifyDataErrorInfo {
        public abstract string Name { get; }

        public abstract bool IsValid { get; }
        public abstract string this[string columnName] { get; }
        public abstract string Error { get; }
        public abstract IEnumerable GetErrors(string propertyName);
        public abstract bool HasErrors { get; }

        public void RaiseErrorsChanged(DataErrorsChangedEventArgs args)
        {
            ErrorsChanged(this, args);
        }
        public  event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged= delegate { };
    }
}