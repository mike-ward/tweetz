using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using tweetz.core.Views.Adorners;

namespace tweetz.core.Views
{
    public partial class TabItemHeaderWithIndicators : UserControl
    {
        private TabItemErrorAdorner?     _errorAdorner;
        private TabItemNewTweetsAdorner? _newTweetsAdorner;

        public TabItemHeaderWithIndicators()
        {
            InitializeComponent();
        }

        private void OnIsVisibleChanged(object _, DependencyPropertyChangedEventArgs __)
        {
            if (!(HeaderTextBlock.Parent is UIElement parent)) return;

            if (IsVisible)
            {
                // Bummer adorners can't be used in XAML
                // (well, not easily)
                var layer = AdornerLayer.GetAdornerLayer(parent);
                if (layer is null) return;

                var errorBrush = new SolidColorBrush(ErrorIndicatorColor);
                errorBrush.Freeze();

                _errorAdorner = new TabItemErrorAdorner(parent, errorBrush);
                layer.Add(_errorAdorner);

                var errorBinding = new Binding
                {
                    Path   = new PropertyPath("ErrorIndicatorVisibility", pathParameters: Array.Empty<object>()),
                    Source = this,
                    Mode   = BindingMode.OneWay
                };

                _errorAdorner.SetBinding(VisibilityProperty, errorBinding);

                // New tweets adorner

                var newTweetsBrush = new SolidColorBrush(NewTweetsIndicatorColor);
                newTweetsBrush.Freeze();

                _newTweetsAdorner = new TabItemNewTweetsAdorner(parent, newTweetsBrush);
                layer.Add(_newTweetsAdorner);

                var newTweetsBinding = new Binding
                {
                    Path   = new PropertyPath("NewTweetsIndicatorVisibility", pathParameters: Array.Empty<object>()),
                    Source = this,
                    Mode   = BindingMode.OneWay
                };

                _newTweetsAdorner.SetBinding(VisibilityProperty, newTweetsBinding);
            }
            else
            {
                var errorAdorner = _errorAdorner;
                if (errorAdorner is not null)
                {
                    AdornerLayer.GetAdornerLayer(parent)?.Remove(errorAdorner);
                    _errorAdorner = null;
                }

                var newTweetsAdorner = _newTweetsAdorner;
                if (newTweetsAdorner is not null)
                {
                    AdornerLayer.GetAdornerLayer(parent)?.Remove(newTweetsAdorner);
                    _newTweetsAdorner = null;
                }
            }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(TabItemHeaderWithIndicators),
                new PropertyMetadata(string.Empty));

        public Color ErrorIndicatorColor
        {
            get => (Color)GetValue(ErrorIndicatorColorProperty);
            set => SetValue(ErrorIndicatorColorProperty, value);
        }

        public static readonly DependencyProperty ErrorIndicatorColorProperty = DependencyProperty.Register(
            nameof(ErrorIndicatorColor),
            typeof(Color),
            typeof(TabItemHeaderWithIndicators),
            new PropertyMetadata(Colors.Crimson));

        public Visibility ErrorIndicatorVisibility
        {
            get => (Visibility)GetValue(ErrorIndicatorVisibilityProperty);
            set => SetValue(ErrorIndicatorVisibilityProperty, value);
        }

        public static readonly DependencyProperty ErrorIndicatorVisibilityProperty = DependencyProperty.Register(
            nameof(ErrorIndicatorVisibility),
            typeof(Visibility),
            typeof(TabItemHeaderWithIndicators),
            new PropertyMetadata(Visibility.Collapsed));

        public Color NewTweetsIndicatorColor
        {
            get => (Color)GetValue(NewTweetsIndicatorColorProperty);
            set => SetValue(NewTweetsIndicatorColorProperty, value);
        }

        public static readonly DependencyProperty NewTweetsIndicatorColorProperty = DependencyProperty.Register(
            nameof(NewTweetsIndicatorColor),
            typeof(Color),
            typeof(TabItemHeaderWithIndicators),
            new PropertyMetadata(Colors.DarkGreen));

        public Visibility NewTweetsIndicatorVisibility
        {
            get => (Visibility)GetValue(NewTweetsIndicatorVisibilityProperty);
            set => SetValue(NewTweetsIndicatorVisibilityProperty, value);
        }

        public static readonly DependencyProperty NewTweetsIndicatorVisibilityProperty = DependencyProperty.Register(
            nameof(NewTweetsIndicatorVisibility),
            typeof(Visibility),
            typeof(TabItemHeaderWithIndicators),
            new PropertyMetadata(Visibility.Collapsed));
    }
}