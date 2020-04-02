using System;
using Microsoft.Extensions.DependencyInjection;
using tweetz.core.Commands;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    internal static class BootStrapper
    {
        private static ServiceProvider ServiceProvider { get; }

        static BootStrapper()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Top level controls
            services.AddSingleton<MainWindow>();

            // View models
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<GetPinControlViewModel>();
            services.AddSingleton<TabBarControlViewModel>();
            services.AddSingleton<HomeTimelineControlViewModel>();
            services.AddSingleton<FavoritesTimelineControlViewModel>();
            services.AddSingleton<SearchControlViewModel>();
            services.AddSingleton<SettingsControlViewModel>();
            services.AddSingleton<ComposeControlViewModel>();
            services.AddSingleton<UserProfileBlockViewModel>();
            services.AddSingleton<MediaViewerBlockViewModel>();

            // Commands
            services.AddSingleton<ICommandBinding, AddImageCommand>();
            services.AddSingleton<ICommandBinding, DecreaseFontSizeCommand>();
            services.AddSingleton<ICommandBinding, GetMentionsCommand>();
            services.AddSingleton<ICommandBinding, GoToSearchCommand>();
            services.AddSingleton<ICommandBinding, ImageViewerCommand>();
            services.AddSingleton<ICommandBinding, IncreaseFontSizeCommand>();
            services.AddSingleton<ICommandBinding, OpenLinkCommand>();
            services.AddSingleton<ICommandBinding, QuoteTweetCommand>();
            services.AddSingleton<ICommandBinding, RemoveImageCommand>();
            services.AddSingleton<ICommandBinding, ReplyToCommand>();
            services.AddSingleton<ICommandBinding, ScrollToHomeCommand>();
            services.AddSingleton<ICommandBinding, SearchCommand>();
            services.AddSingleton<ICommandBinding, ShowTwitterStatusCommand>();
            services.AddSingleton<ICommandBinding, SignOutCommand>();
            services.AddSingleton<ICommandBinding, ToggleFavoritesCommand>();
            services.AddSingleton<ICommandBinding, ToggleFollowCommand>();
            services.AddSingleton<ICommandBinding, ToggleRetweetCommand>();
            services.AddSingleton<ICommandBinding, ToggleShowComposeCommand>();
            services.AddSingleton<ICommandBinding, ToggleSpellCheckerCommand>();
            services.AddSingleton<ICommandBinding, UpdateStatusCommand>();

            // Infrastructure
            services.AddSingleton<ICheckForUpdates, CheckForUpdates>();
            services.AddSingleton<IImageViewerService, ImageViewerService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IOpenUrlService, OpenUrlService>();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<ISystemState, SystemState>();
            services.AddSingleton<ISystemTrayIconService, SystemTrayIconService>();
            services.AddSingleton<ITwitterService, TwitterService>();
            services.AddSingleton<IWindowInteropService, WindowInteropService>();
        }

        public static object GetService(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return ServiceProvider.GetService(type) ?? throw new NotSupportedException(type.Name);
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}