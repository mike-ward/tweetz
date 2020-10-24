using System.Windows.Controls;
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

        private async void OnIsVisibleChangedAsync(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ComposeControlViewModel vm)
            {
                await vm.GetUserInfoAsync().ConfigureAwait(false);
            }
        }
    }
}