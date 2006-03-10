using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace Janrain.OpenId
{
    public abstract class Association : ICloneable
    {
        protected static DateTime UNIX_EPOCH = new System.DateTime(
                1970, 1, 1, 0, 0, 0, 0);

        protected string handle;
        protected internal DateTime issued;
        protected internal TimeSpan expiresIn;

        public string Handle { 
            get { return this.handle; } 
        }

        public DateTime Issued {
            get { return this.issued; }
        }

        public uint IssuedUnix {
            get { return (uint) (this.issued - UNIX_EPOCH).TotalSeconds; }
        }

        protected DateTime Expires {
            get { return this.issued.Add(this.expiresIn); }
        }

        public bool IsExpired { 
            get { return this.issued.Add(this.expiresIn) < DateTime.UtcNow;}
        }

        public long ExpiresIn {
            get { return (long)(this.Expires - DateTime.UtcNow).Seconds;}
        }

        protected abstract string AssociationType {
            get;
        }

        public abstract byte[] Secret {
            get;
        }

        public object Clone()
        {
            return Deserialize(this.Serialize());
        }

        public virtual byte[] Serialize()
        {
            NameValueCollection data = new NameValueCollection();
            data.Add("version", "2");
            data.Add("handle", this.handle);
            data.Add("secret", CryptUtil.ToBase64String(this.Secret));
            data.Add("issued", this.IssuedUnix.ToString());
            data.Add("expires_in", 
                    ((int)this.expiresIn.TotalSeconds).ToString());
            data.Add("assoc_type", this.AssociationType);

            return KVUtil.DictToKV(data);
        }

        public static Association Deserialize(byte[] data)
        {
            NameValueCollection kvpairs = KVUtil.KVToDict(data);
            string version = kvpairs["version"];
            if ( version != "2" ) {
                string msg = String.Format("Unknown version: {0}", version);
                throw new NotSupportedException(msg);
            }

            string assocType = kvpairs["assoc_type"];
            switch (assocType) {
            case "HMAC-SHA1":
                return new HMACSHA1Association(kvpairs);
            default:
                throw new NotSupportedException(String.Format(
                            "Unknown Association type: {0}", assocType));
            }
        }

        public abstract string SignDict(string[] fields, NameValueCollection data, string prefix);
    }

    public class HMACSHA1Association : Association
    {
        byte[] key;

        protected override string AssociationType
	{
            get { return "HMAC-SHA1"; }
        }

        public override byte[] Secret
	{
            get { return this.key; }
        }

        public HMACSHA1Association(string handle, byte[] secret, TimeSpan expiresIn)
        {
            this.handle = handle;
            this.key = secret;
            this.issued = UNIX_EPOCH.Add(new TimeSpan(0, 0, (int) (DateTime.UtcNow - UNIX_EPOCH).TotalSeconds));
            this.expiresIn = new TimeSpan(0, 0, (int)expiresIn.TotalSeconds);
        }

        protected internal HMACSHA1Association(NameValueCollection kvpairs)
        {
            this.handle = kvpairs["handle"];
            this.key = Convert.FromBase64String(kvpairs["secret"]);

            int seconds = Convert.ToInt32(kvpairs["issued"]);
            this.issued = UNIX_EPOCH.Add(new TimeSpan(0, 0, seconds));
            
            seconds = Convert.ToInt32(kvpairs["expires_in"]);
            this.expiresIn = new TimeSpan(0, 0, seconds);
        }

        public override bool Equals(object o)
        {
            if (o == null) return false;
            if (o is HMACSHA1Association)
                {
                    HMACSHA1Association a = (HMACSHA1Association)o;
                    if (a.handle != this.handle)
                        return false;

                    if (CryptUtil.ToBase64String(a.Secret) != 
                        CryptUtil.ToBase64String(this.Secret)) 
                        return false;

                    if (a.Expires != this.Expires)
                        return false;

                    if (a.expiresIn != this.expiresIn)
                        return false;

                    return true;
                }
            return false;
        }

        public override int GetHashCode()
        {
            HMACSHA1 hmac = new HMACSHA1(this.Secret);

            CryptoStream cs = new CryptoStream(Stream.Null, hmac, 
                    CryptoStreamMode.Write);
            byte[] hbytes = ASCIIEncoding.ASCII.GetBytes(this.handle);
            cs.Write(hbytes, 0, hbytes.Length);
            cs.Close();
            byte[] hash = hmac.Hash;
            hmac.Clear();
            long val = 0;
            for (long i = 0; i < hash.Length; i++)
                val ^= hash[i];
            val ^= this.Expires.ToFileTimeUtc();
            return (int)val;
        }

        public override string SignDict(string[] fields, NameValueCollection data, string prefix)
        {
	    NameValueCollection l = new NameValueCollection();
            foreach (string field in fields)
		l.Add(field, data[prefix + field]);

            return CryptUtil.ToBase64String(Sign(l));
        }

        private byte[] Sign(NameValueCollection l)
        {
            byte[] data = KVUtil.SeqToKV(l, false);
            HMACSHA1 hmac = new HMACSHA1(this.Secret);
            byte[] hash = hmac.ComputeHash(data);
            hmac.Clear();
            return hash;
        }
    }
}
