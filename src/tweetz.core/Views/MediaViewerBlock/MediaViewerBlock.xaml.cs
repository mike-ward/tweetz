using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.MediaViewerBlock
{
    public partial class MediaViewerBlock : UserControl
    {
        public MediaViewerBlock()
        {
            InitializeComponent();
            DataContextChanged += UpdateDataContext;
        }

        private MediaViewerBlockViewModel ViewModel => (MediaViewerBlockViewModel)DataContext;

        private void UpdateDataContext(object _, DependencyPropertyChangedEventArgs e)
        {
            MediaControls.DataContext = e.NewValue;
            ViewModel.PropertyChanged += (_, ea) =>
            {
                if (ea.PropertyName.IsEqualTo(nameof(MediaViewerBlockViewModel.Uri))) ShowLoadingIndicator();
            };
        }

        private void ShowLoadingIndicator()
        {
            MediaElement.Stop();
            MediaElement.Source      = null;
            MediaElement.Visibility  = Visibility.Collapsed;
            ImageElement.Visibility  = Visibility.Collapsed;
            MediaControls.Visibility = Visibility.Collapsed;
            MediaControls.SetControlVisibilities(Visibility.Collapsed);
            ViewModel.ErrorMessage      = null;
            LoadingIndicator.Visibility = Visibility.Collapsed;

            var uri = ((MediaViewerBlockViewModel)DataContext).Uri;
            if (Services.ImageViewerService.IsMp4(uri?.ToString()))
            {
                MediaElement.Source         = uri;
                LoadingIndicator.Visibility = Visibility.Visible;
            }
            else
            {
                ImageElement.Visibility  = Visibility.Visible;
                MediaControls.Visibility = Visibility.Visible;
            }
        }

        private void ShowMediaControls()
        {
            LoadingIndicator.Visibility = Visibility.Collapsed;
            MediaControls.Visibility    = Visibility.Visible;
        }

        private void Popup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
            e.Handled = true;
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void Popup_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
            e.Handled = true;
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            ShowMediaControls();
            MediaElement.Play();
            MediaElement.Visibility = Visibility.Visible;
        }

        private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            LoadingIndicator.Visibility = Visibility.Collapsed;
            ViewModel.ErrorMessage      = e.ErrorException.Message;
        }

        private void Close()
        {
            ViewModel.Uri = null;
            if (MediaElement is not null)
            {
                MediaElement.Close();
                MediaElement.Source     = null;
                MediaElement.Visibility = Visibility.Collapsed;
            }

            ImageElement.Visibility = Visibility.Collapsed;
        }

        private void Popup_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MediaElement.Volume = e.Delta > 0
                ? Math.Min(MediaElement.Volume + 0.1, 1.0)
                : Math.Max(MediaElement.Volume - 0.1, 0.0);
        }
    }
}