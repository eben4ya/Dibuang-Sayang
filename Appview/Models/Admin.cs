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
    }
}
