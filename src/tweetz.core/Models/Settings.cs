using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using tweetz.core.Infrastructure;

namespace tweetz.core.Models
{
    public class Settings : NotifyPropertyChanged, ISettings, IEquatable<Settings>
    {
        private string? accessToken;
        private string? accessTokenSecret;
        private string? screenName;
        private bool hideProfileImages;
        private bool hideImages;
        private bool pauseWhenScrolled;
        private bool donated;
        private bool spellCheck;
        private double fontSize = 12;
        private string theme = "dark";
        private WindowPosition mainWindowPosition = new WindowPosition { Left = 10, Top = 10, Width = 350, Height = 900 };

        public string? AccessToken
        {
            get => accessToken;
            set
            {
                SetProperty(ref accessToken, value);
                NotifyAuthenticatedChanged();
            }
        }

        public string? AccessTokenSecret
        {
            get => accessTokenSecret;
            set
            {
                SetProperty(ref accessTokenSecret, value);
                NotifyAuthenticatedChanged();
            }
        }

        [JsonIgnore]
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(AccessTokenSecret);

        [JsonIgnore]
        public bool IsNotAuthenticated => !IsAuthenticated; // XAML is such a turd

        private void NotifyAuthenticatedChanged()
        {
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(IsNotAuthenticated));
        }

        public string? ScreenName { get => screenName; set => SetProperty(ref screenName, value); }
        public bool HideProfileImages { get => hideProfileImages; set => SetProperty(ref hideProfileImages, value); }
        public bool HideImages { get => hideImages; set => SetProperty(ref hideImages, value); }
        public bool PauseWhenScrolled { get => pauseWhenScrolled; set => SetProperty(ref pauseWhenScrolled, value); }
        public bool SpellCheck { get => spellCheck; set => SetProperty(ref spellCheck, value); }
        public bool Donated { get => donated; set => SetProperty(ref donated, value); }
        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }
        public string Theme { get => theme; set => SetProperty(ref theme, value); }
        public WindowPosition MainWindowPosition { get => mainWindowPosition; set => SetProperty(ref mainWindowPosition, value); }

        // Load / Save

        [JsonIgnore]
        public string SettingsFilePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "tweetz.core.settings.txt");
            }
        }

        public void Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json);
                AccessToken = settings.AccessToken;
                AccessTokenSecret = settings.AccessTokenSecret;
                ScreenName = settings.ScreenName;
                HideProfileImages = settings.HideProfileImages;
                HideImages = settings.HideImages;
                PauseWhenScrolled = settings.PauseWhenScrolled;
                SpellCheck = settings.SpellCheck;
                FontSize = settings.FontSize;
                Theme = settings.Theme;
                Donated = settings.Donated;
                MainWindowPosition = settings.MainWindowPosition;
            }
            catch
            {
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize<Settings>(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }

        // IEquatable

        public override int GetHashCode()
        {
            return AsTuple().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is Settings o && Equals(o);
        }

        public bool Equals([AllowNull] Settings other)
        {
            return other != null && AsTuple() == other.AsTuple();
        }

        private (
            string? AccessToken,
            string? AccessTokenSecret,
            string? ScreenName,
            bool HideProfileImages,
            bool HideImages,
            bool PauseWhenScrolled,
            bool SpellCheck,
            double FontSize,
            string Theme,
            bool Donated,
            WindowPosition MainWindowPosition
            ) AsTuple()
        {
            return (
                AccessToken,
                AccessTokenSecret,
                ScreenName,
                HideProfileImages,
                HideImages,
                PauseWhenScrolled,
                SpellCheck,
                FontSize,
                Theme,
                Donated,
                MainWindowPosition
                );
        }
    }
}