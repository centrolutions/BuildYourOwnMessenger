using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace BuildYourOwnMessenger.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get {  return Ioc.Default.GetService<MainViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get {  return Ioc.Default.GetService<SettingsViewModel>(); }
        }
    }
}
