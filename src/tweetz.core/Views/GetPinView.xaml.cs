using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jab;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class GetPinView : UserControl
    {
        private GetPinControlViewModel ViewModel => (GetPinControlViewModel)DataContext;

        public GetPinView()
        {
            DataContext = App.ServiceProvider.GetService<GetPinControlViewModel>();
            InitializeComponent();
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void GetPinClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await ViewModel.GetPinAsync().ConfigureAwait(true);
                await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                ShowPage(2);
            }
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void SignInClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await ViewModel.SignInAsync().ConfigureAwait(true);
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
            if (page is < 1 or > 2) throw new ArgumentOutOfRangeException(nameof(page), "page argument must be 1 or 2");
            Page1.Visibility = page == 1
                ? Visibility.Visible
                : Visibility.Hidden;
            Page2.Visibility = page == 2
                ? Visibility.Visible
                : Visibility.Hidden;
        }
    }
}