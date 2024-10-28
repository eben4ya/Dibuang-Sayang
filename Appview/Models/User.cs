using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appview.Models
{
    public class User
    {
        public int userID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        private bool isLoggedIn = false;
        private List<string> reservations = new List<string>();


        public void Register()
        {
            // Logika untuk melakukan register user baru (menghubungkan database atau API)
            Console.WriteLine("User has been registered.");

        }
        public void Login()
        {
            if (!isLoggedIn)
            {
                // Logika untuk autentikasi credential user 
                Console.WriteLine("User logged in successfully.");
                isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("User is already logged in.");
            }
        }

        public void Logout()
        {
            if (isLoggedIn)
            {
                // Logika untuk user logout              
                Console.WriteLine("User logged out successfully.");
                isLoggedIn = false;
            }
            else
            {
                Console.WriteLine("User is not logged in.");
            }
        }

        public void ViewReservation()
        {
            if (isLoggedIn)
            {
                // Logika untuk fetch data reservation
                // untuk sekarang masih berbentuk list dummy 
                Console.WriteLine("Fetching reservations for user...");
                foreach (var reservation in reservations)
                {
                    Console.WriteLine($"Reservation: {reservation}");
                }
            }
            else
            {
                Console.WriteLine("User must be logged in to view reservations.");
            }
        }

    }
}
