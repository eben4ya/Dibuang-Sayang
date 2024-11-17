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
                        p.productname
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
                                ProductName = reader["productname"].ToString()
                            });
                        }
                    }
                }
            }
        }

        // Fetch all orders without filtering by user
        private void FetchAllOrders()
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
                u.username  
            FROM 
                reservation r
            JOIN 
                apps_hotel_modified h ON r.hotel_id = h.hotel_id
            JOIN 
                product p ON r.product_id = p.product_id
            JOIN 
                apps_user_modified u ON r.user_id = u.user_id";  // Join with the user table to get the username

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
                                UserName = reader["username"].ToString()  // New property for the user's name
                            });
                        }
                    }
                }
            }
        }

    }
}
