﻿using System.Windows.Controls;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            DataContext = App.ServiceProvider.GetService<MainViewModel>();
            InitializeComponent();
        }
    }
}