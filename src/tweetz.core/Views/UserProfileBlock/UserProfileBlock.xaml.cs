using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.UserProfileBlock
{
    public partial class UserProfileBlock : UserControl
    {
        public UserProfileBlock()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            OnLoadAsync().ConfigureAwait(false);
        }

        private async ValueTask OnLoadAsync()
        {
            if (DataContext is UserProfileBlockViewModel vm)
            {
                await vm.GetUserInfoAsync(Tag as string).ConfigureAwait(false);
            }
        }
    }
}