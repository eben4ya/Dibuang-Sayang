using Appview.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            //string password = txtPassword.Password;
            string confirmPassword = txtPasswordConfirm.Text;
            //string confirmPassword = txtPasswordConfirm.Password;
            bool isHotel = radioHotel.IsChecked == true; 

            // Basic validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!radioCustomer.IsChecked == true && !isHotel)
            {
                MessageBox.Show("Please select Customer or Hotel.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            var db = new Database();
            bool isRegistered;

            if (isHotel)
            {
                // Register as Hotel Admin
                isRegistered = db.RegisterHotel(username, email, password);
            }
            else
            {
                // Register as Customer
                isRegistered = db.RegisterUser(username, email, password);
            }

            if (isRegistered)
            {
                MessageBox.Show("Registration successful!");
                // Optionally navigate back to the login page
                var loginPage = new Login();
                var mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.Content = loginPage;
                }
            }
            else
            {
                MessageBox.Show("Registration failed. Try again.");
            }
        }

        public BitmapImage LoadDefaultLogo()
        {
            BitmapImage placeholder = null;

            string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT image_data FROM static_images WHERE image_name = @imageName", connection))
                    {
                        command.Parameters.AddWithValue("@imageName", "default_image");

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read() && reader["image_data"] != DBNull.Value)
                            {
                                placeholder = ConvertToBitmapImage((byte[])reader["image_data"]);
                                logoImageControl.Source = placeholder;
                            }
                        }
                    }
                }

                // If no image found in database, handle it gracefully
                if (placeholder == null)
                {
                    MessageBox.Show("Placeholder image not found in database. Using fallback default.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    placeholder = new BitmapImage();
                    placeholder.BeginInit();
                    placeholder.UriSource = new Uri("https://i.ibb.co.com/XJNy0Gz/food-icon.jpg");
                    placeholder.CacheOption = BitmapCacheOption.OnLoad;
                    placeholder.EndInit();
                    logoImageControl.Source = placeholder;
                }
            }
            catch (Exception ex)
            {
                // Handle database connection or query errors
                MessageBox.Show($"Failed to load placeholder image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                placeholder = new BitmapImage();
                placeholder.BeginInit();
                placeholder.UriSource = new Uri("https://i.ibb.co.com/XJNy0Gz/food-icon.jpg");
                placeholder.CacheOption = BitmapCacheOption.OnLoad;
                placeholder.EndInit();
                logoImageControl.Source = placeholder;

            }

            return placeholder;
        }
        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null | imageData.Length == 0) return null;

            using (var stream = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();

                return image;
            }
        }
    }
}
