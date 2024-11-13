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
                    using (var command = new NpgsqlCommand("SELECT productname, price, quantityavailable, expirationdate FROM product", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string productName = reader["productname"].ToString();
                                decimal price = (decimal)reader["price"];
                                int quantity = (int)reader["quantityavailable"];
                                DateTime expiryDate = (DateTime)reader["expirationdate"];

                                var newProduct = new Product(productName, expiryDate, price, quantity);

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
