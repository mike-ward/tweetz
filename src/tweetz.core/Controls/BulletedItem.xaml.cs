using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Controls
{
    public partial class BulletedItem : UserControl
    {
        public BulletedItem()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "BulletText",
            typeof(string),
            typeof(BulletedItem));

        public string BulletText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}