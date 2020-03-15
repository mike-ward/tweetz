using System;
using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Services;

namespace tweetz.core.ViewModels
{
    public class MediaViewerBlockViewModel : NotifyPropertyChanged
    {
        private Uri? _uri;
        private string? errorMessage;

        public Uri? Uri { get => _uri; set => SetProperty(ref _uri, value); }

        public string? ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }

        public Rect PlacementRectangle => Screen.ScreenRectFromWindow(Application.Current.MainWindow);
    }
}