using Npgsql;
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
    /// <summary>
    /// Interaction logic for AddProducts.xaml
    /// </summary>
    public partial class AddProducts : UserControl
    {
        public AddProducts()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new TodayOrder();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        //Add product to db
        private void AddProductToDatabase(string productName, decimal price, int quantity, DateTime expiryDate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO product (product_name, description, price, quantity_available, expiry_date) VALUES (@name, @desc, @price, @quantity, @expiryDate)", connection))
                    {
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@expiryDate", expiryDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product added successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to add product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
