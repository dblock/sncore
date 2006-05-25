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
    public class TransitRefererQuery : TransitService
    {
        private string mKeywords;

        public string Keywords
        {
            get
            {

                return mKeywords;
            }
            set
            {
                mKeywords = value;
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

        private int mTotal = 0;

        public int Total
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

        public TransitRefererQuery()
        {

        }

        public TransitRefererQuery(RefererQuery o)
            : base(o.Id)
        {
            Keywords = o.Keywords;
            Created = o.Created;
            Updated = o.Updated;
            Total = o.Total;
        }

        public RefererQuery GetRefererQuery(ISession session)
        {
            RefererQuery p = (Id != 0) ? (RefererQuery)session.Load(typeof(RefererQuery), Id) : new RefererQuery();
            p.Keywords = this.Keywords;
            p.Total = this.Total;
            return p;
        }
    }

    /// <summary>
    /// Managed RefererQuery.
    /// </summary>
    public class ManagedRefererQuery : ManagedService
    {
        private RefererQuery mRefererQuery = null;

        public ManagedRefererQuery(ISession session)
            : base(session)
        {

        }

        public ManagedRefererQuery(ISession session, int id)
            : base(session)
        {
            mRefererQuery = (RefererQuery)session.Load(typeof(RefererQuery), id);
        }

        public ManagedRefererQuery(ISession session, RefererQuery value)
            : base(session)
        {
            mRefererQuery = value;
        }

        public ManagedRefererQuery(ISession session, TransitRefererQuery value)
            : base(session)
        {
            mRefererQuery = value.GetRefererQuery(session);
        }

        public int Id
        {
            get
            {
                return mRefererQuery.Id;
            }
        }

        public TransitRefererQuery TransitRefererQuery
        {
            get
            {
                return new TransitRefererQuery(mRefererQuery);
            }
        }

        public void Delete()
        {
            Session.Delete(mRefererQuery);
        }
    }
}
