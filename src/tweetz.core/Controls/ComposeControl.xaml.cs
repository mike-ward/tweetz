using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class ComposeControl : UserControl
    {
        public ComposeControl()
        {
            InitializeComponent();

            IsVisibleChanged += async (s, args) =>
            {
                if (DataContext is ComposeControlViewModel vm) await vm.GetUserInfo();
            };
        }
    }
}