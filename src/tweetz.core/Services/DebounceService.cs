using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace tweetz.core.Services
{
    internal static class DebounceService
    {
        public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
        {
            CancellationTokenSource? cancelTokenSource = null;

            return arg =>
            {
                cancelTokenSource?.Cancel();
                cancelTokenSource = new CancellationTokenSource();

                Task.Delay(milliseconds, cancelTokenSource.Token)
                    .ContinueWith(t =>
                    {
                        if (t.IsCompletedSuccessfully)
                        {
                            Application.Current.Dispatcher.Invoke(() => func(arg));
                        }
                    }, TaskScheduler.Default);
            };
        }
    }
}