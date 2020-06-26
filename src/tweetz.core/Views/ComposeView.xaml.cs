using System.Threading.Tasks;
using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class ComposeView : UserControl
    {
        public ComposeView()
        {
            InitializeComponent();
            IsVisibleChanged += OnIsVisibleChnaged;
        }

        private void OnIsVisibleChnaged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            OnIsVisibleChnagedAsync().ConfigureAwait(false);
        }

        private async ValueTask OnIsVisibleChnagedAsync()
        {
            if (DataContext is ComposeControlViewModel vm)
            {
                await vm.GetUserInfoAsync().ConfigureAwait(false);
            }
        }
    }
}