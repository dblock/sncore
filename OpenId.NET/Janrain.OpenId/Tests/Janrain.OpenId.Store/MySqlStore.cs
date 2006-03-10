using System;

using ByteFX.Data.MySqlClient;

using Janrain.TestHarness;
using Janrain.OpenId.Store.MySql;

using OpenIdTest.OpenId.Store;

namespace OpenIdTest.OpenId.Store.MySql
{
    [TestSuite]
    public class MySqlStoreTestSuite
    {
        [Test]
        public void MySqlStore()
        {
            MySqlConnection connection = new MySqlConnection(
                    "Server=localhost;" +
                    "User ID=root;" +
                    "Password=;");
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            string sql = "DROP DATABASE openid_test;";
            cmd.CommandText = sql;
            try {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            { }
            cmd.Dispose();

            cmd = connection.CreateCommand();
            cmd.CommandText = "CREATE DATABASE openid_test;";
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = connection.CreateCommand();
            cmd.CommandText = "USE openid_test;";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            
            MySqlStore store = new MySqlStore(connection);
            store.CreateTables();
            StoreTester tester = new StoreTester(store);
            tester.Test();
        }
    }
    
}
