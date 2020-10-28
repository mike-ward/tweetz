using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Infrastructure.Extensions;

namespace tweetz.core.ViewModels
{
    public class TitleBarControlViewModel : NotifyPropertyChanged
    {
        public ISettings Settings { get; set; }

        public TitleBarControlViewModel(ISettings settings)
        {
            title = string.Empty;
            Settings = settings;
            Settings.PropertyChanged += Settings_PropertyChanged;
        }

        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs? e)
        {
            if (e is null) { return; }

            if (e.PropertyName.IsEqualTo(nameof(Settings.ScreenName)) ||
                e.PropertyName.IsEqualTo(nameof(Settings.HideScreenName)))
            {
                var appTitle = (string)Application.Current.FindResource("title");

                Title = Settings.HideScreenName || string.IsNullOrWhiteSpace(Settings.ScreenName)
                    ? appTitle
                    : $"{appTitle} – {Settings.ScreenName}";
            }
        }
    }
}