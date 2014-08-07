using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using ConfigurationEditor.ViewModels.Properties;
using ConfigurationManager;
using ConfigurationManager.ConfigurationProperties;
using Microsoft.Win32;
using Ninject;
using Ninject.Extensions.Conventions;

namespace ConfigurationEditor.ViewModels
{
    public class ApplicationShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private IConfigurationManager _configurationManager;
        private bool _isBusy;
        private BindableCollection<ConfigurationElementViewModel> _configuration;
        private IDictionary<IConfigurationProperty, Screen> _propToVm = new Dictionary<IConfigurationProperty, Screen>();
        private string _configFilePath;
        private ConfigurationGroupViewModel _configurationRoot;
        private const string Plugins = "Plugins";
        public ApplicationShellViewModel()
        {
            DisplayName = "MTD Confiuguration Editor";
            EmptyScreen = new EmptyScreenViewModel();

        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public Screen EmptyScreen { get; set; }

        public async void SaveConfig()
        {
            if (!string.IsNullOrEmpty(_configFilePath) && 
                ConfigurationRoot != null &&
                ConfigurationRoot.IsValid)
            {
                IsBusy = true;
                await Task.Run(() => _configurationManager.Save(_configFilePath));
                IsBusy = false;
            }
        }


        public async void OpenConfig()
        {
            var fileDlg = new OpenFileDialog()
            {
                Filter = "MTD Exe file (*.exe)|*.exe"
            };
            if (fileDlg.ShowDialog() == true)
            {
                IsBusy = true;
                try
                {
                    var directoryName = Path.GetDirectoryName(fileDlg.FileName);
                    var pluginsDir = Path.Combine(directoryName, Plugins);
                    var standardKernel = new StandardKernel();
                    await Task.Run(() =>
                        standardKernel.Bind(
                            scanner => scanner.From(GetAllDllEntries(pluginsDir).Select(Assembly.LoadFrom))
                                .SelectAllClasses()
                                .InheritedFrom<ConfigurationNode>()
                                .BindAllInterfaces()));

                    var configurationNodes = standardKernel.GetAll<IConfigurationNode>();
                    if (!configurationNodes.Any())
                    {
                        await Execute.OnUIThreadAsync(() => Xceed.Wpf.Toolkit.MessageBox.Show(
                            "There are no ConfigurationNodes found under your selected MTD folder"));

                        return;
                    }
                    await Task.Run(() =>
                    {
                        _configurationManager = standardKernel.Get<ConfigurationManager.ConfigurationManager>();

                        _configFilePath = Path.Combine(directoryName, _configurationManager.DefaultConfigurationFileName);

                        var mtdVersion = AssemblyName.GetAssemblyName(fileDlg.FileName).Version;
                        _configurationManager.OpenConfiguration(_configFilePath, mtdVersion);
                        _propToVm.Clear();
                        Items.Clear();
                        Items.Add(EmptyScreen);
                        var configRoot = new ConfigurationGroupViewModel(new ConfigurationGroup("Root"));
                        CreateVms(configRoot, _configurationManager.AppConfiguration.ConfigurationElements);
                        ConfigurationRoot = configRoot;

                        ActivateItem(EmptyScreen);
                    });
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public ConfigurationGroupViewModel ConfigurationRoot
        {
            get { return _configurationRoot; }
            set
            {
                if (Equals(value, _configurationRoot)) return;
                _configurationRoot = value;
                NotifyOfPropertyChange(() => ConfigurationRoot);
            }
        }

        private void CreateVms(ConfigurationGroupViewModel parent, List<IConfigurationElement> configurationElements)
        {
            if (configurationElements == null)
            {
                return;
            }
            foreach (var configurationElement in configurationElements)
            {
                var node = configurationElement as IConfigurationNode;
                if (node != null)
                {
                    ConfigurationNodeViewModel nodeVm;
                    if (node is ErrorConfigurationNode)
                    {
                        nodeVm = new ErrorConfigurationNodeViewModel(node);
                        Items.Add(nodeVm);
                    }
                    else
                    {
                        nodeVm = new ConfigurationNodeViewModel(node);
                        foreach (var propVm in nodeVm.Children)
                        {
                            AddPropVm(propVm.ConfigProp, propVm);
                        }

                    }
                    parent.AddChild(nodeVm);
                }
                var grp = configurationElement as ConfigurationGroup;
                if (grp != null)
                {
                    var configurationGroupViewModel = new ConfigurationGroupViewModel(grp);
                    parent.AddChild(configurationGroupViewModel);
                    CreateVms(configurationGroupViewModel, grp.ConfigurationElements);
                }

            }
        }
        private void AddPropVm(IConfigurationProperty prop, Screen vm)
        {
            _propToVm[prop] = vm;
            Items.Add(vm);
        }
        private static string[] GetAllDllEntries(string pluginsDir)
        {
            var modulesSearchPattern = "*.dll";

            //var pluginsDir = Directory.GetCurrentDirectory() + "\\" + Plugins;
            string[] pluginsFileEntries = Directory.Exists(pluginsDir)
                ? Directory.GetFiles(pluginsDir, modulesSearchPattern, SearchOption.AllDirectories)
                : new string[] { };
            return pluginsFileEntries.ToArray();
        }
        public void SelectedItemChanged(RoutedPropertyChangedEventArgs<object> selectedItemArgs)
        {
            var configPropVm = selectedItemArgs.NewValue as ConfigurationPropertyViewModel;
            if (configPropVm != null)
            {
                ActivateItem(_propToVm[configPropVm.ConfigProp]);
            }
            else if (selectedItemArgs.NewValue is ErrorConfigurationNodeViewModel)
            {
                ActivateItem(selectedItemArgs.NewValue as Screen);
            }
            else
            {
                ActivateItem(EmptyScreen);

            }
        }
    }
}