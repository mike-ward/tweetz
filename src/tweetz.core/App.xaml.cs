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
            AppDomain.CurrentDomain.UnhandledException +=
                (s, args) => ShowCrashReport((Exception)args.ExceptionObject);

            var settings = BootStrapper.GetService<ISettings>();
            settings.PropertyChanged += SettingsChanged;

            var mainWindow = BootStrapper.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Theme))
            {
                if (sender is Settings settings) ChangeTheme(settings.Theme);
            }
        }

        private static void ChangeTheme(string theme)
        {
            var uri = new Uri($"Infrastructure/Resources/{theme}.xaml", UriKind.Relative);
            var colorDictionary = (ResourceDictionary)LoadComponent(uri);
            Current.Resources.MergedDictionaries.Insert(1, colorDictionary);
            Current.Resources.MergedDictionaries.RemoveAt(0);
        }

        private static void ShowCrashReport(Exception exception)
        {
            var crashReport = new CrashReport(exception);
            MessageBox.Show(crashReport.Report);
            Environment.Exit(1);
        }
    }
}