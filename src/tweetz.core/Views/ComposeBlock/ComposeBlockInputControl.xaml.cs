﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Commands;
using tweetz.core.ViewModels;
using tweetz.core.Views.Adorners;

namespace tweetz.core.Views.ComposeBlock
{
    public partial class ComposeBlockInputControl : UserControl
    {
        public ComposeBlockInputControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Could not figure a way to set data context for textblock in XAML.
            // DataContext=ComposeControlViewModel is needed to determine text of watermark

            var vm        = (ComposeControlViewModel)DataContext;
            var textBlock = (TextBlock)TextBox.GetValue(WatermarkAdornerService.WatermarkProperty);
            textBlock.DataContext = vm;

            Loaded -= OnLoaded;
        }

        private void OnIsVisibleChanged(object _, DependencyPropertyChangedEventArgs __)
        {
            if (IsVisible)
            {
                TextBox.Focus();
                TextBox.SelectionStart = TextBox.Text.Length;
            }
        }

        private void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Because AcceptsReturn == true, capture ctrl+return here.

            if (!e.Handled && e.Key == Key.Return && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                var command = UpdateStatusCommand.Command;
                if (command.CanExecute(parameter: null, this)) command.Execute(parameter: null, this);
                e.Handled = true;
            }
        }
    }
}