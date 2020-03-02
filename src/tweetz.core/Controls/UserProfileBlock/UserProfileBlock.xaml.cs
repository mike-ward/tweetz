using System.Threading.Tasks;
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

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            _ = OnLoadAsync();
        }

        private async Task OnLoadAsync()
        {
            if (DataContext is UserProfileBlockViewModel vm)
            {
                await vm.GetUserInfo(Tag as string).ConfigureAwait(false);
            }
        }
    }
}