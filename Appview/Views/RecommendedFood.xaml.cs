using Appview.Models;
using Appview.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class RecommendedFood : UserControl
    {
        public RecommendedFood()
        {
            InitializeComponent();
            int userId = UserSession.UserId;

            var viewModel = new CompositeViewModelRecommendedFood
            {
                ProductViewModel = new GetProductFromDB(),
                OrderViewModel = new GetOrdersFromDB(userId)
            };

            viewModel.InitializeFilteredProducts();
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
                selectedData.ProductDescription,
                selectedData.ProductImage
                );  // Instantiate the FoodDetails page
            var mainWindow = Application.Current.MainWindow as MainWindow;  // Get reference to MainWindow

            if (mainWindow != null)
            {
                mainWindow.Content = foodDetailsPage;  // Navigate to FoodDetails page
            }
        }

    }
    public class CompositeViewModelRecommendedFood : INotifyPropertyChanged
    {
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                FilterProducts();
            }
        }
        public GetProductFromDB ProductViewModel { get; set; }
        public GetOrdersFromDB OrderViewModel { get; set; }

        private ObservableCollection<Product> _filteredProducts;
        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            private set
            {
                _filteredProducts = value;
                OnPropertyChanged();
            }
        }
        public CompositeViewModelRecommendedFood()
        {
            //FilteredProducts = new ObservableCollection<Product>(ProductViewModel.Products);
        }

        public void InitializeFilteredProducts()
        {
            if (ProductViewModel?.Products != null)
            {
                FilteredProducts = new ObservableCollection<Product>(ProductViewModel.Products);
            }
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredProducts = new ObservableCollection<Product>(ProductViewModel.Products);
            }
            else
            {
                var filtered = ProductViewModel.Products
                    .Where(p => p.ProductName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilteredProducts = new ObservableCollection<Product>(filtered);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
