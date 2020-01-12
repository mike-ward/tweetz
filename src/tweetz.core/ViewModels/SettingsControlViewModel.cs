using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;

namespace tweetz.core.ViewModels
{
    public class SettingsControlViewModel : NotifyPropertyChanged
    {
        private string? updateAvailableToolTip;

        public ISettings Settings { get; }
        public VersionInfo VersionInfo { get; }
        public ISystemState SystemState { get; }
        public string? UpdateAvailableToolTip { get => updateAvailableToolTip; set => SetProperty(ref updateAvailableToolTip, value); }

        public SettingsControlViewModel(ISettings settings, VersionInfo versionInfo, ISystemState systemState, ICheckForUpdates checkForUpdates)
        {
            Settings = settings;
            VersionInfo = versionInfo;
            SystemState = systemState;

            checkForUpdates.PropertyChanged += (s, args) => UpdateAvailableToolTip =
                checkForUpdates.Version.Trim() != VersionInfo.Version
                ? LanguageService.Instance.Lookup("new-version-available")
                : null;
        }

        public void SaveSettings()
        {
            Settings.Save();
        }
    }
}