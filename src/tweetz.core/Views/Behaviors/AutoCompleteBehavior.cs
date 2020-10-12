using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace tweetz.core.Views.Behaviors
{
    public static class AutoCompleteBehavior
    {
        private static readonly KeyEventHandler KeyDown = new KeyEventHandler(OnPreviewKeyDown);
        private static readonly TextChangedEventHandler TextChanged = new TextChangedEventHandler(OnTextChanged);

        public static readonly DependencyProperty AutoCompleteItemsSource =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteItemsSource",
                typeof(IEnumerable<string>),
                typeof(AutoCompleteBehavior),
                new UIPropertyMetadata(null, OnAutoCompleteItemsSource)
            );

        public static IEnumerable<string>? GetAutoCompleteItemsSource(DependencyObject obj) => obj.GetValue(AutoCompleteItemsSource) as IEnumerable<string>;

        public static void SetAutoCompleteItemsSource(DependencyObject obj, IEnumerable<string> value) => obj.SetValue(AutoCompleteItemsSource, value);

        private static void OnAutoCompleteItemsSource(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.TextChanged -= TextChanged;
                tb.PreviewKeyDown -= KeyDown;

                if (!(e.NewValue is null))
                {
                    tb.TextChanged += TextChanged;
                    tb.PreviewKeyDown += KeyDown;
                }
            }
        }

        public static readonly DependencyProperty AutoCompleteStringComparison =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteStringComparison",
                typeof(StringComparison),
                typeof(AutoCompleteBehavior),
                new UIPropertyMetadata(StringComparison.OrdinalIgnoreCase)
            );

        public static StringComparison GetAutoCompleteStringComparison(DependencyObject obj) => (StringComparison)obj.GetValue(AutoCompleteStringComparison);

        public static void SetAutoCompleteStringComparison(DependencyObject obj, StringComparison value) => obj.SetValue(AutoCompleteStringComparison, value);

        public static readonly DependencyProperty AutoCompleteIndicator =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteIndicator",
                typeof(string),
                typeof(AutoCompleteBehavior),
                new UIPropertyMetadata(String.Empty)
            );

        public static string GetAutoCompleteIndicator(DependencyObject obj) => (string)obj.GetValue(AutoCompleteIndicator);

        public static void SetAutoCompleteIndicator(DependencyObject obj, String value) => obj.SetValue(AutoCompleteIndicator, value);

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) { return; }
            if (e.OriginalSource is not TextBox tb) { return; }

            // If enter pressed and the selected text goes all the way to the
            // end, move caret position to the end

            if (tb.SelectionLength > 0 && (tb.SelectionStart + tb.SelectionLength == tb.Text.Length))
            {
                tb.SelectionStart = tb.CaretIndex = tb.Text.Length;
                tb.SelectionLength = 0;
                e.Handled = true;
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is null ||
                e.OriginalSource is not TextBox tb ||
                string.IsNullOrEmpty(tb.Text))
            {
                return;
            }

            if (e.Changes.Any(change => change.RemovedLength > 0) &&
                e.Changes.All(change => change.AddedLength == 0))
            {
                return;
            }

            var values = GetAutoCompleteItemsSource(tb);
            if (values is null) { return; }

            var startIndex = 0;
            var matchingString = tb.Text;
            var indicator = GetAutoCompleteIndicator(tb);

            if (!string.IsNullOrEmpty(indicator))
            {
                startIndex = tb.Text.LastIndexOf(indicator);
                if (startIndex == -1) { return; }

                startIndex += indicator.Length;
                matchingString = tb.Text[startIndex..];
            }

            if (string.IsNullOrEmpty(matchingString)) { return; }

            // Do search and changes here.

            var textLength = matchingString.Length;
            var comparer = GetAutoCompleteStringComparison(tb);

            var match = values
                .Where(value =>
                    !string.IsNullOrEmpty(value) &&
                    value.Length >= textLength &&
                    value[..textLength].Equals(matchingString, comparer))
                .Select(value => value[textLength..]) // Only select the last part of the suggestion
                .FirstOrDefault();

            if (string.IsNullOrEmpty(match)) { return; }

            var matchStart = startIndex + matchingString.Length;
            tb.TextChanged -= TextChanged;
            tb.Text += match;
            tb.CaretIndex = matchStart;
            tb.SelectionStart = matchStart;
            tb.SelectionLength = tb.Text.Length - startIndex;
            tb.TextChanged += TextChanged;
        }
    }
}