using System;
using tweetz.core.Infrastructure;

namespace tweetz.core.ViewModels
{
    public class MediaViewerBlockViewModel : NotifyPropertyChanged
    {
        private Uri? _uri;
        private string? errorMessage;

        public Uri? Uri { get => _uri; set => SetProperty(ref _uri, value); }

        public string? ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }
    }
}