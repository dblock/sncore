using System;
using System.Data;

using ByteFX.Data.MySqlClient;

using Janrain.OpenId.Store;

namespace Janrain.OpenId.Store.MySql
{
    
    public class MySqlStore : SQLStore
    {
        public MySqlStore(IDbConnection connection) :
	    this(connection, "oid_settings", "oid_associations", "oid_nonces")
        { }

        public MySqlStore(IDbConnection connection, string settingsTable, string associationsTable, string noncesTable) :
            base(connection, settingsTable, associationsTable, noncesTable)
        { }
        
        protected override string CreateSettingsTableSQL
        {
            get {
                return String.Format(@"CREATE TABLE {0}
                        (
                         setting VARCHAR(128) UNIQUE PRIMARY KEY,
                         value BLOB(20)
                        )
                        TYPE=InnoDB;"
                        , this.settingsTable);
            }
        }
        
        protected override string CreateAssociationsTableSQL
        {
            get {
                return String.Format(@"CREATE TABLE {0}
                        (
                         server_url VARCHAR(767),
                         handle VARCHAR(255),
                         serialized BLOB(10000),
                         PRIMARY KEY (server_url, handle)
                        )
                        TYPE=InnoDB;"
                        , this.associationsTable);
            }
        }

        protected override string CreateNoncesTableSQL
        {
            get {
                return String.Format(@"CREATE TABLE {0}
                        ( nonce CHAR(8) UNIQUE PRIMARY KEY,
                          timestamp INTEGER
                        )
                        TYPE=InnoDB;"
                        , this.noncesTable);
            }
        }

        
        protected override IDbCommand CreateAuthCmd(IDbTransaction txn, byte[] authKey)
        {
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("INSERT INTO {0} VALUES (\"auth_key\", @authKey);", this.settingsTable);
            IDataParameter param = cmd.Parameters.Add("@authKey", authKey);
            param.DbType = DbType.Binary;
            return cmd;
        }

        protected override IDbCommand GetAuthCmd(IDbTransaction txn)
        {
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("SELECT value FROM {0} WHERE setting = \"auth_key\";", this.settingsTable);
            return cmd;
        }
        
        protected override IDbCommand SetAssociationCmd(IDbTransaction txn, Uri serverUri, Association assoc)
        {
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("REPLACE INTO {0} VALUES (@serverUrl, @handle, @serialized);", this.associationsTable);
            
            MySqlParameter param;
            param = cmd.Parameters.Add("@serverUrl", serverUri.AbsoluteUri);
            param.DbType= DbType.String;
            param = cmd.Parameters.Add("@handle", assoc.Handle);
            param.DbType= DbType.String;
            param = cmd.Parameters.Add("@serialized", assoc.Serialize());
            param.DbType= DbType.Binary;
            return cmd;
        }

        protected override IDbCommand GetAssociationsCmd(IDbTransaction txn, Uri serverUri)
        {
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("SELECT serialized FROM {0} WHERE server_url = @serverUrl;", this.associationsTable);

            MySqlParameter param = cmd.Parameters.Add("@serverUrl", 
                    serverUri.AbsoluteUri);
            param.DbType = DbType.String;
            return cmd;
        }
        
        protected override IDbCommand GetAssociationCmd(IDbTransaction txn, Uri serverUri, string handle)
        {
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("SELECT serialized FROM {0} {1}", this.associationsTable,
					    "WHERE server_url = @serverUrl AND handle = @handle;'");

            MySqlParameter param = cmd.Parameters.Add("@serverUrl", serverUri.AbsoluteUri);
            param.DbType = DbType.String;
            param = cmd.Parameters.Add("@handle", handle);
            param.DbType = DbType.String;
            return cmd;
        }
        
        protected override IDbCommand RemoveAssociationCmd(IDbTransaction txn, Uri serverUri, string handle)
        {
            MySqlParameter param;
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("DELETE FROM {0} WHERE server_url = @serverUrl AND handle = @handle;", this.associationsTable);
            
            param = cmd.Parameters.Add("@serverUrl", serverUri.AbsoluteUri);
            param.DbType = DbType.String;
            param = cmd.Parameters.Add("@handle", handle);
            param.DbType = DbType.String;
            return cmd;
        }
        
        protected override IDbCommand AddNonceCmd(IDbTransaction txn, string nonce, Int32 timestamp)
        {
            MySqlParameter param;
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("REPLACE INTO {0} VALUES (@nonce, @timestamp);", this.noncesTable);
            
            param = cmd.Parameters.Add("@nonce", nonce);
            param.DbType = DbType.String;
            param = cmd.Parameters.Add("@timestamp", timestamp);
            param.DbType = DbType.Int32;
            
            return cmd;
        }

        protected override IDbCommand GetNonceCmd(IDbTransaction txn, string nonce)
        {
            MySqlParameter param;
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("SELECT * FROM {0} WHERE nonce = @nonce;", this.noncesTable);
            
            param = cmd.Parameters.Add("@nonce", nonce);
            param.DbType = DbType.String;

            return cmd;
        }
        
        protected override IDbCommand RemoveNonceCmd(IDbTransaction txn, string nonce)
        {
            MySqlParameter param;
            MySqlCommand cmd = (MySqlCommand) this.connection.CreateCommand();
            cmd.Transaction = txn;
            cmd.CommandText = String.Format("DELETE FROM {0} WHERE nonce = @nonce;", this.noncesTable);
            
            param = cmd.Parameters.Add("@nonce", nonce);
            param.DbType = DbType.String;

            return cmd;
            
        }
        
    }
}
