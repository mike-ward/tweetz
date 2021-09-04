using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using Jab;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;

namespace tweetz.core
{
    public partial class App : Application
    {
        public static MyServiceProvider ServiceProvider { get; } = new();
        public static HttpClient        MyHttpClient    => twitter.core.Services.OAuthApiRequest.MyHttpClient;

        static App()
        {
            if (!DesktopWindowManager.DesktopWindowManager.IsDwmEnabled())
            {
                MessageBox.Show("Desktop Window Manager service must be enabled to run", "Tweetz");
                Environment.Exit(1);
            }
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (!Debugger.IsAttached) AppCenter.Start("14eb5ed2-3dc9-4cb3-8ad5-a630a9d90407", typeof(Analytics), typeof(Crashes));
            ServiceProvider.GetService<ISettings>().PropertyChanged += SettingsThemeChanged;
            ServiceProvider.GetService<MainWindow>().Show();
        }

        private void SettingsThemeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(Settings.Theme)) && sender is Settings settings)
            {
                var uri             = new Uri($"Resources/{settings.Theme}.xaml", UriKind.Relative);
                var colorDictionary = (ResourceDictionary)LoadComponent(uri);
                Resources.MergedDictionaries.Add(colorDictionary);
            }
        }

        public static string GetString(string resourceName)
        {
            return Current.FindResource(resourceName) as string ?? throw new InvalidOperationException();
        }
    }
}