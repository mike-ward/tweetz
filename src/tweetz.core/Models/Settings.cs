using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using tweetz.core.Interfaces;
using tweetz.core.Services;

namespace tweetz.core.Models
{
    public class Settings : NotifyPropertyChanged, ISettings
    {
        private const string defaultFontFamily = "Segoe UI";

        public static Settings? SettingsStatic { get; private set; }

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
            SettingsStatic    = this;
        }

        private string              Profile           { get; }
        private IMessageBoxService? MessageBoxService { get; }

        private string?        accessToken;
        private string?        accessTokenSecret;
        private bool           alternateLayout;
        private string?        screenName;
        private bool           hideProfileImages;
        private bool           hideImages;
        private bool           imagesAsLinks;
        private bool           hideExtendedContent;
        private bool           hideScreenName;
        private bool           hidePossiblySensitive;
        private bool           hideTranslate;
        private bool           donated;
        private bool           spellCheck;
        private bool           showInSystemTray;
        private bool           alwaysOnTop;
        private bool           applyGrayscaleShader;
        private double         fontSize   = 12;
        private string         fontFamily = defaultFontFamily;
        private string         theme      = "dark";
        private string?        myTweetColor;
        private string?        translateApiKey;
        private bool           shortLinks;
        private bool           gdiFontMetrics;
        private WindowPosition mainWindowPosition = new() { Left = 10, Top = 10, Width = 350, Height = 900 };

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

        public bool AlternateLayout
        {
            get => alternateLayout;
            set => SetProperty(ref alternateLayout, value);
        }

        public string? ScreenName
        {
            get => screenName;
            set => SetProperty(ref screenName, value);
        }

        public bool HideProfileImages
        {
            get => hideProfileImages;
            set => SetProperty(ref hideProfileImages, value);
        }

        public bool HideImages
        {
            get => hideImages;
            set => SetProperty(ref hideImages, value);
        }

        public bool ImagesAsLinks
        {
            get => imagesAsLinks;
            set => SetProperty(ref imagesAsLinks, value);
        }

        public bool HideExtendedContent
        {
            get => hideExtendedContent;
            set => SetProperty(ref hideExtendedContent, value);
        }

        public bool HideScreenName
        {
            get => hideScreenName;
            set => SetProperty(ref hideScreenName, value);
        }

        public bool HidePossiblySensitive
        {
            get => hidePossiblySensitive;
            set => SetProperty(ref hidePossiblySensitive, value);
        }

        public bool HideTranslate
        {
            get => hideTranslate;
            set => SetProperty(ref hideTranslate, value);
        }

        public bool SpellCheck
        {
            get => spellCheck;
            set => SetProperty(ref spellCheck, value);
        }

        public bool ShowInSystemTray
        {
            get => showInSystemTray;
            set => SetProperty(ref showInSystemTray, value);
        }

        public bool AlwaysOnTop
        {
            get => alwaysOnTop;
            set => SetProperty(ref alwaysOnTop, value);
        }

        public bool Donated
        {
            get => donated;
            set => SetProperty(ref donated, value);
        }

        public bool ApplyGrayscaleShader
        {
            get => applyGrayscaleShader;
            set => SetProperty(ref applyGrayscaleShader, value);
        }

        public double FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }

        public string FontFamily
        {
            get => fontFamily;
            set => SetProperty(ref fontFamily, value);
        }

        public string Theme
        {
            get => theme;
            set => SetProperty(ref theme, value);
        }

        public string? MyTweetColor
        {
            get => myTweetColor;
            set => SetProperty(ref myTweetColor, value);
        }

        public string? TranslateApiKey
        {
            get => translateApiKey;
            set => SetProperty(ref translateApiKey, value);
        }

        public ObservableHashSet<string> HiddenImageSet { get; set; } = new();

        public bool ShortLinks
        {
            get => shortLinks;
            set => SetProperty(ref shortLinks, value);
        }

        public bool GdiFontMetrics
        {
            get => gdiFontMetrics;
            set => SetProperty(ref gdiFontMetrics, value);
        }

        public WindowPosition MainWindowPosition
        {
            get => mainWindowPosition;
            set => SetProperty(ref mainWindowPosition, value);
        }

        // Load / Save

        [JsonIgnore]
        public string SettingsFilePath => Path.Combine(
            AppContext.BaseDirectory,
            $"{Profile}.settings.txt");

        public void Load()
        {
            try
            {
                var json     = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json);
                if (settings is null) throw new InvalidOperationException(App.GetString("settings-read-error"));
                CopySettings(settings);
            }
            catch (Exception ex)
            {
                // falls back to defaults
                TraceService.Message(ex.Message + App.GetString("settings-continue-defaults"));
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (IOException ex)
            {
                var retry  = App.GetString("try-again");
                var result = MessageBoxService?.ShowMessageBoxYesNo(ex.Message + $"\n\n{retry}");
                if (result == MessageBoxResult.Yes) Save();
            }
        }

        private void CopySettings(Settings settings)
        {
            AccessToken           = settings.AccessToken;
            AccessTokenSecret     = settings.AccessTokenSecret;
            AlternateLayout       = settings.AlternateLayout;
            ScreenName            = settings.ScreenName;
            HideProfileImages     = settings.HideProfileImages;
            HideImages            = settings.HideImages;
            ImagesAsLinks         = settings.ImagesAsLinks;
            HideExtendedContent   = settings.HideExtendedContent;
            HideScreenName        = settings.HideScreenName;
            HidePossiblySensitive = settings.HidePossiblySensitive;
            HideTranslate         = settings.HideTranslate;
            SpellCheck            = settings.SpellCheck;
            ShowInSystemTray      = settings.showInSystemTray;
            AlwaysOnTop           = settings.alwaysOnTop;
            FontSize              = settings.FontSize;
            FontFamily            = settings.FontFamily;
            Theme                 = settings.Theme;
            ApplyGrayscaleShader  = settings.ApplyGrayscaleShader;
            MyTweetColor          = settings.MyTweetColor;
            Donated               = settings.Donated;
            TranslateApiKey       = settings.TranslateApiKey;
            MainWindowPosition    = settings.MainWindowPosition;
            HiddenImageSet        = settings.HiddenImageSet;
            ShortLinks            = settings.ShortLinks;
            GdiFontMetrics        = settings.GdiFontMetrics;

            void OnHiddenImageSetOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(HiddenImageSet));
            HiddenImageSet.CollectionChanged -= OnHiddenImageSetOnCollectionChanged;
            HiddenImageSet.CollectionChanged += OnHiddenImageSetOnCollectionChanged;
        }
    }
}