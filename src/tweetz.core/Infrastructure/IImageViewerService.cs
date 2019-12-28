using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using twitter.core.Models;

namespace tweetz.core.Infrastructure
{
    public interface IImageViewerService
    {
        Popup CreatePopup(Window window, Uri uri);

        Uri MediaSource(Media media);
    }
}