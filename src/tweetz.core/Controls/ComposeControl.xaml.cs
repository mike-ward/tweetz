using System.Threading.Tasks;
using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class ComposeControl : UserControl
    {
        public ComposeControl()
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
                await vm.GetUserInfo().ConfigureAwait(false);
            }
        }
    }
}