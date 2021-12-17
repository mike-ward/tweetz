using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class ComposeView : UserControl
    {
        public ComposeView()
        {
            InitializeComponent();
            IsVisibleChanged += OnIsVisibleChangedAsync;
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void OnIsVisibleChangedAsync(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (DataContext is ComposeControlViewModel vm)
                {
                    await vm.GetUserInfoAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}