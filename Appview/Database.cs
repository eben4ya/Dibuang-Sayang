using System;
using Npgsql;
using System.Configuration;

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

                string query = "SELECT COUNT(1) FROM apps_user WHERE username = @username AND password = @password";
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
    }
}
