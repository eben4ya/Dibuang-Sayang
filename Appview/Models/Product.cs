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
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public DateTime ExpiryDate { get; set; }
		public decimal ProductPrice { get; set; }
		public int QuantityAvailable { get; set; }
		public string ProductDescription { get; set; }

		// Constructor
		public Product(int productId, string name, DateTime expiryDate, decimal price, int quantityAvailable, string productDescription)
		{	
			ProductId = productId;
            ProductName = name;
			ExpiryDate = expiryDate;
            ProductPrice = price;
			QuantityAvailable = quantityAvailable;
			ProductDescription = productDescription;
		}

		public void UpdateProductDetails(string name, DateTime expiryDate, decimal price, int quantityAvailable, string productDescription)
		{
			ProductName = name;
			ExpiryDate = expiryDate;
			ProductPrice = price;
			QuantityAvailable = quantityAvailable;
			ProductDescription = productDescription;
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
