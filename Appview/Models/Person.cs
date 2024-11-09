using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appview.Models
{
    public class Person
    {
        //Implementasi "Encapsulation" dengan menggunakan getter dan setter
        public string Name { get; set; }
        public string Email { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}, Email: {Email}");
        }

        public virtual void DisplayRole()
        {
            Console.WriteLine("This is a person");
        }
    }
}
