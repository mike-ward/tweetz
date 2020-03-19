using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Models;

namespace tweetz.core.ViewModels
{
    public class SettingsControlViewModel : NotifyPropertyChanged
    {
        private string? updateAvailableToolTip;

        public ISettings Settings { get; }
        public ISystemState SystemState { get; }
        public string? UpdateAvailableToolTip { get => updateAvailableToolTip; set => SetProperty(ref updateAvailableToolTip, value); }

        public SettingsControlViewModel(ISettings settings, ISystemState systemState, ICheckForUpdates checkForUpdates)
        {
            Settings = settings;
            SystemState = systemState;

            if (checkForUpdates != null)
            {
                checkForUpdates.PropertyChanged += (s, args) =>
                    UpdateAvailableToolTip = checkForUpdates.Version.Trim() != VersionInfo.Version
                        ? (string)Application.Current.FindResource("new-version-available")
                        : null;
            }
        }

        public void SaveSettings()
        {
            Settings.Save();
        }
    }
}