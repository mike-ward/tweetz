using System.Windows;
using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls.UserProfileBlock
{
    public partial class UserProfileBlock : UserControl
    {
        public UserProfileBlock()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserProfileBlockViewModel vm)
            {
                await vm.GetUserInfo(Tag as string);
            }
        }
    }
}