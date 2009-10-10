using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace SnCore.Web.Soap.Tests
{
    public abstract class DatabaseTestInstance
    {
        private static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationSettings.AppSettings["DatabaseConnectionString"];
            if (string.IsNullOrEmpty(connectionString)) connectionString = "Server=.;Database=SnCore;Trusted_Connection=yes;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static void UpdateSearchIndex(string table)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(string.Format(
                    "EXEC sp_fulltext_table @tabname='{0}', @action='update_index'", table), connection);
                command.ExecuteNonQuery();
            }
        }

        public static void RebuildSearchIndex()
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand("ALTER FULLTEXT CATALOG SnCore REBUILD", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
