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
        public static void UpdateSearchIndex(string table)
        {
            SqlConnection connection = new SqlConnection((string) ConfigurationSettings.AppSettings["DatabaseConnectionString"]);
            connection.Open();
            SqlCommand command = new SqlCommand(string.Format(
                "EXEC sp_fulltext_table @tabname='{0}', @action='update_index'", table), connection);
            command.ExecuteNonQuery();
        }

        public static void RebuildSearchIndex()
        {
            SqlConnection connection = new SqlConnection((string)ConfigurationSettings.AppSettings["DatabaseConnectionString"]);
            connection.Open();
            SqlCommand command = new SqlCommand("ALTER FULLTEXT CATALOG SnCore REBUILD", connection);
            command.ExecuteNonQuery();
        }
    }
}
