using DynamicConfigurationManager.Interfaces;

namespace ConfigurationEditor.ViewModels
{
    public  class ErrorConfigurationNodeViewModel:ConfigurationNodeViewModel
    {
        public ErrorConfigurationNodeViewModel(IConfigurationNode node):base(node)
        {
        }
    }
}