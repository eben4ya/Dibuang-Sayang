﻿using Appview.ViewModel;
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
    /// Interaction logic for FoodDetails.xaml
    /// </summary>
    public partial class FoodDetails : UserControl
    {
        public FoodDetails(string Title, decimal Price)
        {
            InitializeComponent();
            DataContext = new GetProductFromDB();
            ContentTitle.Text = Title;
            ContentPrice.Text = Price.ToString();
        }

        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            // Create an instance of FoodDetails page
            var paymentPage = new Payment();

            // Get the main window and set its content to the new page
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = paymentPage;
            }
        }
    }
}
