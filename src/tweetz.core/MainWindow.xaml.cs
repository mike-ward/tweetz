using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        protected override void OnSourceInitialized(EventArgs e)
        {
            ViewModel.Initialize(this);
            base.OnSourceInitialized(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnClosing(this);
            base.OnClosing(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DragMove();
            base.OnMouseLeftButtonDown(e);
        }
    }
}