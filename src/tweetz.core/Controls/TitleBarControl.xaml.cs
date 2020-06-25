using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Commands;

namespace tweetz.core.Controls
{
    public partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            InitializeComponent();
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            ApplicationCommands.Close.Execute(null, this);
        }

        private void Minimize(object sender, MouseButtonEventArgs e)
        {
            MinimizeCommand.Command.Execute(null, this);
        }
    }
}