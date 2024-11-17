using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Appview.Services
{
    public class WeatherService
    {
        private readonly string apiKey = "a394145349a7323a58c762a69910fdfd"; // API key from OpenWeather
        private readonly string city = "Yogyakarta"; // specific city

        public async Task GetWeatherNotificationAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(result);

                        string weather = json["weather"][0]["main"].ToString();

                        switch (weather)
                        {
                            case "Clouds":
                                MessageBox.Show("Cuaca mendung, sebaiknya jaga-jaga bawa payung.", "Weather Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case "Rain":
                            case "Thunderstorm":
                                MessageBox.Show("Cuaca hujan atau badai, pastikan bawa payung!", "Weather Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            default:
                                MessageBox.Show("Cuaca cerah, have a good day!", "Weather Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Gagal mendapatkan data cuaca. Silakan coba lagi nanti.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
