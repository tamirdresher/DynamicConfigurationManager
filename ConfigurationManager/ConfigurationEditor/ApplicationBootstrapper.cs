using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using ConfigurationEditor.ViewModels;
using Ninject;

namespace ConfigurationEditor
{
    public class ApplicationBootstrapper : Caliburn.Micro.BootstrapperBase
    {
       
        public IKernel Container { get; set; }

        public ApplicationBootstrapper()
        {
            Container = new StandardKernel();

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            Dictionary<string, object> settings = new Dictionary<string, object>();
            settings.Add("Height", 500);
            settings.Add("Width", 1000);
            settings.Add("SizeToContent", SizeToContent.Manual);
            DisplayRootViewFor<ApplicationShellViewModel>(settings);
        }

        protected override void Configure()
        {
            base.Configure();
            Container.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            Container.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        }

      
        protected override void BuildUp(object instance)
        {
            Container.Inject(instance);
        }

        protected override object GetInstance(Type service, string key)
        {
            return Container.Get(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.GetAll(service);
        }


    }
}