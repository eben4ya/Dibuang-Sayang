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
                    using (var command = new NpgsqlCommand("SELECT product_id, productname, price, quantityavailable, expirationdate, description FROM product", connection))
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

                                var newProduct = new Product(productId, productName, expiryDate, price, quantity, description);

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
