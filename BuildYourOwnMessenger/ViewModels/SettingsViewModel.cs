using BuildYourOwnMessenger.Messages;
using BuildYourOwnMessenger.Services;
using System.Runtime.CompilerServices;

namespace BuildYourOwnMessenger.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IMessenger _Messenger;

        bool _IsOnline;
        public bool IsOnline
        {
            get { return _IsOnline; }
            set { SetProperty(ref _IsOnline, value); }
        }

        public SettingsViewModel(IMessenger messenger)
        {
            _Messenger = messenger;
            IsOnline = Properties.Settings.Default.WorkOnline;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(IsOnline))
            {
                Properties.Settings.Default.WorkOnline = IsOnline;
                Properties.Settings.Default.Save();
                _Messenger.Send(new OnlineStatusChangedMessage(IsOnline));
            }
        }
    }
}
