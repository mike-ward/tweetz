using System.Windows.Controls;
using Jab;
using tweetz.core.Interfaces;

namespace tweetz.core.Views.SettingsBlock
{
    public partial class SettingsOpenBlock : UserControl
    {
        public SettingsOpenBlock()
        {
            DataContext = App.ServiceProvider.GetService<ISettings>();
            InitializeComponent();
        }
    }
}