using System;
using System.Data;
using System.Security.Cryptography;

namespace Janrain.OpenId.Store
{
    public abstract class SQLStore : IAssociationStore
    {
        // Store Nonces for a max of Six Hours
        protected static TimeSpan MAX_NONCE_AGE = new TimeSpan(6, 0, 0);
        
        protected IDbConnection connection;
        protected string settingsTable;
        protected string associationsTable;
        protected string noncesTable;
        
        protected abstract string CreateSettingsTableSQL { get; }
        protected abstract string CreateAssociationsTableSQL { get; }
        protected abstract string CreateNoncesTableSQL { get; }

        protected abstract IDbCommand CreateAuthCmd(IDbTransaction txn, byte[] authKey );
        protected abstract IDbCommand GetAuthCmd(IDbTransaction txn);

        protected abstract IDbCommand SetAssociationCmd(IDbTransaction txn, Uri serverUri, Association assoc);

        protected abstract IDbCommand GetAssociationsCmd(IDbTransaction txn, Uri serverUri);
        
        protected abstract IDbCommand GetAssociationCmd(IDbTransaction txn, Uri serverUri, string handle);

        protected abstract IDbCommand RemoveAssociationCmd(IDbTransaction txn, Uri serverUri, string handle);

        protected abstract IDbCommand AddNonceCmd(IDbTransaction txn, string nonce, int timestamp);
        
        protected abstract IDbCommand GetNonceCmd(IDbTransaction txn, string nonce);
        
        protected abstract IDbCommand RemoveNonceCmd(IDbTransaction txn, string nonce);
        
        public bool IsDumb 
        { 
            get {
                return false;
            }
        }

        public byte[] AuthKey 
        { 
            get {
                IDbTransaction txn = this.connection.BeginTransaction();
                try 
                {
                    IDbCommand cmd = GetAuthCmd(txn);
                    byte[] authKey = null;
                    IDataReader reader = cmd.ExecuteReader();
		    try
		    {
			if (reader.Read())
			    authKey= (byte[]) reader[0];
		    }
		    finally
		    {
			reader.Close();
		    }

                    if (authKey == null)
                    {
                        authKey = new byte[20];
                        (new RNGCryptoServiceProvider()).GetBytes(authKey);
                        cmd = CreateAuthCmd(txn, authKey);
                        cmd.ExecuteNonQuery();
                    }
                    
                    txn.Commit();
                    return authKey;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    txn.Rollback();
                    throw e;
                }
            }
        }
        
        public SQLStore( IDbConnection connection ) : this(connection, "oid_settings", "oid_associations", "oid_nonces")
        {
	}

        public SQLStore(IDbConnection connection, string settingsTable, string associationsTable, string noncesTable)
        {
            this.connection = connection;
            this.settingsTable = settingsTable;
            this.associationsTable = associationsTable;
            this.noncesTable = noncesTable;
        }

        public virtual void CreateTables()
        {
            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                IDbCommand cmd = this.connection.CreateCommand();
                cmd.Connection = this.connection;
                cmd.Transaction = txn;
                cmd.CommandText = this.CreateSettingsTableSQL;
                cmd.ExecuteNonQuery();
                
                cmd = this.connection.CreateCommand();
                cmd.Connection = this.connection;
                cmd.Transaction = txn;
                cmd.CommandText = this.CreateAssociationsTableSQL;
                cmd.ExecuteNonQuery();
                
                cmd = this.connection.CreateCommand();
                cmd.Connection = this.connection;
                cmd.Transaction = txn;
                cmd.CommandText = this.CreateNoncesTableSQL;
                cmd.ExecuteNonQuery();

                txn.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }

        
        public void StoreAssociation(Uri serverUri, Association assoc)
        {
	    Console.WriteLine("StoreSecret: {0}",
			      CryptUtil.ToBase64String(assoc.Secret));

            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                IDbCommand cmd = SetAssociationCmd(txn, serverUri, assoc);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }
        
        public virtual Association GetAssociation(Uri serverUri)
        {
            return GetAssociation(serverUri, null);
        }

        public virtual Association GetAssociation(Uri serverUri, string handle)
        {
            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                IDbCommand cmd;
                if (handle == null)
                    cmd= this.GetAssociationsCmd(txn, serverUri);
                else
                    cmd = this.GetAssociationCmd(txn, serverUri, handle);

                Association assoc, best = null;
                IDataReader reader = cmd.ExecuteReader();
		try
		{
		    while (reader.Read())
		    {
			assoc = Association.Deserialize((byte[]) reader[0]);
			Console.WriteLine("ReadSecret: {0}", CryptUtil.ToBase64String(assoc.Secret));

			if (assoc.IsExpired)
			    RemoveAssociation(txn, serverUri, assoc.Handle);
			else
			    if (best == null || best.Issued < assoc.Issued)
				best = assoc;
		    }
		}
		finally
		{
		    reader.Close();
		}

                txn.Commit();
                return best;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }

        private bool RemoveAssociation(IDbTransaction txn, Uri serverUri, string handle)
        {
            IDbCommand cmd = RemoveAssociationCmd(txn, serverUri, handle);
            IDataReader reader = cmd.ExecuteReader();
	    try
	    {
		while (reader.Read()) {}
	    }
	    finally
	    {
		reader.Close();
	    }
            bool result = reader.RecordsAffected > 0;
            reader.Close();
            return result;
        }

        public virtual bool RemoveAssociation(Uri serverUri, string handle)
        {
            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                bool result = RemoveAssociation(txn, serverUri, handle);
                txn.Commit();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }
        
        public virtual void StoreNonce(string nonce)
        {
	    Console.WriteLine("StoreNonce: [{0}]", nonce);
            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                int timestamp = (int) (DateTime.UtcNow - 
                        (new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds;
                
                IDbCommand cmd = AddNonceCmd(txn, nonce, timestamp);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }
        
        public virtual bool UseNonce(string nonce)
        {
	    Console.WriteLine("UseNonce: [{0}]", nonce);
            IDbTransaction txn = this.connection.BeginTransaction();
            try 
            {
                bool present = false;
                IDbCommand cmd = GetNonceCmd(txn, nonce);
		bool read;
                
                IDataReader reader = cmd.ExecuteReader();
		try
		{
		    if (read = reader.Read())
		    {
			DateTime timestamp =
			    (new DateTime(1970, 1, 1, 0, 0, 0, 0)) 
			    + (new TimeSpan(0, 0, (int) reader[1]));

			if ((DateTime.UtcNow - timestamp) <= MAX_NONCE_AGE)
			{
			    Console.WriteLine("Present");
			    present = true;
			}
			
		    }
		    else
		    {
			Console.WriteLine("Nothing Read");
		    }
		}
		finally
		{
		    reader.Close();
		}

		if (read)
		{
		    cmd = RemoveNonceCmd(txn, nonce);
		    cmd.ExecuteNonQuery();
		}

                txn.Commit();

                return present;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                txn.Rollback();
                throw e;
            }
        }
    }


}

