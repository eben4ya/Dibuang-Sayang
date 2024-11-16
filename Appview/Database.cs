using System;
using Npgsql;
using System.Configuration;
using System.Windows;

namespace Appview.Data
{
    public class Database
    {
        private string connectionString;

        public Database()
        {
            // Read connection string from configuration file
           connectionString = ConfigurationManager.ConnectionStrings["PostgreSqlConnection"].ConnectionString;

        }

        public bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(1) FROM apps_user_modified WHERE username = @username AND password = @password";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Check if the user exists
                    var result = cmd.ExecuteScalar();
                    isAuthenticated = Convert.ToInt32(result) > 0;
                }
            }

            return isAuthenticated;
        }
        public bool RegisterUser(string username, string email, string password)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO apps_user_modified (username, email, password) VALUES (@username, @email, @password)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true if insert was successful
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }

        public bool RegisterHotel(string username, string email, string password)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO apps_hotel_modified (username, email, password) VALUES (@username, @email, @password)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true if insert was successful
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                MessageBox.Show("An error occurred: " + ex.Message);
                return false;
            }
        }

        public bool AuthenticateHotel(string username, string password)
        {
            bool isAuthenticated = false;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(1) FROM apps_hotel_modified WHERE username = @username AND password = @password";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Check if the hotel admin exists
                    var result = cmd.ExecuteScalar();
                    isAuthenticated = Convert.ToInt32(result) > 0;
                }
            }

            return isAuthenticated;
        }

        //Coba coba
        public int ValidateUser(string username, string password)
        {
            int userId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT user_id FROM apps_user_modified WHERE username = @username AND password = @password";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        var result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Wrong username or password, ", ex.Message);
            }

            return userId;
        }

        public bool InsertReservation(int userId, int quantity, decimal totalPrice, int productId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    //Fixed product_id and hotel_id for now
                    int hotelId = 1;

                    using (var transaction = conn.BeginTransaction())
                    {
                        string insertReservationQuery = @"
                            INSERT INTO reservation (user_id, product_id, reservationdate, hotel_id, amount_pcs, total_price)
                            VALUES (@userId, @productId, NOW(), @hotelId, @amountPcs, @totalPrice)";

                        using (var insertCmd = new NpgsqlCommand(insertReservationQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@userId", userId);
                            insertCmd.Parameters.AddWithValue("@productId", productId);
                            insertCmd.Parameters.AddWithValue("@hotelId", hotelId);
                            insertCmd.Parameters.AddWithValue("@amountPcs", quantity);
                            insertCmd.Parameters.AddWithValue("@totalPrice", totalPrice);

                            insertCmd.ExecuteNonQuery();
                        }

                        string updateStockQuery = @"
                            UPDATE product
                            SET quantityavailable = quantityavailable - @quantity
                            WHERE product_id = @productId AND quantityavailable >= @quantity";

                        using (var updateCmd = new NpgsqlCommand(updateStockQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@quantity", quantity);
                            updateCmd.Parameters.AddWithValue("@productId", productId);

                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                throw new Exception("Insufficent stock available");
                            }
                        }

                        // Commit transaction
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                return false;
            }
        }

    }
}
