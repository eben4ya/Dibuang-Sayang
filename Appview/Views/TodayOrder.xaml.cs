using Appview.Models;
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
            var viewModel = new CompositeViewModelTodayOrder
            {
               OrderViewModel = new GetOrdersFromDB(),
               UpdateOrderStatus = new UpdateOrder()

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

        private void Button_Click_Done(object sender, RoutedEventArgs e)
        {
            // Mengambil DataContext dari CompositeViewModel
            var viewModel = DataContext as CompositeViewModelTodayOrder;
            if (viewModel == null)
            {
                MessageBox.Show("Failed to get the view model.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Mengambil reservation yang terhubung dengan tombol
            var button = sender as Button;
            if (button != null)
            {
                var reservation = button.DataContext as Reservation;

                if (reservation != null)
                {
                    // Menggunakan UpdateOrderStatus dari viewModel untuk memperbarui status pesanan
                    viewModel.UpdateOrderStatus.UpdateOrderStatus(reservation.ReservationID, "Dikonfirmasi");
                    MessageBox.Show($"Order ID {reservation.ReservationID} has been confirmed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to get order information!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}

public class CompositeViewModelTodayOrder
{
    public GetOrdersFromDB OrderViewModel { get; set; }
    public UpdateOrder UpdateOrderStatus { get; set; }

}
