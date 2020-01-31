using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Commands;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();

            void LoadedHandler(object sender, RoutedEventArgs args)
            {
                // this check allows the xaml designer to render
                if (DataContext is SearchControlViewModel vm) vm.SetSearchText = SetSearchText;
                Loaded -= LoadedHandler;
            }

            Loaded += LoadedHandler;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) && !string.IsNullOrEmpty(Search.Text))
            {
                SearchCommand.Command.Execute(Search.Text, Search);
            }
        }

        private void SetSearchText(string text)
        {
            Search.Text = text;
            Search.SelectionStart = text.Length;
            GoToSearchCommand.Command.Execute(text, Search);
        }
    }
}