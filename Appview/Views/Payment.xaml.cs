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
using Appview.Data;
using Appview.ViewModel;
using static Appview.ViewModel.UserSession;

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : UserControl
    {
        private int _productId;
        public Payment(int quantity, decimal price, int productId)
        {
            InitializeComponent();

            _productId = productId;

            // Store user_id if needed for later use
            //this.userId = userId;

            //Calculate the total payment
            decimal totalPayment = quantity * price;

            //Update the UI with the passed data
            QuantityTextBlock.Text = $"{quantity} Pcs";
            TotalPaymentTextBlock.Text = $"Rp{totalPayment:N0}";
        }

        private void BayarSekarangButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = UserSession.UserId;

            int quantity = int.Parse(QuantityTextBlock.Text.Split(' ')[0]);
            decimal totalPayment = decimal.Parse(TotalPaymentTextBlock.Text.Replace("Rp", "").Replace(",", ""));

            // Insert reservation into the dataabse
            var db = new Database();
            bool isInserted = db.InsertReservation(userId, quantity, totalPayment, _productId);

            if (isInserted)
            {
                MessageBox.Show("Payment successful. Redirecting...");

                // Navigate back to FoodRecommendation page
                var recommendedFoodPage = new RecommendedFood();
                var mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.Content = recommendedFoodPage;
                }
            }
            
            else
            {
                MessageBox.Show("Payment failed. Please try again.");
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
