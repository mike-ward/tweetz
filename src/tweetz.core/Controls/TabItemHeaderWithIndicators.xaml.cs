using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using tweetz.core.Controls.Adorners;

namespace tweetz.core.Controls
{
    public partial class TabItemHeaderWithIndicators : UserControl
    {
        private TabItemIndicatorAdorner? _adorner;

        public TabItemHeaderWithIndicators()
        {
            InitializeComponent();
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var parent = (UIElement)HeaderTextBlock.Parent;
            if (parent == null) return;

            if (IsVisible)
            {
                // Bummer adorners can't be used in XAML
                // (well, not easily)
                var layer = AdornerLayer.GetAdornerLayer(parent);
                if (layer == null) return;

                var brush = new SolidColorBrush(IndicatorColor);
                brush.Freeze();

                _adorner = new TabItemIndicatorAdorner(parent, brush);
                layer.Add(_adorner);

                var binding = new Binding()
                {
                    Path = new PropertyPath("ToolTip"),
                    Source = this,
                    Mode = BindingMode.OneWay,
                    Converter = Application.Current.FindResource("NotNullToVisibilityConverter") as IValueConverter
                };

                _adorner.SetBinding(VisibilityProperty, binding);
            }
            else
            {
                var adorner = _adorner;
                if (adorner != null)
                {
                    AdornerLayer.GetAdornerLayer(parent)?.Remove(adorner);
                    _adorner = null;
                }
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(TabItemHeaderWithIndicators),
                new PropertyMetadata(string.Empty));

        public Color IndicatorColor
        {
            get { return (Color)GetValue(IndicatorColorProperty); }
            set { SetValue(IndicatorColorProperty, value); }
        }

        public static readonly DependencyProperty IndicatorColorProperty = DependencyProperty.Register(
            "IndicatorColor",
            typeof(Color),
            typeof(TabItemHeaderWithIndicators),
            new PropertyMetadata(Colors.Crimson));
    }
}