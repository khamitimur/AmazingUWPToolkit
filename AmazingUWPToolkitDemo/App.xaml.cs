using AmazingUWPToolkit.ApplicatonView;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AmazingUWPToolkitDemo.ViewModels;
using AmazingUWPToolkitDemo.Views;

namespace AmazingUWPToolkitDemo
{
    public sealed partial class App
    {
        #region Fields

        private WinRTContainer container;

        #endregion

        #region Constructor

        public App()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides of CaliburnApplication

        protected override void Configure()
        {
            container = new WinRTContainer();
            container.RegisterWinRTServices();

            container.RegisterSingleton(typeof(IApplicationViewData), null, typeof(ApplicationViewData));
            container.RegisterSingleton(typeof(IApplicationViewHelper), null, typeof(ApplicationViewHelper));

            container.Singleton<MainViewModel>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);

            base.PrepareViewFirst(rootFrame);
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            if (e.Kind == ActivationKind.ToastNotification)
            {
                if (Window.Current.Content == null)
                {
                    DisplayRootView<MainView>();
                }
            }

            base.OnActivated(e);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Window.Current.Content == null)
            {
                DisplayRootView<MainView>(e.SplashScreen);
            }
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

        #endregion
    }
}