﻿using Appview.Models;
using Appview.ViewModel;
using System;
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

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for TodayOrder.xaml
    /// </summary>
    public partial class TodayOrder : UserControl
    {
        public TodayOrder()
        {
            InitializeComponent();
            var viewModel = new CompositeViewModel
            {
                OrderViewModel = new GetOrdersFromDB()
            };
            //DataContext = new GetProductFromDB();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var loginPage = new AddProducts();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }
    }
}
