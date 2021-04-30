using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Jab;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = BootStrapper.ServiceProvider.GetService<MainWindowViewModel>();
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
            try
            {
                DragMove();
            }
            catch (InvalidOperationException)
            {
                // From crash report on 2020-04-11
                //
                // System.InvalidOperationException: Can only call DragMove when
                // primary mouse button is down.
                //
                // Stack trace led to this mouse down handler so how the mouse
                // is not down is a bit of mystery. Catch it to avoid an app crash.
            }

            base.OnMouseLeftButtonDown(e);
        }
    }
}