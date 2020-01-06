using System.ComponentModel;
using tweetz.core.Models;

namespace tweetz.core.Infrastructure
{
    public interface ISettings : INotifyPropertyChanged
    {
        public bool IsAuthenticated { get; }
        public string? AccessToken { get; set; }
        public string? AccessTokenSecret { get; set; }
        public string? ScreenName { get; set; }
        public bool HideProfileImages { get; }
        public bool PauseWhenScrolled { get; set; }
        public bool SpellCheck { get; set; }
        public double FontSize { get; }
        public string? Theme { get; }
        public bool Donated { get; }
        public WindowPosition MainWindowPosition { get; set; }

        public void Load();

        public void Save();
    }
}