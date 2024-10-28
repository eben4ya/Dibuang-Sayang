using System.Windows.Controls;
using System.Windows;

namespace Appview.Utils
{
    public static class Placeholder
    {
        public static readonly DependencyProperty PlaceholderProperty =
           DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(Placeholder), new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static string GetPlaceholder(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderProperty);
        }

        public static void SetPlaceholder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderProperty, value);
        }

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus += (sender, args) =>
                {
                    if (textBox.Text == (string)e.NewValue)
                    {
                        textBox.Text = string.Empty;
                        textBox.Foreground = System.Windows.Media.Brushes.White;
                    }
                };

                textBox.LostFocus += (sender, args) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = (string)e.NewValue;
                        textBox.Foreground = System.Windows.Media.Brushes.White;
                    }
                };

                // Initial placeholder
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = (string)e.NewValue;
                    textBox.Foreground = System.Windows.Media.Brushes.White;
                }
            }
        }
    }
}
