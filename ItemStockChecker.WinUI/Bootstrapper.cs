﻿using Caliburn.Micro;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ItemStockChecker.WinUI.ViewModels;
using Logger;

namespace ItemStockChecker.WinUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public Bootstrapper()
        {
            LogManager.GetLog = type => new Log4netLogger(type);
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            container.Instance(container);

            container.Singleton<IWindowManager, WindowManager>();

            container.PerRequest<ShellViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
