using System;
using System.ComponentModel;
using System.Windows;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        protected override void OnSourceInitialized(EventArgs e)
        {
            ViewModel.OnInitialized(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnClosing(this);
        }

        private void Main_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}