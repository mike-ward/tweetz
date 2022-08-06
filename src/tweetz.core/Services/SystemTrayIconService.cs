﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;

namespace tweetz.core.Services
{
    public class SystemTrayIconService : ISystemTrayIconService
    {
        private bool       disposed;
        private ISettings  Settings   { get; }
        private NotifyIcon NotifyIcon { get; }

        public SystemTrayIconService(ISettings settings)
        {
            Settings   = settings;
            NotifyIcon = new NotifyIcon();
        }

        public void Initialize(Window window)
        {
            NotifyIcon.Tag  = window;
            NotifyIcon.Text = App.GetString("title");

            var path = System.Reflection.Assembly.GetEntryAssembly()!.ManifestModule.FullyQualifiedName;
            NotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(path);

            ShowInSystemTray(window);
            NotifyIcon.Click         += OnClick;
            Settings.PropertyChanged += UpdateVisibility;
        }

        private void OnClick(object? _, EventArgs __)
        {
            // Bring window to front
            var window = NotifyIcon.Tag as Window;
            if (window is not null)
            {
                window.WindowState = WindowState.Minimized;
                window.Show();
                window.WindowState = WindowState.Normal;
            }
        }

        private void UpdateVisibility(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(Settings.ShowInSystemTray)))
            {
                var window = NotifyIcon.Tag as Window;
                if (window is not null) ShowInSystemTray(window);
            }
        }

        public void Close()
        {
            if (!disposed)
            {
                disposed                 =  true;
                NotifyIcon.Tag           =  null;
                NotifyIcon.Visible       =  false;
                NotifyIcon.Click         -= OnClick;
                Settings.PropertyChanged -= UpdateVisibility;
                NotifyIcon.Dispose();
            }
        }

        private void ShowInSystemTray(Window window)
        {
            window.ShowInTaskbar = !Settings.ShowInSystemTray;
            NotifyIcon.Visible   = Settings.ShowInSystemTray;
        }
    }
}