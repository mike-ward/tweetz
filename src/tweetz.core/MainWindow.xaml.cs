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

        protected override void OnSourceInitialized(EventArgs e)
        {
            ((MainWindowViewModel)DataContext).Initiate(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Shutdown(this);
        }
    }
}