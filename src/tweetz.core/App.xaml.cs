using System;
using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Infrastructure.ExceptionHandling;
using tweetz.core.Models;

namespace tweetz.core
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += ShowCrashReport;
            BootStrapper.GetService<ISettings>().PropertyChanged += SettingsThemeChanged;
            BootStrapper.GetService<MainWindow>().Show();
        }

        private static void ShowCrashReport(object? sender, UnhandledExceptionEventArgs args)
        {
            var crashReport = new CrashReport((Exception)args.ExceptionObject);
            MessageBox.Show(crashReport.Report);
            Environment.Exit(1);
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