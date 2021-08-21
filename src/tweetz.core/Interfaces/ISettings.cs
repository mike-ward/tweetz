using System.ComponentModel;
using tweetz.core.Models;

namespace tweetz.core.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        bool           IsAuthenticated       { get; }
        string?        AccessToken           { get; set; }
        string?        AccessTokenSecret     { get; set; }
        string?        ScreenName            { get; set; }
        bool           HideProfileImages     { get; }
        bool           HideImages            { get; }
        bool           HideExtendedContent   { get; }
        bool           HideScreenName        { get; }
        bool           HidePossiblySensitive { get; }
        bool           HideTranslate         { get; }
        bool           SpellCheck            { get; set; }
        bool           ShowInSystemTray      { get; set; }
        bool           AlwaysOnTop           { get; set; }
        double         FontSize              { get; set; }
        string?        Theme                 { get; }
        bool           ApplyGrayscaleShader  { get; set; }
        string?        MyTweetColor          { get; set; }
        string?        TranslateApiKey       { get; set; }
        bool           Donated               { get; }
        WindowPosition MainWindowPosition    { get; set; }

        void Load();

        void Save();

        string SettingsFilePath { get; }
    }
}