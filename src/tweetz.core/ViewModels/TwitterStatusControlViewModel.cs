using System.Collections.ObjectModel;
using tweetz.core.Infrastructure;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class TwitterStatusControlViewModel : NotifyPropertyChanged
    {
        private string? exceptionMessage;
        private bool isScrolled;

        public ISettings Settings { get; }
        public ObservableCollection<TwitterStatus> StatusCollection { get; protected set; } = new ObservableCollection<TwitterStatus>();
        public string? ExceptionMessage { get => exceptionMessage; protected set => SetProperty(ref exceptionMessage, value); }

        public TwitterStatusControlViewModel(ISettings settings)
        {
            Settings = settings;
            Settings.PropertyChanged += (s, args) => UpdatePausedMessage();
        }

        public bool IsScrolled
        {
            get => isScrolled;
            set
            {
                SetProperty(ref isScrolled, value);
                UpdatePausedMessage();
            }
        }

        private void UpdatePausedMessage()
        {
            ExceptionMessage = isScrolled && Settings.PauseWhenScrolled
                ? LanguageService.Instance.Lookup("paused-due-to-scroll-pos")
                : null;
        }
    }
}