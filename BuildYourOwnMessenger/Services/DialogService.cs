namespace BuildYourOwnMessenger.Services
{
    public class DialogService : IDialogService
    {
        public void OpenSettings()
        {
            var window = new SettingsWindow();
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }
    }
}
