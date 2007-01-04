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
    public class TransitRefererQuery : TransitService<RefererQuery>
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

        public TransitRefererQuery(RefererQuery instance)
            : base(instance)
        {
        }

        public override void SetInstance(RefererQuery instance)
        {
            Keywords = instance.Keywords;
            Created = instance.Created;
            Updated = instance.Updated;
            Total = instance.Total;
            base.SetInstance(instance);
        }

        public override RefererQuery GetInstance(ISession session, ManagedSecurityContext sec)
        {
            RefererQuery instance = base.GetInstance(session, sec);
            instance.Keywords = this.Keywords;
            instance.Total = this.Total;
            return instance;
        }
    }

    public class ManagedRefererQuery : ManagedService<RefererQuery, TransitRefererQuery>
    {
        public ManagedRefererQuery()
        {

        }

        public ManagedRefererQuery(ISession session)
            : base(session)
        {

        }

        public ManagedRefererQuery(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedRefererQuery(ISession session, RefererQuery value)
            : base(session, value)
        {

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
