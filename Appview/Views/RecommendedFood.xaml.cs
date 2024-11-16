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
using static Appview.ViewModel.UserSession;

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for RecommendedFood.xaml
    /// </summary>
    public partial class RecommendedFood : UserControl
    {
        public RecommendedFood()
        {
            InitializeComponent();
            int userId = UserSession.UserId;

            var viewModel = new CompositeViewModel
            {
                ProductViewModel = new GetProductFromDB(),
                OrderViewModel = new GetOrdersFromDB(userId)
            };
            //DataContext = new GetProductFromDB();
            DataContext = viewModel;
        }

        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new ListPemesanan();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;

            var selectedData = border?.DataContext as Product;

            var foodDetailsPage = new FoodDetails(
                selectedData.ProductId,
                selectedData.ProductName,
                selectedData.ProductPrice,
                selectedData.QuantityAvailable,
                selectedData.ExpiryDate,
                selectedData.ProductDescription
                );  // Instantiate the FoodDetails page
            var mainWindow = Application.Current.MainWindow as MainWindow;  // Get reference to MainWindow

            if (mainWindow != null)
            {
                mainWindow.Content = foodDetailsPage;  // Navigate to FoodDetails page
            }
        }

    }
    public class CompositeViewModel
    {
        public GetProductFromDB ProductViewModel { get; set; }
        public GetOrdersFromDB OrderViewModel { get; set; }
    }
}
