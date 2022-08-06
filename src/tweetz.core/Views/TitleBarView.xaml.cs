﻿using System.Windows.Controls;
using System.Windows.Input;
using tweetz.core.Commands;
using tweetz.core.ViewModels;

namespace tweetz.core.Views
{
    public partial class TitleBarView : UserControl
    {
        public TitleBarView()
        {
            DataContext = App.ServiceProvider.GetService<TitleBarControlViewModel>();
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