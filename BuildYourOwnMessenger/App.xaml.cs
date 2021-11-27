using BuildYourOwnMessenger.Services;
using BuildYourOwnMessenger.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;

namespace BuildYourOwnMessenger
{

    public partial class App : Application
    {
        public ViewModelLocator ViewModelLocator {  get { return (ViewModelLocator)Current.TryFindResource("ViewModelLocator"); } }

        public App()
        {
            SetupDependencyInjection();
        }

        private void SetupDependencyInjection()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddSingleton<IOrderCheckerService, OrderCheckerService>()
                    .AddViewModels<ViewModelBase>()
                    .BuildServiceProvider()
                ) ;
            ;
        }
    }
}
