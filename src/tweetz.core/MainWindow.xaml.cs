﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using tweetz.core.Extensions;
using tweetz.core.Services;
using tweetz.core.ViewModels;

namespace tweetz.core
{
    public partial class MainWindow : Window
    {
        private ImageSource? NewTweetsIcon { get; set; }

        public MainWindow()
        {
            DataContext = App.ServiceProvider.GetService<MainWindowViewModel>();
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        protected override void OnSourceInitialized(EventArgs e)
        {
            using var stream2 = Application.GetResourceStream(new Uri("pack://application:,,,/green-dot.ico"))!.Stream;
            NewTweetsIcon = new Icon(stream2).ToImageSource();

            ViewModel.Initialize(this);
            base.OnSourceInitialized(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnClosing(this);
            base.OnClosing(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && ViewModel.Settings.ShowInSystemTray)
            {
                Hide();
            }

            if (WindowState != WindowState.Minimized)
            {
                UpdateAppIcon();
            }

            base.OnStateChanged(e);
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

        public void UpdateAppIcon(bool newTweetsAvailable = false)
        {
            TraceService.Message($"UpdateAppIcon({newTweetsAvailable})");

            TaskbarItemInfo ??= new TaskbarItemInfo();

            TaskbarItemInfo.Overlay = newTweetsAvailable
                ? NewTweetsIcon
                : null;
        }
    }
}