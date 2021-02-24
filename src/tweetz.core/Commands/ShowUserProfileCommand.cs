using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ShowUserProfileCommand : ICommandBinding
    {
        private readonly MainViewModel mainViewModel;
        public static readonly RoutedCommand Command = new RoutedUICommand();

        public ShowUserProfileCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            mainViewModel.UserProfile = e.Parameter as User;
        }
    }
}