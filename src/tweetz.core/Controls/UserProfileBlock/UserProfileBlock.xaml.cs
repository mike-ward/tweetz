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

            // UserProfileBlock is not in the Visual Tree (it's a popup)
            DataContext = (Application.Current as App)?.ServiceProvider.GetService(typeof(UserProfileBlockViewModel));
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            // UserProfileBlock is not in the Visual Tree so it can't inherit FontSize
            FontSize = Application.Current.MainWindow.FontSize;

            if (DataContext is UserProfileBlockViewModel userProfileBlockViewModel)
            {
                await userProfileBlockViewModel.GetUserInfo(Tag as string);
            }
        }
    }
}