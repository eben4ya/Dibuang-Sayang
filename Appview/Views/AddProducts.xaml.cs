using Appview.ViewModel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    public partial class AddProducts : UserControl

    {   // save edit product id
        private int? editProductId = null;
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
        private void AddProductToDatabase(string productName, decimal price, int quantity, DateTime expiryDate, string description, byte[] imageData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "INSERT INTO product (productname, price, quantityavailable, expirationdate, description, image_data) VALUES (@name, @price, @quantity, @expiryDate, @description, @imageData)"
                        , connection))
                    {
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@expiryDate", expiryDate);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@imageData", (object?)imageData ?? DBNull.Value);

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

        private void TambahMenu_Click(object sender, RoutedEventArgs e)
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

            if (selectedImageData == null)
            {
                MessageBox.Show("Silakan pilih gambar produk.", "Kesalahan Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Kembali jika gambar belum dipilih
            }

            if (editProductId.HasValue)
            {
                EditProductInDatabase(editProductId.Value, productName, price, quantity, expiryDate, description, selectedImageData);
                btnAddOrEdit.Content = "Tambahkan Menu";
                editProductId = null;
            }
            else
            {
                AddProductToDatabase(productName, price, quantity, expiryDate, description, selectedImageData);
            }

            // Reload page to refresh data
            ReloadData();
        }

        private byte[] selectedImageData;

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select an image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Load the image into a byte array
                selectedImageData = File.ReadAllBytes(selectedFilePath);

                // Preview the image
                var imageSource = new BitmapImage(new Uri(selectedFilePath));
                var imagePreview = new Image { Source = imageSource, Height = 120, Width = 120, Margin = new Thickness(5) };

                // Display the image preview on the UI
                imgPreview.Source = imageSource;
            }
        }

        private void ButtonKembali_Click(object sender, RoutedEventArgs e)
        {
            var todayOrderPage = new TodayOrder();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = todayOrderPage;
            }
        }

        private void DeleteProductFromDatabase(int productId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("DELETE FROM product WHERE product_id = @productId", connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product deleted successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int productId)
            {
                DeleteProductFromDatabase(productId);

                // Reload page to refresh data
                ReloadData();
            }
        }

        // Edit product in the database
        private void EditProductInDatabase(int productId, string productName, decimal price, int quantity, DateTime expiryDate, string description, byte[] imageData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(
                        "UPDATE product SET productname=@name, price=@price, quantityavailable=@quantity, expirationdate=@expiryDate, description=@description, image_data=@imageData WHERE product_id=@productId"
                        , connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@expiryDate", expiryDate);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@imageData", (object?)imageData ?? DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button button && button.CommandParameter is int productId)
            {
                // Load product data from database
                string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM product WHERE product_id = @productId", connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtNamaMenu.Text = reader["productname"].ToString();
                                txtHarga.Text = reader["price"].ToString();
                                txtStok.Text = reader["quantityavailable"].ToString();
                                datePickerTanggalKadaluarsa.SelectedDate = Convert.ToDateTime(reader["expirationdate"]);
                                txtDeskripsi.Text = reader["description"].ToString();

                                editProductId = productId;
                                btnAddOrEdit.Content = "Edit Menu";
                            }
                        }
                    }
                }
            }

        }
    }
}
