using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using tweetz.core.Infrastructure;

namespace tweetz.core.Models
{
    public class Settings : NotifyPropertyChanged, ISettings
    {
        public Settings()
        {
            var args = Environment.GetCommandLineArgs();

            Profile = args.Length > 1
                ? string.Join("_", args[1].Split(Path.GetInvalidFileNameChars()))
                : "tweetz.core";
        }

        public Settings(IMessageBoxService messageBoxService)
            : this()
        {
            MessageBoxService = messageBoxService;
        }

        private string Profile { get; }
        private IMessageBoxService MessageBoxService { get; }

        private string? accessToken;
        private string? accessTokenSecret;
        private string? screenName;
        private bool hideProfileImages;
        private bool hideImages;
        private bool hideExtendedContent;
        private bool hideScreenName;
        private bool hidePossiblySensitive;
        private bool donated;
        private bool spellCheck;
        private bool showInSystemTray;
        private bool alwaysOnTop;
        private double fontSize = 12;
        private string theme = "dark";
        private WindowPosition mainWindowPosition = new WindowPosition { Left = 10, Top = 10, Width = 350, Height = 900 };

        [JsonIgnore]
        public bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(AccessToken) &&
            !string.IsNullOrWhiteSpace(AccessTokenSecret);

        public string? AccessToken
        {
            get => accessToken;
            set
            {
                SetProperty(ref accessToken, value);
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public string? AccessTokenSecret
        {
            get => accessTokenSecret;
            set
            {
                SetProperty(ref accessTokenSecret, value);
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public string? ScreenName { get => screenName; set => SetProperty(ref screenName, value); }
        public bool HideProfileImages { get => hideProfileImages; set => SetProperty(ref hideProfileImages, value); }
        public bool HideImages { get => hideImages; set => SetProperty(ref hideImages, value); }
        public bool HideExtendedContent { get => hideExtendedContent; set => SetProperty(ref hideExtendedContent, value); }
        public bool HideScreenName { get => hideScreenName; set => SetProperty(ref hideScreenName, value); }
        public bool HidePossiblySensitive { get => hidePossiblySensitive; set => SetProperty(ref hidePossiblySensitive, value); }
        public bool SpellCheck { get => spellCheck; set => SetProperty(ref spellCheck, value); }
        public bool ShowInSystemTray { get => showInSystemTray; set => SetProperty(ref showInSystemTray, value); }
        public bool AlwaysOnTop { get => alwaysOnTop; set => SetProperty(ref alwaysOnTop, value); }
        public bool Donated { get => donated; set => SetProperty(ref donated, value); }
        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }
        public string Theme { get => theme; set => SetProperty(ref theme, value); }
        public WindowPosition MainWindowPosition { get => mainWindowPosition; set => SetProperty(ref mainWindowPosition, value); }

        // Load / Save

        [JsonIgnore]
        public string SettingsFilePath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
            $"{Profile}.settings.txt");

        public void Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json)!;
                AccessToken = settings.AccessToken;
                AccessTokenSecret = settings.AccessTokenSecret;
                ScreenName = settings.ScreenName;
                HideProfileImages = settings.HideProfileImages;
                HideImages = settings.HideImages;
                HideExtendedContent = settings.HideExtendedContent;
                HideScreenName = settings.HideScreenName;
                HidePossiblySensitive = settings.HidePossiblySensitive;
                SpellCheck = settings.SpellCheck;
                ShowInSystemTray = settings.showInSystemTray;
                AlwaysOnTop = settings.alwaysOnTop;
                FontSize = settings.FontSize;
                Theme = settings.Theme;
                Donated = settings.Donated;
                MainWindowPosition = settings.MainWindowPosition;
            }
            catch (Exception ex)
            {
                // falls back to defaults
                Trace.TraceError(ex.Message);
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize<Settings>(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (IOException ex)
            {
                var retry = (string)Application.Current.FindResource("try-again") ?? string.Empty;
                var result = MessageBoxService.ShowMessageBoxYesNo(ex.Message + $"\n\n{retry}");
                if (result == MessageBoxResult.Yes) Save();
            }
        }
    }
}