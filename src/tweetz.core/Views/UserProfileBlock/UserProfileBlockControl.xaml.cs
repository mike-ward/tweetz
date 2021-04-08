using System.Windows;
using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.UserProfileBlock
{
    public partial class UserProfileBlockControl : UserControl
    {
        public UserProfileBlockControl()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserProfileBlockViewModel vm)
            {
                await vm.GetUserInfoAsync(Tag as string).ConfigureAwait(false);
            }
        }
    }
}