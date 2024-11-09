using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Appview.Utils
{
    //internal class NavigationService
    public static class NavigationService
    {
        private static ContentControl _mainContent;

        // Initialize with MainWindows's ContentControl
        public static void Initialize(ContentControl mainContent)
        {
            _mainContent = mainContent;
        }

        // Method to navigate between view
        public static void Navigate(UserControl newPage)
        {
            _mainContent = newPage;
        }
    }
}
