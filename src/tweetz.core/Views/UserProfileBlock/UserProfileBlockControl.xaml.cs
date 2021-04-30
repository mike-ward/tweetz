using System.Windows;
using System.Windows.Controls;
using Jab;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.UserProfileBlock
{
    public partial class UserProfileBlockControl : UserControl
    {
        public UserProfileBlockControl()
        {
            DataContext = BootStrapper.ServiceProvider.GetService<UserProfileBlockViewModel>();
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