using Appview.Data;
using Appview.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Appview.ViewModel.UserSession;

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Navigate to registration page!");
            var registerPage = new Register();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = registerPage;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var db = new Database();
            string username = txtUsername.Text; // Replace with the actual name of the Username TextBox
            string password = txtPassword.Text; // Replace with the actual name of the Password TextBox
            bool isHotel = radioHotel.IsChecked == true;

            if (!radioCustomer.IsChecked == true && !isHotel)
            {
                MessageBox.Show("Please select Customer or Hotel.");
                return;
            }


            if (isHotel)
            {
                // Login as Hotel Admin
                if (db.AuthenticateHotel(username, password))
                {
                    var hotelDashboard = new TodayOrder();
                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.Content = hotelDashboard;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid hotel username or password.");
                }
            }
            else
            {
                // Login as Customer
                //var userId = db.AuthenticateUser(username, password);
                if (db.AuthenticateUser(username, password))
                {
                    int userId = db.ValidateUser(username, password);
                    UserSession.UserId = userId;

                    var recommendedFoodPage = new RecommendedFood();
                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.Content = recommendedFoodPage;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }
    }
}
