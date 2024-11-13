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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Appview.Views
{
    /// <summary>
    /// Interaction logic for RecommendedFood.xaml
    /// </summary>
    public partial class RecommendedFood : UserControl
    {
        public RecommendedFood()
        {
            InitializeComponent();
            DataContext = new GetProductFromDB();
        }

        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }

        public void FoodButton_Click1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            // Create an instance of FoodDetails page
            var foodDetailsPage = new FoodDetails();

            // Get the main window and set its content to the new page
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = foodDetailsPage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new ListPemesanan();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.Content = loginPage;
            }
        }
    }
}
