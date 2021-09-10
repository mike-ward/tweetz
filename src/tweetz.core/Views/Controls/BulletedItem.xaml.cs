using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Views.Controls
{
    public partial class BulletedItem : UserControl
    {
        public BulletedItem()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(BulletText),
            typeof(string),
            typeof(BulletedItem));

        public string BulletText
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}