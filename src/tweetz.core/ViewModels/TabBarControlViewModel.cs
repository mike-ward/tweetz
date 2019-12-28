using tweetz.core.Infrastructure;

namespace tweetz.core.ViewModels
{
    public class TabBarControlViewModel : NotifyPropertyChanged
    {
        private double tabWidth;
        private bool showComposeControl;

        public ISettings Settings { get; }
        public HomeTimelineControlViewModel HomeTimelineControlViewModel { get; }
        public FavoritesTimelineControlViewModel FavoritesTimelineControlViewModel { get; }
        public SearchControlViewModel SearchControlViewModel { get; }
        public SettingsControlViewModel SettingsControlViewModel { get; }
        public ComposeControlViewModel ComposeControlViewModel { get; }

        public double TabWidth { get => tabWidth; set => SetProperty(ref tabWidth, value); }
        public bool ShowComposeControl { get => showComposeControl; set => SetProperty(ref showComposeControl, value); }

        public TabBarControlViewModel(
            ISettings settings,
            HomeTimelineControlViewModel homeTimelineControlViewModel,
            FavoritesTimelineControlViewModel favoritesTimelineControlViewModel,
            SearchControlViewModel searchControlViewModel,
            SettingsControlViewModel settingsControlViewModel,
            ComposeControlViewModel composeControlViewModel)
        {
            Settings = settings;
            HomeTimelineControlViewModel = homeTimelineControlViewModel;
            FavoritesTimelineControlViewModel = favoritesTimelineControlViewModel;
            SearchControlViewModel = searchControlViewModel;
            SettingsControlViewModel = settingsControlViewModel;
            ComposeControlViewModel = composeControlViewModel;
        }
    }
}