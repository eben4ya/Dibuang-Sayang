using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appview.Models
{

	public class Product
	{
		//Implementasi "Encapsulation" dengan menggunakan getter dan setter
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public DateTime ExpiryDate { get; set; }
		public decimal ProductPrice { get; set; }
		public int QuantityAvailable { get; set; }

		// Constructor
		public Product(int productID, string name, string description, DateTime expiryDate, decimal price, int quantityAvailable)
		{
			ProductID = productID;
            ProductName = name;
			Description = description;
			ExpiryDate = expiryDate;
            ProductPrice = price;
			QuantityAvailable = quantityAvailable;
		}

		public void UpdateProductDetails(string name, string description, DateTime expiryDate, decimal price, int quantityAvailable)
		{
			ProductName = name;
			Description = description;
			ExpiryDate = expiryDate;
			ProductPrice = price;
			QuantityAvailable = quantityAvailable;
			Console.WriteLine("Product details updated!");
		}

		public bool CheckAvailability()
		{
			Console.WriteLine("Checking Availability");
			return QuantityAvailable > 0;
		}

		public bool ReserveProduct()
		{
			if (CheckAvailability())
			{
				QuantityAvailable--;
				Console.WriteLine($"Product reserved. Quantity left: {QuantityAvailable}");
				return true;
			}
			else
			{
				Console.WriteLine("Product is out of stock");
				return false;
			}
		}
	}
}
