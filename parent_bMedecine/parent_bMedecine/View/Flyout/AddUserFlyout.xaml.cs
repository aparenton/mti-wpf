﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace parent_bMedecine.View.Flyout
{
    /// <summary>
    /// Logique d'interaction pour AddUserFlyout.xaml
    /// </summary>
    public partial class AddUserFlyout : UserControl
    {
        public AddUserFlyout()
        {
            InitializeComponent();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
            parentWindow.ToggleFlyout(0);
        }
    }
}