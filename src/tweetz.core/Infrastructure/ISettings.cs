using System.ComponentModel;
using tweetz.core.Models;

namespace tweetz.core.Infrastructure
{
    public interface ISettings : INotifyPropertyChanged
    {
        bool IsAuthenticated { get; }
        string? AccessToken { get; set; }
        string? AccessTokenSecret { get; set; }
        string? ScreenName { get; set; }
        bool HideProfileImages { get; }
        bool HideImages { get; }
        bool HideExtendedContent { get; }
        bool PauseWhenScrolled { get; set; }
        bool SpellCheck { get; set; }
        double FontSize { get; }
        string? Theme { get; }
        bool Donated { get; }
        WindowPosition MainWindowPosition { get; set; }

        void Load();

        void Save();
    }
}