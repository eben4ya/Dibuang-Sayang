using Appview.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class FoodDetails : UserControl
    {
        private int _productId;
        private int _stock;
        private string _productName;
        private string _productDesc;
        private BitmapImage _productImage;
        public BitmapImage ProductImage
        {
            get => _productImage;
            set
            {
                _productImage = value;
            }
        }
        public FoodDetails(int productId, string Title, decimal Price, int Stock, DateTime Expired, string Description, BitmapImage ProductImage)
        {
            InitializeComponent();
            //DataContext = new GetProductFromDB();
            DataContext = this;

            _productId = productId;
            _productName = Title;
            _productDesc = Description;
            ContentTitle.Text = Title;
            ContentDescription.Text = Description;
            ContentPrice.Text = Price.ToString();
            ContentStock.Text = Stock.ToString();
            ContentExpiry.Text = Expired.ToString();
            _productImage = ProductImage;
            _stock = Stock;
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            // Parse the current value from the TextBlock
            if (int.TryParse(QuantityTextBlock.Text, out int currentQuantity))
            {
                // Ensure the quantity does not go below 0
                if (currentQuantity > 0)
                {
                    currentQuantity--;
                    QuantityTextBlock.Text = currentQuantity.ToString();
                }
            }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            // Parse the current value from the TextBlock
            if (int.TryParse(QuantityTextBlock.Text, out int currentQuantity))
            {
                currentQuantity++; // Increment the value
                QuantityTextBlock.Text = currentQuantity.ToString();
            }
        }
        private void Pesan_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBlock.Text, out int quantity) && decimal.TryParse(ContentPrice.Text, out decimal price))
            {
                //MessageBox.Show("Button clicked successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                // Create an instance of FoodDetails page
                var paymentPage = new Payment(quantity, price, _productId, _productName, _productDesc, ProductImage);

                // Get the main window and set its content to the new page
                var mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.Content = paymentPage;
                }
            }   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var recommendedFoodPage = new RecommendedFood();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = recommendedFoodPage;
            }

        }
    }
}
