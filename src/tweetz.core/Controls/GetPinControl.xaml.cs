using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class GetPinControl : UserControl
    {
        private GetPinControlViewModel ViewModel => (GetPinControlViewModel)DataContext;

        public GetPinControl()
        {
            InitializeComponent();
        }

        private async void GetPinClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await ViewModel.GetPin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                const int fiveSeconds = 5 * 1000;
                await Task.Delay(fiveSeconds);
                Mouse.OverrideCursor = Cursors.Arrow;
                ShowPage(2);
            }
        }

        private async void SignInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await ViewModel.SignIn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ShowPage(1);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                ShowPage(1);
            }
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            ShowPage(1);
        }

        private void ShowPage(int page)
        {
            if (page < 1 || page > 2) throw new IndexOutOfRangeException("page argument must be 1 or 2");
            Page1.Visibility = page == 1 ? Visibility.Visible : Visibility.Hidden;
            Page2.Visibility = page == 2 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}