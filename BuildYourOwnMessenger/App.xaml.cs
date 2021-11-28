using BuildYourOwnMessenger.Messages;
using BuildYourOwnMessenger.Properties;
using BuildYourOwnMessenger.Services;
using BuildYourOwnMessenger.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Windows;

namespace BuildYourOwnMessenger
{

    public partial class App : Application
    {
        public ViewModelLocator ViewModelLocator {  get { return (ViewModelLocator)Current.TryFindResource("ViewModelLocator"); } }

        public App()
        {
            SetupDependencyInjection();
            SetupSettings();
        }

        private void SetupSettings()
        {
            var messenger = Ioc.Default.GetService<IMessenger>();
            messenger.Send(new OnlineStatusChangedMessage(Settings.Default.WorkOnline));
        }

        private void SetupDependencyInjection()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddSingleton<IMessenger, Messenger>()
                    .AddSingleton<IOrderCheckerService, OrderCheckerService>()
                    .AddViewModels<ViewModelBase>()
                    .BuildServiceProvider()
                ) ;
            ;
        }
    }
}
