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

    }
}
