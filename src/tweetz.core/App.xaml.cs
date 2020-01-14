using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using tweetz.core.Commands;
using tweetz.core.Infrastructure;
using tweetz.core.Infrastructure.ExceptionHandling;
using tweetz.core.Models;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<VersionInfo>();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IWindowInteropService, WindowInteropService>();
            services.AddSingleton<ITwitterService, TwitterService>();
            services.AddSingleton<IOpenUrlService, OpenUrlService>();
            services.AddSingleton<IImageViewerService, ImageViewerService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<ICheckForUpdates, CheckForUpdates>();

            services.AddSingleton<MainWindow, MainWindow>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<ISystemState, SystemState>();

            services.AddSingleton<GetPinControlViewModel, GetPinControlViewModel>();
            services.AddSingleton<TabBarControlViewModel, TabBarControlViewModel>();

            services.AddSingleton<HomeTimelineControlViewModel, HomeTimelineControlViewModel>();
            services.AddSingleton<FavoritesTimelineControlViewModel, FavoritesTimelineControlViewModel>();
            services.AddSingleton<SearchControlViewModel, SearchControlViewModel>();
            services.AddSingleton<SettingsControlViewModel, SettingsControlViewModel>();
            services.AddSingleton<ComposeControlViewModel, ComposeControlViewModel>();
            services.AddTransient<UserProfileBlockViewModel, UserProfileBlockViewModel>();

            services.AddSingleton<ICommandBinding, GoToSearchCommand>();
            services.AddSingleton<ICommandBinding, ImageViewerCommand>();
            services.AddSingleton<ICommandBinding, OpenLinkCommand>();
            services.AddSingleton<ICommandBinding, ScrollToHomeCommand>();
            services.AddSingleton<ICommandBinding, SignOutCommand>();
            services.AddSingleton<ICommandBinding, SearchCommand>();
            services.AddSingleton<ICommandBinding, ToggleRetweetCommand>();
            services.AddSingleton<ICommandBinding, ToggleFavoritesCommand>();
            services.AddSingleton<ICommandBinding, ToggleFollowCommand>();
            services.AddSingleton<ICommandBinding, ToggleShowComposeCommand>();
            services.AddSingleton<ICommandBinding, ToggleSpellCheckerCommand>();
            services.AddSingleton<ICommandBinding, UpdateStatusCommand>();
            services.AddSingleton<ICommandBinding, ReplyToCommand>();
            services.AddSingleton<ICommandBinding, QuoteTweetCommand>();
            services.AddSingleton<ICommandBinding, ShowTwitterStatusCommand>();
            services.AddSingleton<ICommandBinding, AddImageCommand>();
            services.AddSingleton<ICommandBinding, RemoveImageCommand>();
            services.AddSingleton<ICommandBinding, GetMentionsCommand>();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (s, args) => ShowCrashReport((Exception)args.ExceptionObject);

            var settings = ServiceProvider.GetService<ISettings>();
            settings.PropertyChanged += SettingsChanged;

            var mainWindow = ServiceProvider.GetService<MainWindow>();
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