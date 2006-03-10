using System;
using System.Collections;
using System.Security.Cryptography;

using Janrain.OpenId;
using Janrain.OpenId.Store;

namespace OpenIdTest.OpenId 
{
    class MemoryStore : IAssociationStore
    {
        Hashtable serverAssocs;
        Hashtable nonces;
        byte[] authKey = new byte[20];
        
        public MemoryStore()
        {
            this.serverAssocs = new Hashtable();
            this.nonces = new Hashtable();
            (new RNGCryptoServiceProvider()).GetBytes(this.authKey);
        }
        
        public byte[] AuthKey
        {
            get { 
                return (byte[]) this.authKey.Clone();
            }
        }

        public bool IsDumb
        {
            get {
                return false;
            }
        }

        private ServerAssocs GetServerAssocs ( Uri serverUri )
        {
            if (serverAssocs[serverUri] == null)
                serverAssocs.Add(serverUri, new ServerAssocs());
            return (ServerAssocs) serverAssocs[serverUri];
        }
	
        public void StoreAssociation ( Uri serverUri, 
                                       Association assoc )
        {
            ServerAssocs assocs = GetServerAssocs(serverUri);
            assocs.Set((Association)assoc.Clone());
        }
        
        public Association GetAssociation ( Uri serverUri )
        {
            return GetServerAssocs(serverUri).Best();
        }

        public Association GetAssociation ( Uri serverUri, 
                                            string handle )
        {
            return GetServerAssocs(serverUri).Get(handle);
        }
        
        public bool RemoveAssociation ( Uri serverUri, 
                                        string handle )
        {
            return GetServerAssocs(serverUri).Remove(handle);
        }
        
        public void StoreNonce ( string nonce )
        {
            this.nonces[nonce] = 0;
        }
        
        public bool UseNonce(string nonce)
        {
            bool ret = (this.nonces[nonce] != null);
            this.nonces.Remove(nonce);
            return ret;
        }


        class ServerAssocs
        {
            Hashtable assocs;
            
            public ServerAssocs ()
            {
                this.assocs = new Hashtable();
            }
            
            public void Set ( Association assoc )
            {
                this.assocs.Add(assoc.Handle, assoc);
            }
            
            public Association Get ( string handle )
            {
                return (Association) this.assocs[handle];
            }
            
            public bool Remove ( string handle )
            {
                bool ret = (this.assocs[handle] == null);
                this.assocs.Remove(handle);
                return ret;
            }
            
            public Association Best ()
            {
                Association best = null;
                foreach (Association assoc in this.assocs.Values)
                    if (best == null || best.Issued < assoc.Issued)
                        best = assoc;
                return best;
            }
        }
    }
}
