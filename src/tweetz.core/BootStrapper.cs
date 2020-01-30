using System;
using Microsoft.Extensions.DependencyInjection;
using tweetz.core.Commands;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public static class BootStrapper
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
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<VersionInfo>();

            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IWindowInteropService, WindowInteropService>();
            services.AddSingleton<ITwitterService, TwitterService>();
            services.AddSingleton<IOpenUrlService, OpenUrlService>();
            services.AddSingleton<IImageViewerService, ImageViewerService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<ICheckForUpdates, CheckForUpdates>();
            services.AddSingleton<ISystemState, SystemState>();

            services.AddSingleton<GetPinControlViewModel>();
            services.AddSingleton<TabBarControlViewModel>();

            services.AddSingleton<HomeTimelineControlViewModel>();
            services.AddSingleton<FavoritesTimelineControlViewModel>();
            services.AddSingleton<SearchControlViewModel>();
            services.AddSingleton<SettingsControlViewModel>();
            services.AddSingleton<ComposeControlViewModel>();
            services.AddSingleton<UserProfileBlockViewModel>();

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

        public static object GetService(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return ServiceProvider.GetService(type) ?? throw new NotImplementedException(type.Name);
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }
}