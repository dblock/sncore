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
    public class TransitRefererHost : TransitService
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
            : base(o.Id)
        {
            Host = o.Host;
            LastRefererUri = o.LastRefererUri;
            LastRequestUri = o.LastRequestUri;
            Created = o.Created;
            Updated = o.Updated;
            Total = o.Total;
        }

        public RefererHost GetRefererHost(ISession session)
        {
            RefererHost p = (Id != 0) ? (RefererHost)session.Load(typeof(RefererHost), Id) : new RefererHost();
            p.Host = this.Host;
            p.LastRefererUri = this.LastRefererUri;
            p.LastRequestUri = this.LastRequestUri;
            p.Total = this.Total;
            return p;
        }
    }

    /// <summary>
    /// Managed RefererHost.
    /// </summary>
    public class ManagedRefererHost : ManagedService
    {
        public class InvalidRefererHostException : SoapException
        {
            public InvalidRefererHostException()
                : base("Invalid Referer Host", SoapException.ClientFaultCode)
            {

            }
        }

        private RefererHost mRefererHost = null;

        public ManagedRefererHost(ISession session)
            : base(session)
        {

        }

        public ManagedRefererHost(ISession session, int id)
            : base(session)
        {
            mRefererHost = (RefererHost)session.Load(typeof(RefererHost), id);
        }

        public ManagedRefererHost(ISession session, RefererHost value)
            : base(session)
        {
            mRefererHost = value;
        }

        public ManagedRefererHost(ISession session, TransitRefererHost value)
            : base(session)
        {
            mRefererHost = value.GetRefererHost(session);
        }

        public int Id
        {
            get
            {
                return mRefererHost.Id;
            }
        }

        public TransitRefererHost TransitRefererHost
        {
            get
            {
                return new TransitRefererHost(mRefererHost);
            }
        }

        public void Delete()
        {
            Session.Delete(mRefererHost);
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

    }
}
