using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Commands;

namespace tweetz.core.Views
{
    public partial class TitleBarView : UserControl
    {
        public TitleBarView()
        {
            InitializeComponent();
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.Close.Execute(parameter: null, this);
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            MinimizeCommand.Command.Execute(parameter: null, this);
        }
    }
}