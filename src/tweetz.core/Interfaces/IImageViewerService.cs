using System;

namespace tweetz.core.Interfaces
{
    public interface IImageViewerService
    {
        void Open(Uri uri);

        void Close();
    }
}