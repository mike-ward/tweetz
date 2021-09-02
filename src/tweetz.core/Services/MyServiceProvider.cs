using Jab;
using tweetz.core.Commands;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.ViewModels;

namespace tweetz.core.Services
{
    [ServiceProvider]
    //
    [Singleton(typeof(MainWindow))]
    [Singleton(typeof(MainWindowViewModel))]
    [Singleton(typeof(MainViewModel))]
    [Singleton(typeof(GetPinControlViewModel))]
    [Singleton(typeof(TabBarControlViewModel))]
    [Singleton(typeof(HomeTimelineControlViewModel))]
    [Singleton(typeof(FavoritesTimelineControlViewModel))]
    [Singleton(typeof(SearchControlViewModel))]
    [Singleton(typeof(SettingsControlViewModel))]
    [Singleton(typeof(ComposeControlViewModel))]
    [Singleton(typeof(UserProfileBlockViewModel))]
    [Singleton(typeof(MediaViewerBlockViewModel))]
    [Singleton(typeof(TitleBarControlViewModel))]
    [Singleton(typeof(UserProfileTimelineViewModel))]
    // 
    [Singleton(typeof(ICommandBinding), typeof(AddImageCommand))]
    [Singleton(typeof(ICommandBinding), typeof(DecreaseFontSizeCommand))]
    [Singleton(typeof(ICommandBinding), typeof(GetMentionsCommand))]
    [Singleton(typeof(ICommandBinding), typeof(GoToSearchCommand))]
    [Singleton(typeof(ICommandBinding), typeof(HideImageCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ImageViewerCommand))]
    [Singleton(typeof(ICommandBinding), typeof(IncreaseFontSizeCommand))]
    [Singleton(typeof(ICommandBinding), typeof(MinimizeCommand))]
    [Singleton(typeof(ICommandBinding), typeof(OpenLinkCommand))]
    [Singleton(typeof(ICommandBinding), typeof(QuoteTweetCommand))]
    [Singleton(typeof(ICommandBinding), typeof(RemoveImageCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ReplyToCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ScrollToHomeCommand))]
    [Singleton(typeof(ICommandBinding), typeof(SearchCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ShowTwitterStatusCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ShowUserProfileCommand))]
    [Singleton(typeof(ICommandBinding), typeof(SignOutCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ToggleFavoriteCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ToggleFollowCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ToggleRetweetCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ToggleShowComposeCommand))]
    [Singleton(typeof(ICommandBinding), typeof(ToggleSpellCheckerCommand))]
    [Singleton(typeof(ICommandBinding), typeof(TranslateCommand))]
    [Singleton(typeof(ICommandBinding), typeof(UpdateStatusCommand))]
    //
    [Singleton(typeof(ICheckForUpdates), typeof(CheckForUpdates))]
    [Singleton(typeof(IImageViewerService), typeof(ImageViewerService))]
    [Singleton(typeof(IMessageBoxService), typeof(MessageBoxService))]
    [Singleton(typeof(IOpenUrlService), typeof(OpenUrlService))]
    [Singleton(typeof(ISettings), typeof(Settings))]
    [Singleton(typeof(ISystemState), typeof(SystemState))]
    [Singleton(typeof(ISystemTrayIconService), typeof(SystemTrayIconService))]
    [Singleton(typeof(ITwitterService), typeof(TwitterService))]
    [Singleton(typeof(IWindowInteropService), typeof(WindowInteropService))]
    //
    // partial class required so Jab generate code
    //
    public partial class MyServiceProvider { }
}