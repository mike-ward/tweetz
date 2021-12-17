using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Jab;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core.Views.UserProfileBlock
{
    public partial class UserProfileBlockControl : UserControl
    {
        public UserProfileBlockControl()
        {
            DataContext = App.ServiceProvider.GetService<UserProfileBlockViewModel>();
            InitializeComponent();
            Loaded += OnLoad;
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is UserProfileBlockViewModel vm)
                {
                    await vm.GetUserInfoAsync(Tag as string).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}