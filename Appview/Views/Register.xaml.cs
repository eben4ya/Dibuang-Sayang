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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }

        private void GoToLoginPage(object sender, RequestNavigateEventArgs e)
        {
            // Membuat instance dari Login UserControl
            var loginComponent = new Login();

            // Membuat jendela baru untuk menampilkan Login UserControl
            var loginWindow = new Window
            {
                Content = loginComponent,
                Height = 450,
                Width = 800,
                Title = "Login",
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            loginWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Navigate to registration page!");
            var loginPage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var paymentLogin = new Payment();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = paymentLogin;
            }
        }
    }
}
