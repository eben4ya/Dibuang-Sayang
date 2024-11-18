using Appview.ViewModel;
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
            DataContext = new GetProductFromDB();
        }

        private void ReloadData()
        {
            // Function to reload data from database
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                // Make instance from this page
                var newPage = new AddProducts();

                // Set main window content to this page
                mainWindow.Content = newPage;
            }
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
        private void AddProductToDatabase(string productName, decimal price, int quantity, DateTime expiryDate, string description)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO product (productname, price, quantityavailable, expirationdate, description) VALUES (@name, @price, @quantity, @expiryDate, @description)", connection))
                    {
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@expiryDate", expiryDate);
                        command.Parameters.AddWithValue("@description", description);

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get data from the input fields
            string productName = txtNamaMenu.Text;
            decimal price;
            int quantity;
            string description = txtDeskripsi.Text;

            if (!decimal.TryParse(txtHarga.Text, out price))
            {
                MessageBox.Show("Harga harus berupa angka desimal yang valid.", "Kesalahan Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Kembali jika parsing gagal
            }

            if (!int.TryParse(txtStok.Text, out quantity))
            {
                MessageBox.Show("Stok harus berupa angka bulat yang valid.", "Kesalahan Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Kembali jika parsing gagal
            }

            if (datePickerTanggalKadaluarsa.SelectedDate == null)
            {
                MessageBox.Show("Silakan pilih tanggal kadaluarsa yang valid.", "Kesalahan Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Kembali jika tidak ada tanggal yang dipilih
            }

            DateTime expiryDate = datePickerTanggalKadaluarsa.SelectedDate.Value;  

            AddProductToDatabase(productName, price, quantity, expiryDate, description);

            // Reload page to refresh data
            ReloadData();
        }
    }
}
