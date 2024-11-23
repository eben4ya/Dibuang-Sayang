using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;
using System.Windows;
using System.Collections.ObjectModel;
using Appview.Models;
using static Appview.ViewModel.UserSession;
using System.Windows.Media.Imaging;
using System.IO;

namespace Appview.ViewModel
{
    public class GetOrdersFromDB
    {
        string connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

        public ObservableCollection<Reservation> Orders { get; set; }

        // Constructor to get orders for a specific user
        public GetOrdersFromDB(int userId)
        {
            Orders = new ObservableCollection<Reservation>();
            FetchOrders(userId);
        }

        // Constructor to get all orders
        public GetOrdersFromDB()
        {
            Orders = new ObservableCollection<Reservation>();
            FetchAllOrders();
        }

        // Fetch orders for a specific user
        private void FetchOrders(int userId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT 
                        r.reservation_id,
                        r.user_id,
                        r.product_id,
                        r.reservationdate,
                        r.status,
                        r.hotel_id,
                        r.amount_pcs,
                        r.total_price,
                        h.hotel_name,
                        p.productname,
                        p.image_data
                    FROM 
                        reservation r
                    JOIN 
                        apps_hotel_modified h ON r.hotel_id = h.hotel_id
                    JOIN 
                        product p ON r.product_id = p.product_id
                    WHERE 
                        r.user_id = @UserId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BitmapImage productImage = null;
                            if (reader["image_data"] != DBNull.Value)
                            {
                                productImage = ConvertToBitmapImage((byte[])reader["image_data"]);
                            }
                            else
                            {
                                productImage = LoadPlaceHolderImage();
                            }
                            Orders.Add(new Reservation
                            {
                                ReservationID = (int)reader["reservation_id"],
                                UserID = (int)reader["user_id"],
                                ProductID = (int)reader["product_id"],
                                ReservationDate = Convert.ToDateTime(reader["reservationdate"]),
                                Status = reader["status"].ToString(),
                                HotelID = (int)reader["hotel_id"],
                                AmountPcs = (int)reader["amount_pcs"],
                                TotalPrice = (decimal)reader["total_price"],
                                HotelName = reader["hotel_name"].ToString(),
                                ProductName = reader["productname"].ToString(),
                                ProductImage = productImage
                            });
                        }
                    }
                }
            }
        }
        private BitmapImage LoadPlaceHolderImage()
        {
            var placeholder = new BitmapImage();
            placeholder.BeginInit();
            placeholder.UriSource = new Uri("https://i.ibb.co.com/pnRxBcK/food-icon.jpg");
            placeholder.CacheOption = BitmapCacheOption.OnLoad;
            placeholder.EndInit();

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

        // Fetch all orders without filtering by user
        private void FetchAllOrders()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

            //  Added new username column & filter only display order with status not confirmed
                var query = @"
            SELECT 
                r.reservation_id,
                r.user_id,
                r.product_id,
                r.reservationdate,
                r.status,
                r.hotel_id,
                r.amount_pcs,
                r.total_price,
                h.hotel_name,
                p.productname,
                p.image_data,
                u.username  
            FROM 
                reservation r
            JOIN 
                apps_hotel_modified h ON r.hotel_id = h.hotel_id
            JOIN 
                product p ON r.product_id = p.product_id
            JOIN 
                apps_user_modified u ON r.user_id = u.user_id
            WHERE
                 r.status = 'Menunggu'";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BitmapImage productImage = null;
                            if (reader["image_data"] != DBNull.Value)
                            {
                                productImage = ConvertToBitmapImage((byte[])reader["image_data"]);
                            }
                            else
                            {
                                productImage = LoadPlaceHolderImage();
                            }
                            Orders.Add(new Reservation
                            {
                                ReservationID = (int)reader["reservation_id"],
                                UserID = (int)reader["user_id"],
                                ProductID = (int)reader["product_id"],
                                ReservationDate = Convert.ToDateTime(reader["reservationdate"]),
                                Status = reader["status"].ToString(),
                                HotelID = (int)reader["hotel_id"],
                                AmountPcs = (int)reader["amount_pcs"],
                                TotalPrice = (decimal)reader["total_price"],
                                HotelName = reader["hotel_name"].ToString(),
                                ProductName = reader["productname"].ToString(),
                                UserName = reader["username"].ToString(),  // New property for the user's name
                                ProductImage = productImage
                            });
                        }
                    }
                }
            }
        }

    }
}
