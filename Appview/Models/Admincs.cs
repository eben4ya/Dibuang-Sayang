using System;
using System.Collections.Generic;
using System.Linq;

namespace Appview.Models
{
    // Product class to represent a product object
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Price: {Price:C}";
        }
    }

    public class Admin
    {
        private List<Product> _productList;  

        public Admin()
        {
            _productList = new List<Product>();  
        }

 
        public void AddProduct(int id, string name, decimal price)
        {          
            Product newProduct = new Product { Id = id, Name = name, Price = price };
            _productList.Add(newProduct);
            Console.WriteLine("Product added successfully: " + newProduct);
        }

        public void UpdateProduct(int id, string newName, decimal newPrice)
        {
            // Find the product in the list by ID
            Product productToUpdate = _productList.FirstOrDefault(p => p.Id == id);

            if (productToUpdate != null)
            {
                // Update the product details
                productToUpdate.Name = newName;
                productToUpdate.Price = newPrice;

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
            Product productToRemove = _productList.FirstOrDefault(p => p.Id == id);

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
