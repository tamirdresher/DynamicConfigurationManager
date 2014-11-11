using ConfigurationManager;
using ConfigurationManager.Interfaces;

namespace ConfigurationEditor.ViewModels
{
    public  class ErrorConfigurationNodeViewModel:ConfigurationNodeViewModel
    {
        public ErrorConfigurationNodeViewModel(IConfigurationNode node):base(node)
        {
        }
    }
}