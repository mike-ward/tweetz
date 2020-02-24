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

        private async void OnIsVisibleChnaged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ComposeControlViewModel vm)
            {
                await vm.GetUserInfo();
            }
        }
    }
}