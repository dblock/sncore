using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitRefererHost : TransitService<RefererHost>
    {
        private string mHost;

        public string Host
        {
            get
            {

                return mHost;
            }
            set
            {
                mHost = value;
            }
        }

        private string mLastRefererUri;

        public string LastRefererUri
        {
            get
            {

                return mLastRefererUri;
            }
            set
            {
                mLastRefererUri = value;
            }
        }

        private string mLastRequestUri;

        public string LastRequestUri
        {
            get
            {

                return mLastRequestUri;
            }
            set
            {
                mLastRequestUri = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mUpdated;

        public DateTime Updated
        {
            get
            {

                return mUpdated;
            }
            set
            {
                mUpdated = value;
            }
        }

        private long mTotal = 0;

        public long Total
        {
            get
            {
                return mTotal;
            }
            set
            {
                mTotal = value;
            }
        }

        public TransitRefererHost()
        {

        }

        public TransitRefererHost(RefererHost o)
            : base(o)
        {
        }

        public override void SetInstance(RefererHost value)
        {
            Host = value.Host;
            LastRefererUri = value.LastRefererUri;
            LastRequestUri = value.LastRequestUri;
            Created = value.Created;
            Updated = value.Updated;
            Total = value.Total;
            base.SetInstance(value);
        }

        public override RefererHost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            RefererHost instance = base.GetInstance(session, sec);
            instance.Host = this.Host;
            instance.LastRefererUri = this.LastRefererUri;
            instance.LastRequestUri = this.LastRequestUri;
            instance.Total = this.Total;
            return instance;
        }
    }

    public class ManagedRefererHost : ManagedService<RefererHost, TransitRefererHost>
    {
        public class InvalidRefererHostException : SoapException
        {
            public InvalidRefererHostException()
                : base("Invalid Referer Host", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedRefererHost(ISession session)
            : base(session)
        {

        }

        public ManagedRefererHost(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedRefererHost(ISession session, RefererHost value)
            : base(session, value)
        {

        }

        public static RefererHost FindOrCreate(ISession session, string host)
        {
            RefererHost h = (RefererHost)session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Eq("Host", host))
                .UniqueResult();

            if (h == null)
            {
                h = new RefererHost();
                h.Created = h.Updated = DateTime.UtcNow;
                h.LastRefererUri = h.LastRequestUri = "http://localhost/";
                h.Host = host;
                h.Total = 0;
            }

            return h;
        }

        public static RefererHost Find(ISession session, string host)
        {
            RefererHost h = (RefererHost)session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Eq("Host", host))
                .UniqueResult();

            if (h == null)
            {
                throw new ManagedRefererHost.InvalidRefererHostException();
            }

            return h;
        }

        public static int GetRefererHostId(ISession session, string host)
        {
            return Find(session, host).Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Updated = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Updated;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }
}
