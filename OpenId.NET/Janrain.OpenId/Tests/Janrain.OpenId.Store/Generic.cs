using System;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

using Janrain.OpenId;
using Janrain.OpenId.Store;

using Janrain.TestHarness;

namespace OpenIdTest.OpenId.Store
{
    public class StoreTester
    {
        IAssociationStore store;
        static Uri SERVER_URL = new Uri("http://www.myopenid.com/openid");
        static DateTime NOW = DateTime.UtcNow;
        
        
        public StoreTester ( IAssociationStore store )
        {
            this.store = store;
        }

        public static Association GenAssociation ()
        {
            return GenAssociation(0, 600);
        }

        public static Association GenAssociation (int issued)
        {
            return GenAssociation(issued, 600);
        }

        public static Association GenAssociation (int issued, int lifetime)
        {
            byte[] secret = new byte[20];
            (new RNGCryptoServiceProvider()).GetBytes(secret);
            byte[] handle = new byte[128];
            CryptUtil.RandomSelection(handle, 
                    Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz" + 
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                        "0123456789"));

            return new HMACSHA1Association(
                    Encoding.ASCII.GetString(handle),
                    secret,
                    (NOW + new TimeSpan(0, 0, issued + lifetime)) - 
                    DateTime.UtcNow);
        }

        public void CheckRetrieve ()
        {
            CheckRetrieve(null, null);
        }
        
        public void CheckRetrieve ( string handle, 
                                    Association expected )
        {
            Association retrieved = this.store.GetAssociation(SERVER_URL,
                    handle);
            if (expected == null || this.store.IsDumb)
                TestTools.Assert(retrieved == null);
            else
                TestTools.Assert(retrieved.Equals(expected));
        }

        public void CheckRemove ( string handle,
                                  bool expected )
        {
            CheckRemove(SERVER_URL, handle, expected);
        }
        
        public void CheckRemove ( Uri serverUrl,
                                  string handle,
                                  bool expected )
        {
            bool present = this.store.RemoveAssociation(serverUrl, handle);
            bool expectedPresent = (! this.store.IsDumb) && expected;
            TestTools.Assert(((!expectedPresent) && (!present)) || 
                    (expectedPresent && present));
        }
        

        public void Test ()
        {
            Association assoc = GenAssociation();
            // Make sure that a missing association returns no result
            CheckRetrieve();

            // Check that after storage, getting returns the same result
            this.store.StoreAssociation(SERVER_URL, assoc);
            CheckRetrieve(null, assoc);

            // more than once
            CheckRetrieve(null, assoc);
            
            // Storing more than once has no ill effect
            this.store.StoreAssociation(SERVER_URL, assoc);
            CheckRetrieve(null, assoc);
            
            // Removing an association that does not exist returns not present
            CheckRemove(assoc.Handle + "x", false);

            // Removing an association that does not exist returns not present
            CheckRemove(
                    new Uri(SERVER_URL.ToString() + "x"), assoc.Handle, false);
    
            // Removing an association that is present returns present
            CheckRemove(assoc.Handle, true);
            
            // but not present on subsequent calls
            CheckRemove(assoc.Handle, false);
            
            // Put assoc back in the this.store
            this.store.StoreAssociation(SERVER_URL, assoc);
            
            // More recent and expires after assoc
            Thread.Sleep(new TimeSpan(0, 0, 1));
            Association assoc2 = GenAssociation(1);
            this.store.StoreAssociation(SERVER_URL, assoc2);
            
            // After storing an association with a different handle, but the
            // same , the handle with the later expiration is
            // returned.
            CheckRetrieve(null, assoc2);

            // We can still retrieve the older association
            CheckRetrieve(assoc.Handle, assoc);
            
            // Plus we can retrieve the association with the later expiration
            // explicitly
            CheckRetrieve(assoc2.Handle, assoc2);
            
            // More recent, but expires earlier than assoc2 or assoc
            Thread.Sleep(new TimeSpan(0, 0, 1));
            Association assoc3 = GenAssociation(2, 100);
            this.store.StoreAssociation(SERVER_URL, assoc3);
            
            CheckRetrieve(null, assoc3);
            CheckRetrieve(assoc.Handle, assoc);
            CheckRetrieve(assoc2.Handle, assoc2);
            CheckRetrieve(assoc3.Handle, assoc3);
            
            CheckRemove(assoc2.Handle, true);
            
            CheckRetrieve(null, assoc3);
            CheckRetrieve(assoc.Handle, assoc);
            CheckRetrieve(assoc2.Handle, null);
            CheckRetrieve(assoc3.Handle, assoc3);
            
            CheckRemove(assoc2.Handle, false);
            CheckRemove(assoc3.Handle, true);
            
            CheckRetrieve(null, assoc);
            CheckRetrieve(assoc.Handle, assoc);
            CheckRetrieve(assoc2.Handle, null);
            CheckRetrieve(assoc3.Handle, null);
            
            CheckRemove(assoc2.Handle, false);
            CheckRemove(assoc.Handle, true);
            CheckRemove(assoc3.Handle, false);
            
            CheckRetrieve(null, null);
            CheckRetrieve(assoc.Handle, null);
            CheckRetrieve(assoc2.Handle, null);
            CheckRetrieve(assoc3.Handle, null);
            
            CheckRemove(assoc2.Handle, false);
            CheckRemove(assoc.Handle, false);
            CheckRemove(assoc3.Handle, false);
        }
    }
}
