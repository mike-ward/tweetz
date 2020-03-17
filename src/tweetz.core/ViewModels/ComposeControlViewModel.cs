using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class ComposeControlViewModel : NotifyPropertyChanged
    {
        private User user;
        private bool isUpdating;
        private bool isUploadingMedia;
        private TwitterStatus? inReplyTo;
        private string statusText = string.Empty;
        private string attachmentUrl = string.Empty;
        private string watermarkText;

        public ComposeControlViewModel(ISettings settings, ITwitterService twitterService)
        {
            Settings = settings;
            TwitterService = twitterService;
        }

        public ISettings Settings { get; }
        public ITwitterService TwitterService { get; }

        public User User { get => user; set => SetProperty(ref user, value); }
        public bool IsUpdating { get => isUpdating; set => SetProperty(ref isUpdating, value); }
        public bool IsUploadingMedia { get => isUploadingMedia; set => SetProperty(ref isUploadingMedia, value); }

        public TwitterStatus? InReplyTo { get => inReplyTo; set => SetProperty(ref inReplyTo, value); }
        public string StatusText { get => statusText; set => SetProperty(ref statusText, value); }
        public string AttachmentUrl { get => attachmentUrl; set => SetProperty(ref attachmentUrl, value); }
        public string WatermarkText { get => watermarkText; set => SetProperty(ref watermarkText, value); }
        public ObservableCollection<MediaInfo> Media { get; } = new ObservableCollection<MediaInfo>();

        public async Task GetUserInfo()
        {
            try
            {
                if (User == null)
                {
                    if (Settings.ScreenName == null) return;
                    User = await TwitterService.UserInfo(Settings.ScreenName).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private readonly string[] GifExtensions = new[] { ".gif" };
        private readonly string[] VidExtensions = new[] { ".mp4" };
        private readonly string[] ImgExtensions = new[] { ".jpg", ".png", ".webp" };

        public void Clear()
        {
            InReplyTo = null;
            StatusText = string.Empty;
            AttachmentUrl = string.Empty;
            Media.Clear();
            WatermarkText = (string)Application.Current.FindResource("whats-happening");
        }

        public bool AddImage(string filename)
        {
            var result = CanAdd(filename);
            if (result) Media.Add(new MediaInfo { Path = filename, MediaId = string.Empty });
            return result;
        }

        public void RemoveImage(string filename)
        {
            var item = Media.FirstOrDefault(mi => mi.Path == filename);
            if (item != null) Media.Remove(item);
        }

        private bool CanAdd(string filename)
        {
            var ext = Path.GetExtension(filename);
            var gif = Media.Count(m => GifExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
            var vid = Media.Count(m => VidExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
            var img = Media.Count(m => ImgExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));

            return GifExtensions.Any(gif => gif.Equals(ext, StringComparison.OrdinalIgnoreCase)) ||
                VidExtensions.Any(vid => vid.Equals(ext, StringComparison.OrdinalIgnoreCase))
                ? gif == 0 && vid == 0 && img == 0
                : ImgExtensions.Any(img => img.Equals(ext, StringComparison.OrdinalIgnoreCase)) && gif == 0 && vid == 0 && img < 4;
        }

        public bool CanAddImage()
        {
            var gif = Media.Count(m => GifExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
            var vid = Media.Count(m => VidExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
            var img = Media.Count(m => ImgExtensions.Any(ext => m.Path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));

            return gif == 0 && vid == 0 && img < 4;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "none")]
        public string ContentType(string filename)
        {
            var ext = Path.GetExtension(filename);
            if (GifExtensions.Any(gif => gif.Equals(ext, StringComparison.OrdinalIgnoreCase))) return $"image/{ext.TrimStart('.')}";
            if (VidExtensions.Any(vid => vid.Equals(ext, StringComparison.OrdinalIgnoreCase))) return $"video/{ext.TrimStart('.')}";
            if (ImgExtensions.Any(vid => vid.Equals(ext, StringComparison.OrdinalIgnoreCase))) return $"image/{ext.TrimStart('.')}";
            return string.Empty;
        }
    }
}