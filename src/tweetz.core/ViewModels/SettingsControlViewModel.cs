﻿using tweetz.core.Interfaces;
using tweetz.core.Models;

namespace tweetz.core.ViewModels
{
    public class SettingsControlViewModel : NotifyPropertyChanged
    {
        private string? updateAvailableToolTip;

        public ISettings    Settings    { get; }
        public ISystemState SystemState { get; }

        public string? UpdateAvailableToolTip
        {
            get => updateAvailableToolTip;
            set => SetProperty(ref updateAvailableToolTip, value);
        }

        public SettingsControlViewModel(ISettings settings, ISystemState systemState, ICheckForUpdates checkForUpdates)
        {
            Settings    = settings;
            SystemState = systemState;

            checkForUpdates.PropertyChanged += (s, args) =>
                UpdateAvailableToolTip = string.CompareOrdinal(checkForUpdates.Version.Trim(), VersionInfo.Version) != 0
                    ? App.GetString("new-version-available")
                    : null;
        }

        public void SaveSettings()
        {
            Settings.Save();
        }
    }
}