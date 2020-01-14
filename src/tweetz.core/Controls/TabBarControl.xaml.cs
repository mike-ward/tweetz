using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class TabBarControl : UserControl
    {
        public TabBarControl()
        {
            InitializeComponent();
        }

        private TabBarControlViewModel ViewModel => (TabBarControlViewModel)DataContext;

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ViewModel.ShowComposeControl = false;
                TabControl.SelectedIndex = 0;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // there's no uniform size option in tabcontrol.
            var width = e.NewSize.Width / TabControl.Items.Count - 1;
            ViewModel.TabWidth = width;
        }

        /// <summary>
        /// Hocus pocus, try to set the focus when switching tabs so page up/dn, home/end
        /// keyboard shortcuts for scrolling work.
        /// </summary>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = TabControl.Items[TabControl.SelectedIndex] as HeaderedContentControl;
            var timeline = item?.Content as TimelineControl;

            if (timeline?.FindName("ItemsControl") is ItemsControl itemsControl)
            {
                if (VisualTreeHelper.GetChildrenCount(itemsControl) > 0)
                {
                    if (VisualTreeHelper.GetChild(itemsControl, 0) is ScrollViewer scrollViewer)
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => scrollViewer.Focus()));
                    }
                }
            }
        }
    }
}