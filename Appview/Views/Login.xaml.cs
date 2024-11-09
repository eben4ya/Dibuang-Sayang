using Appview.Data;
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

            if (db.AuthenticateUser(username, password))
            {
                var recommendedFood = new RecommendedFood();
                var mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.Content = recommendedFood;
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }
    }
}
