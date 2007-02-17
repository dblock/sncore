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

namespace SnCore.Services
{
    public class TransitSummarizedCounter : TimestampCounter<long>
    {
        public TransitSummarizedCounter()
            : base(DateTime.UtcNow, 0)
        {

        }

        public TransitSummarizedCounter(DateTime ts, long total)
            : base(ts, total)
        {

        }

        public TransitSummarizedCounter(Counter c)
            : base(DateTime.UtcNow, 0)
        {
            if (c != null)
            {
                Timestamp = c.Created;
                Total = c.Total;
            }
        }
    }

    public class TransitCounter : TransitService<Counter>
    {
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

        private long mTotal;

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

        private string mUri;

        public string Uri
        {
            get
            {
                return mUri;
            }
            set
            {
                mUri = value;
            }
        }

        public TransitCounter()
        {

        }

        public TransitCounter(Counter instance)
            : base(instance)
        {

        }

        public override void SetInstance(Counter instance)
        {
            Created = instance.Created;
            Modified = instance.Modified;
            Uri = instance.Uri;
            Total = instance.Total;
            base.SetInstance(instance);
        }

        public override Counter GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Counter instance = base.GetInstance(session, sec);
            instance.Uri = this.Uri;
            instance.Total = this.Total;
            return instance;
        }
    }

    public class ManagedCounter : ManagedService<Counter, TransitCounter>
    {
        public class InvalidCounterException : SoapException
        {
            public InvalidCounterException()
                : base("Invalid counter setting", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedCounter()
        {

        }


        public ManagedCounter(ISession session)
            : base(session)
        {

        }

        public ManagedCounter(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedCounter(ISession session, Counter value)
            : base(session, value)
        {

        }

        public static TransitCounter FindByUri(ISession session, string uri, ManagedSecurityContext sec)
        {
            ManagedCounter m_counter = new ManagedCounter();
            Counter counter = (Counter)session.CreateCriteria(typeof(Counter))
                    .Add(Expression.Eq("Uri", uri))
                    .UniqueResult();

            if (counter == null)
            {
                counter = new Counter();
                counter.Uri = uri;
                counter.Total = 0;
                counter.Created = counter.Modified = DateTime.UtcNow;
            }

            m_counter.SetInstance(session, counter);
            return m_counter.GetTransitInstance(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
