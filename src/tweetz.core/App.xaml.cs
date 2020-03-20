using System;
using System.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using tweetz.core.Infrastructure;
using tweetz.core.Models;

namespace tweetz.core
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            AppCenter.Start("14eb5ed2-3dc9-4cb3-8ad5-a630a9d90407", typeof(Analytics), typeof(Crashes));
            BootStrapper.GetService<ISettings>().PropertyChanged += SettingsThemeChanged;
            BootStrapper.GetService<MainWindow>().Show();
        }

        private void SettingsThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Theme) && sender is Settings settings)
            {
                var uri = new Uri($"Infrastructure/Resources/{settings.Theme}.xaml", UriKind.Relative);
                var colorDictionary = (ResourceDictionary)LoadComponent(uri);
                Resources.MergedDictionaries.Insert(1, colorDictionary);
                Resources.MergedDictionaries.RemoveAt(0);
            }
        }
    }
}