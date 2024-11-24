using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using Appview.Models;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Printing;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO;

namespace Appview.ViewModel
{
    public class GetProductFromDB 
    {
        public ObservableCollection<Product> Products { get; set; }

        public GetProductFromDB()
        {
            Products = new ObservableCollection<Product>();
            LoadProductsFromDatabase();
        }

        private void LoadProductsFromDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT product_id, productname, price, quantityavailable, expirationdate, description, image_data FROM product", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productId = (int)reader["product_id"];
                                string productName = reader["productname"].ToString();
                                decimal price = (decimal)reader["price"];
                                int quantity = (int)reader["quantityavailable"];
                                DateTime expiryDate = (DateTime)reader["expirationdate"];
                                string description = reader["description"].ToString();

                                BitmapImage productImage = null;
                                if (reader["image_data"] != DBNull.Value)
                                {
                                    productImage = ConvertToBitmapImage((byte[])reader["image_data"]);
                                }
                                else
                                {
                                    productImage = LoadPlaceHolderImage();
                                }

                                var newProduct = new Product(productId, productName, expiryDate, price, quantity, description, productImage);

                                Products.Add(newProduct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors here
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BitmapImage LoadPlaceHolderImage()
        {
            BitmapImage placeholder = null;

            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT image_data FROM static_images WHERE image_name = @imageName", connection))
                    {
                        command.Parameters.AddWithValue("@imageName", "food_icon");

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read() && reader["image_data"] != DBNull.Value)
                            {
                                placeholder = ConvertToBitmapImage((byte[])reader["image_data"]);
                            }
                        }
                    }
                }

                // If no image found in database, handle it gracefully
                if (placeholder == null)
                {
                    MessageBox.Show("Placeholder image not found in database. Using fallback default.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    placeholder = new BitmapImage();
                    placeholder.BeginInit();
                    placeholder.UriSource = new Uri("https://i.ibb.co.com/XJNy0Gz/food-icon.jpg");
                    placeholder.CacheOption = BitmapCacheOption.OnLoad;
                    placeholder.EndInit();
                }
            }
            catch (Exception ex)
            {
                // Handle database connection or query errors
                MessageBox.Show($"Failed to load placeholder image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                placeholder = new BitmapImage();
                placeholder.BeginInit();
                placeholder.UriSource = new Uri("https://i.ibb.co.com/XJNy0Gz/food-icon.jpg");
                placeholder.CacheOption = BitmapCacheOption.OnLoad;
                placeholder.EndInit();
            }

            return placeholder;
        }

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null | imageData.Length == 0) return null;

            using (var stream = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();

                return image;
            }
        }

        public static Product FindById (ObservableCollection<Product> Products, int id)
        {
            int maxLen = Products.Count;

            // find from Products where productId = id
            for (int i = 0; i + 1 == maxLen; i++)
            {
                if (Products[i].ProductId == id)
                    return Products[i];
            };

            // return error
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
