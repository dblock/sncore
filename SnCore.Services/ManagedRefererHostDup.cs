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
    public class TransitRefererHostDup : TransitService
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

        private string mRefererHost;

        public string RefererHost
        {
            get
            {

                return mRefererHost;
            }
            set
            {
                mRefererHost = value;
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

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        public TransitRefererHostDup()
        {

        }

        public TransitRefererHostDup(RefererHostDup o)
            : base(o.Id)
        {
            Host = o.Host;
            RefererHost = o.RefererHost.Host;
            Created = o.Created;
            Modified = o.Modified;
        }

        public RefererHostDup GetRefererHostDup(ISession session)
        {
            RefererHostDup d = (Id != 0) ? (RefererHostDup)session.Load(typeof(RefererHostDup), Id) : new RefererHostDup();
            d.Host = this.Host;
            d.RefererHost = ManagedRefererHost.Find(session, this.RefererHost);
            return d;
        }
    }

    /// <summary>
    /// Managed RefererHostDup.
    /// </summary>
    public class ManagedRefererHostDup : ManagedService<RefererHostDup>
    {
        private RefererHostDup mRefererHostDup = null;

        public ManagedRefererHostDup(ISession session)
            : base(session)
        {

        }

        public ManagedRefererHostDup(ISession session, int id)
            : base(session)
        {
            mRefererHostDup = (RefererHostDup)session.Load(typeof(RefererHostDup), id);
        }

        public ManagedRefererHostDup(ISession session, RefererHostDup value)
            : base(session)
        {
            mRefererHostDup = value;
        }

        public ManagedRefererHostDup(ISession session, TransitRefererHostDup value)
            : base(session)
        {
            mRefererHostDup = value.GetRefererHostDup(session);
        }

        public int Id
        {
            get
            {
                return mRefererHostDup.Id;
            }
        }

        public TransitRefererHostDup TransitRefererHostDup
        {
            get
            {
                return new TransitRefererHostDup(mRefererHostDup);
            }
        }

        public void Delete()
        {
            Session.Delete(mRefererHostDup);
        }

        public void CreateOrUpdate(TransitRefererHostDup o)
        {
            Session.Save(ManagedRefererHost.FindOrCreate(Session, o.RefererHost));
            mRefererHostDup = o.GetRefererHostDup(Session);
            mRefererHostDup.Modified = DateTime.UtcNow;
            if (Id == 0) mRefererHostDup.Created = mRefererHostDup.Modified;

            // merge existing dup hosts
            IList hosts = Session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Like("Host", mRefererHostDup.Host)).List();

            foreach (RefererHost host in hosts)
            {
                if (host != mRefererHostDup.RefererHost)
                {
                    mRefererHostDup.RefererHost.Total += host.Total;
                    Session.Delete(host);
                }
            }

            Session.Save(mRefererHostDup);
        }
    }
}
