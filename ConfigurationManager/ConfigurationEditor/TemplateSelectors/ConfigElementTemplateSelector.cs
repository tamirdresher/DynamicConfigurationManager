using System.Windows;
using System.Windows.Controls;
using ConfigurationEditor.ViewModels;
using ConfigurationEditor.ViewModels.Properties;

namespace ConfigurationEditor.TemplateSelectors
{
    public class ConfigElementTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;

            if (item is ConfigurationGroupViewModel)
            {
                return element.TryFindResource("ConfigGroupDataTemplate") as DataTemplate;
            }
            if (item is ErrorConfigurationNodeViewModel)
            {
                return element.TryFindResource("ErrorNodeDataTemplate") as DataTemplate;
            }
            if (item is ConfigurationNodeViewModel)
            {
                return element.TryFindResource("ConfigNodeDataTemplate") as DataTemplate;
            }
            if (item is ConfigurationPropertyViewModel)
            {
                return element.TryFindResource("ConfigPropertyDataTemplate") as DataTemplate;

            }
            return null;
        }
    }

}