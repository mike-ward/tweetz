using tweetz.core.Interfaces;

namespace tweetz.core.ViewModels
{
    public class TabBarControlViewModel : NotifyPropertyChanged
    {
        private double tabWidth;
        private bool showComposeControl;

        public double TabWidth { get => tabWidth; set => SetProperty(ref tabWidth, value); }
        public bool ShowComposeControl { get => showComposeControl; set => SetProperty(ref showComposeControl, value); }

        public ISettings Settings { get; }

        public TabBarControlViewModel(ISettings settings)
        {
            Settings = settings;
        }
    }
}