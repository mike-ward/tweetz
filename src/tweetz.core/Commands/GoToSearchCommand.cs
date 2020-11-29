using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Extensions;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    public class GoToSearchCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private static void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            if (sender is not DependencyObject dp) { return; }

            var tabControl = dp.GetChildrenOfType<TabControl>().FirstOrDefault();
            if (tabControl is null) { return; }

            const int SearchTab = 2;
            tabControl.SelectedIndex = SearchTab;
        }
    }
}