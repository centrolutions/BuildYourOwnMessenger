using BuildYourOwnMessenger.Services;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace BuildYourOwnMessenger.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _DialogService;
        private readonly IOrderCheckerService _OrderChecker;

        string _OnlineStatus;
        public string OnlineStatus
        {
            get { return _OnlineStatus; }
            set { SetProperty(ref _OnlineStatus, value); }
        }

        int _OrderCount;
        public int OrderCount
        {
            get { return _OrderCount; }
            set { SetProperty(ref _OrderCount, value); }
        }

        public ICommand OpenSettingsCommand { get; }

        public MainViewModel(IDialogService dialogService, IOrderCheckerService orderChecker)
        {
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            _DialogService = dialogService;
            _OrderChecker = orderChecker;
            _OrderChecker.OrderCountChanged += OrderChecker_OrderCountChanged;
            SetOnlineStatus(true);
        }

        private void OrderChecker_OrderCountChanged(object? sender, System.EventArgs e)
        {
            OrderCount = _OrderChecker.OrderCount;
        }

        private void OpenSettings()
        {
            _DialogService.OpenSettings();
        }

        private void SetOnlineStatus(bool isOnline)
        {
            OnlineStatus = isOnline ? "You are online" : "You are offline";
            if (isOnline)
                _OrderChecker.StartCheckingOrders();
            else
                _OrderChecker.StopCheckingOrders();
        }
    }
}
