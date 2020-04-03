using System;

namespace tweetz.core.Infrastructure
{
    public interface IImageViewerService
    {
        void Open(Uri uri);

        void Close();
    }
}