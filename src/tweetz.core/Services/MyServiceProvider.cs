using System;
using Microsoft.Extensions.DependencyInjection;
using tweetz.core.Commands;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.ViewModels;

// namespace tweetz.core.Services
// {
//     [ServiceProvider]
//     //
//     services.AddSingleton<MainWindow>();
//     services.AddSingleton<MainWindowViewModel>();
//     services.AddSingleton<MainViewModel>();
//     services.AddSingleton<GetPinControlViewModel>();
//     services.AddSingleton<TabBarControlViewModel>();
//     services.AddSingleton<HomeTimelineControlViewModel>();
//     services.AddSingleton<FavoritesTimelineControlViewModel>();
//     services.AddSingleton<SearchControlViewModel>();
//     services.AddSingleton<SettingsControlViewModel>();
//     services.AddSingleton<ComposeControlViewModel>();
//     services.AddSingleton<UserProfileBlockViewModel>();
//     services.AddSingleton<MediaViewerBlockViewModel>();
//     services.AddSingleton<TitleBarControlViewModel>();
//     services.AddSingleton<UserProfileTimelineViewModel>();
//     // 
//     services.AddSingleton<ICommandBinding), typeof(AddImageCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ClearHiddenImageCacheCommand>();
//     services.AddSingleton<ICommandBinding), typeof(DecreaseFontSizeCommand>();
//     services.AddSingleton<ICommandBinding), typeof(GetMentionsCommand>();
//     services.AddSingleton<ICommandBinding), typeof(GoToSearchCommand>();
//     services.AddSingleton<ICommandBinding), typeof(HideImageCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ImageViewerCommand>();
//     services.AddSingleton<ICommandBinding), typeof(IncreaseFontSizeCommand>();
//     services.AddSingleton<ICommandBinding), typeof(MinimizeCommand>();
//     services.AddSingleton<ICommandBinding), typeof(OpenLinkCommand>();
//     services.AddSingleton<ICommandBinding), typeof(QuoteTweetCommand>();
//     services.AddSingleton<ICommandBinding), typeof(RemoveImageCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ReplyToCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ScrollToHomeCommand>();
//     services.AddSingleton<ICommandBinding), typeof(SearchCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ShowTwitterStatusCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ShowUserProfileCommand>();
//     services.AddSingleton<ICommandBinding), typeof(SignOutCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ToggleFavoriteCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ToggleFollowCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ToggleRetweetCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ToggleShowComposeCommand>();
//     services.AddSingleton<ICommandBinding), typeof(ToggleSpellCheckerCommand>();
//     services.AddSingleton<ICommandBinding), typeof(TranslateCommand>();
//     services.AddSingleton<ICommandBinding), typeof(UpdateStatusCommand>();
//     //
//     services.AddSingleton<ICheckForUpdates), typeof(CheckForUpdates>();
//     services.AddSingleton<IImageViewerService), typeof(ImageViewerService>();
//     services.AddSingleton<IMessageBoxService), typeof(MessageBoxService>();
//     services.AddSingleton<IOpenUrlService), typeof(OpenUrlService>();
//     services.AddSingleton<ISettings), typeof(Settings>();
//     services.AddSingleton<ISystemState), typeof(SystemState>();
//     services.AddSingleton<ISystemTrayIconService), typeof(SystemTrayIconService>();
//     services.AddSingleton<ITwitterService), typeof(TwitterService>();
//     services.AddSingleton<IWindowInteropService), typeof(WindowInteropService>();
//     //
//     // partial class required so Jab generate code
//     //
//     public partial class MyServiceProvider { }
// }

namespace tweetz.core.Services
{
    public class MyServiceProvider
    {
        private readonly ServiceProvider serviceProvider;

        public MyServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Top level controls
            services.AddSingleton<MainWindow>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<GetPinControlViewModel>();
            services.AddSingleton<TabBarControlViewModel>();
            services.AddSingleton<HomeTimelineControlViewModel>();
            services.AddSingleton<FavoritesTimelineControlViewModel>();
            services.AddSingleton<SearchControlViewModel>();
            services.AddSingleton<SettingsControlViewModel>();
            services.AddSingleton<ComposeControlViewModel>();
            services.AddSingleton<UserProfileBlockViewModel>();
            services.AddSingleton<MediaViewerBlockViewModel>();
            services.AddSingleton<TitleBarControlViewModel>();
            services.AddSingleton<UserProfileTimelineViewModel>();
            // 
            services.AddSingleton<ICommandBinding, AddImageCommand>();
            services.AddSingleton<ICommandBinding, ClearHiddenImageCacheCommand>();
            services.AddSingleton<ICommandBinding, DecreaseFontSizeCommand>();
            services.AddSingleton<ICommandBinding, GetMentionsCommand>();
            services.AddSingleton<ICommandBinding, GoToSearchCommand>();
            services.AddSingleton<ICommandBinding, HideImageCommand>();
            services.AddSingleton<ICommandBinding, ImageViewerCommand>();
            services.AddSingleton<ICommandBinding, IncreaseFontSizeCommand>();
            services.AddSingleton<ICommandBinding, MinimizeCommand>();
            services.AddSingleton<ICommandBinding, OpenLinkCommand>();
            services.AddSingleton<ICommandBinding, QuoteTweetCommand>();
            services.AddSingleton<ICommandBinding, RemoveImageCommand>();
            services.AddSingleton<ICommandBinding, ReplyToCommand>();
            services.AddSingleton<ICommandBinding, ScrollToHomeCommand>();
            services.AddSingleton<ICommandBinding, SearchCommand>();
            services.AddSingleton<ICommandBinding, ShowTwitterStatusCommand>();
            services.AddSingleton<ICommandBinding, ShowUserProfileCommand>();
            services.AddSingleton<ICommandBinding, SignOutCommand>();
            services.AddSingleton<ICommandBinding, ToggleFavoriteCommand>();
            services.AddSingleton<ICommandBinding, ToggleFollowCommand>();
            services.AddSingleton<ICommandBinding, ToggleRetweetCommand>();
            services.AddSingleton<ICommandBinding, ToggleShowComposeCommand>();
            services.AddSingleton<ICommandBinding, ToggleSpellCheckerCommand>();
            services.AddSingleton<ICommandBinding, TranslateCommand>();
            services.AddSingleton<ICommandBinding, UpdateStatusCommand>();
            //
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

        public object GetService(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return serviceProvider.GetService(type) ?? throw new NotSupportedException(type.Name);
        }

        public T GetService<T>()
        {
            return serviceProvider.GetService<T>() ?? throw new NotSupportedException(typeof(T).Name);
        }
    }
}