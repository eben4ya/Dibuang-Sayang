using System;
using System.Collections.Generic;
using System.Linq;

namespace Appview.Models
{
    public class Admin : Person
    {
        private List<Product> _productList;

        public override void DisplayRole()
        {
            Console.WriteLine("This is an Admin");
        }

        public Admin()
        {
            _productList = new List<Product>();  
        }

 
        public void AddProduct(int id, string name, string description, decimal price, DateTime expiryDate, int quantityAvailable)
        {          
            Product newProduct = new Product(id, name, description, expiryDate, price, quantityAvailable );
            _productList.Add(newProduct);
            Console.WriteLine("Product added successfully: " + newProduct);
        }

        public void UpdateProduct(int id, string newName, decimal newPrice)
        {
            // Find the product in the list by ID
            Product productToUpdate = _productList.FirstOrDefault(p => p.ProductID == id);

            if (productToUpdate != null)
            {
                // Update the product details
                productToUpdate.ProductName = newName;
                productToUpdate.ProductPrice = newPrice;

                Console.WriteLine("Product updated successfully: " + productToUpdate);
            }
            else
            {
                Console.WriteLine("Product not found with ID: " + id);
            }
        }

        // Method to delete a product
        public void DeleteProduct(int id)
        {
            // Find the product in the list by ID
            Product productToRemove = _productList.FirstOrDefault(p => p.ProductID == id);

            if (productToRemove != null)
            {
                // Remove the product from the list
                _productList.Remove(productToRemove);

                Console.WriteLine("Product deleted successfully: " + productToRemove);
            }
            else
            {
                Console.WriteLine("Product not found with ID: " + id);
            }
        }

        // Method to list all products
        public void ListProducts()
        {
            if (_productList.Count > 0)
            {
                Console.WriteLine("Listing all products:");
                foreach (var product in _productList)
                {
                    Console.WriteLine(product);
                }
            }
            else
            {
                Console.WriteLine("No products available.");
            }
        }
    }
}
