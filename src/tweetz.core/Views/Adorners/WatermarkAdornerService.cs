using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace tweetz.core.Views.Adorners
{
    public static class WatermarkAdornerService
    {
#nullable disable

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
           "Watermark",
           typeof(object),
           typeof(WatermarkAdornerService),
           new FrameworkPropertyMetadata(defaultValue: null, new PropertyChangedCallback(OnWatermarkChanged)));

        private static readonly Dictionary<object, ItemsControl> itemsControls = new Dictionary<object, ItemsControl>();

        public static object GetWatermark(DependencyObject d)
        {
            if (d is null) throw new ArgumentNullException(nameof(d));
            return d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject d, object value)
        {
            if (d is null) throw new ArgumentNullException(nameof(d));
            d.SetValue(WatermarkProperty, value);
        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Control)d;
            control.Loaded -= Control_Loaded;
            control.Loaded += Control_Loaded;

            if (d is ComboBox)
            {
                control.GotKeyboardFocus -= Control_GotKeyboardFocus;
                control.GotKeyboardFocus += Control_GotKeyboardFocus;

                control.LostKeyboardFocus -= Control_Loaded;
                control.LostKeyboardFocus += Control_Loaded;
            }
            else if (d is TextBox)
            {
                control.GotKeyboardFocus -= Control_GotKeyboardFocus;
                control.GotKeyboardFocus += Control_GotKeyboardFocus;

                control.LostKeyboardFocus -= Control_Loaded;
                control.LostKeyboardFocus += Control_Loaded;
                ((TextBox)control).TextChanged += Control_GotKeyboardFocus;
            }
            else if (d is ItemsControl itemControl && d is not ComboBox)
            {
                var i = itemControl;

                // for Items property
                i.ItemContainerGenerator.ItemsChanged += ItemsChanged;
                itemsControls.Add(i.ItemContainerGenerator, i);

                // for ItemsSource property
                var prop = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, i.GetType());
                prop.AddValueChanged(i, ItemsSourceChanged);
            }
        }

        private static void Control_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            var c = (Control)sender;
            if (ShouldShowWatermark(c))
            {
                ShowWatermark(c);
            }
            else
            {
                RemoveWatermark(c);
            }
        }

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            var control = (Control)sender;
            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
        }

        private static void ItemsSourceChanged(object sender, EventArgs e)
        {
            var c = (ItemsControl)sender;
            if (c.ItemsSource is null || ShouldShowWatermark(c))
            {
                ShowWatermark(c);
            }
            else
            {
                RemoveWatermark(c);
            }
        }

        private static void ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (itemsControls.TryGetValue(sender, out var control))
            {
                if (ShouldShowWatermark(control))
                {
                    ShowWatermark(control);
                }
                else
                {
                    RemoveWatermark(control);
                }
            }
        }

        private static void RemoveWatermark(UIElement control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);

            // layer could be null if control is no longer in the visual tree
            if (layer is not null)
            {
                var adorners = layer.GetAdorners(control);
                if (adorners is null)
                {
                    return;
                }

                foreach (var adorner in adorners)
                {
                    if (adorner is WatermarkAdorner)
                    {
                        adorner.Visibility = Visibility.Hidden;
                        layer.Remove(adorner);
                    }
                }
            }
        }

        private static void ShowWatermark(Control control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);

            // layer could be null if control is no longer in the visual tree
            if (layer is not null)
            {
                layer.Add(new WatermarkAdorner(control, GetWatermark(control)));
            }
        }

        private static bool ShouldShowWatermark(Control c)
        {
            return c switch
            {
                ComboBox _ => string.IsNullOrEmpty((c as ComboBox)?.Text),
                TextBoxBase _ => string.IsNullOrEmpty((c as TextBox)?.Text),
                ItemsControl _ => (c as ItemsControl)?.Items.Count == 0,
                _ => false,
            };
        }
    }
}