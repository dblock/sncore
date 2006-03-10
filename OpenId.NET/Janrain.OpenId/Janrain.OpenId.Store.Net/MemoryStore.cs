using System;
using System.Collections;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;

using Janrain.OpenId;
using Janrain.OpenId.Store;

namespace Janrain.OpenId.Store.Net
{
    public class MemoryStore : IAssociationStore
    {
        Cache cache = HttpRuntime.Cache;
        Hashtable serverAssocs;
        Hashtable nonces;
        byte[] authKey;

        public MemoryStore()
        {
            this.serverAssocs = (Hashtable)cache["OpenId.serverAssocs"];
            this.nonces = (Hashtable)cache["OpenId.nonces"];
            this.authKey = (byte[])cache["OpenId.authKey"];

            if (serverAssocs == null)
                cache.Insert("OpenId.serverAssocs", serverAssocs = new Hashtable(),
                    null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0));

            if (nonces == null)
                cache.Insert("OpenId.nonces", nonces = new Hashtable(), 
                    null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0));

            if (authKey == null)
            {
                authKey = new byte[20];
                (new RNGCryptoServiceProvider()).GetBytes(authKey);
                cache.Insert("OpenId.authKey", authKey, 
                    null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0, 0));
            }

        }

        public byte[] AuthKey
        {
            get
            {
                return (byte[])this.authKey.Clone();
            }
        }

        public bool IsDumb
        {
            get
            {
                return false;
            }
        }

        private ServerAssocs GetServerAssocs(Uri serverUri)
        {
            if (!serverAssocs.ContainsKey(serverUri))
                serverAssocs.Add(serverUri, new ServerAssocs());

            return (ServerAssocs)serverAssocs[serverUri];
        }

        public void StoreAssociation(Uri serverUri,
                                       Association assoc)
        {
            ServerAssocs assocs = GetServerAssocs(serverUri);
            assocs.Set((Association)assoc.Clone());
        }

        public Association GetAssociation(Uri serverUri)
        {
            return GetServerAssocs(serverUri).Best();
        }

        public Association GetAssociation(Uri serverUri,
                                            string handle)
        {
            return GetServerAssocs(serverUri).Get(handle);
        }

        public bool RemoveAssociation(Uri serverUri,
                                        string handle)
        {
            return GetServerAssocs(serverUri).Remove(handle);
        }

        public void StoreNonce(string nonce)
        {
            this.nonces[nonce] = 0;
        }

        public bool UseNonce(string nonce)
        {
            bool ret = this.nonces.ContainsKey(nonce);
            this.nonces.Remove(nonce);
            return ret;
        }


        class ServerAssocs
        {
            Hashtable assocs;

            public ServerAssocs()
            {
                this.assocs = new Hashtable();
            }

            public void Set(Association assoc)
            {
                this.assocs[assoc.Handle] = assoc;
            }

            public Association Get(string handle)
            {
                Association assoc = null;
                if (this.assocs.Contains(handle))
                    assoc = (Association)this.assocs[handle];
                return assoc;
            }

            public bool Remove(string handle)
            {
                bool ret = this.assocs.Contains(handle);
                this.assocs.Remove(handle);
                return ret;
            }

            public Association Best()
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
