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
    public class TransitRefererHostDup : TransitService<RefererHostDup>
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

        public TransitRefererHostDup(RefererHostDup instance)
            : base(instance)
        {

        }

        public override void SetInstance(RefererHostDup instance)
        {
            Host = instance.Host;
            RefererHost = instance.RefererHost.Host;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override RefererHostDup GetInstance(ISession session, ManagedSecurityContext sec)
        {
            RefererHostDup instance = base.GetInstance(session, sec);
            instance.Host = this.Host;
            instance.RefererHost = ManagedRefererHost.Find(session, this.RefererHost);
            return instance;
        }
    }

    public class ManagedRefererHostDup : ManagedService<RefererHostDup, TransitRefererHostDup>
    {
        public ManagedRefererHostDup()
        {

        }

        public ManagedRefererHostDup(ISession session)
            : base(session)
        {

        }

        public ManagedRefererHostDup(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedRefererHostDup(ISession session, RefererHostDup value)
            : base(session, value)
        {

        }

        public override int CreateOrUpdate(TransitRefererHostDup instance, ManagedSecurityContext sec)
        {
            RefererHost rh = ManagedRefererHost.FindOrCreate(Session, instance.RefererHost);

            // merge existing dup hosts
            IList hosts = Session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Like("Host", instance.Host)).List();

            foreach (RefererHost host in hosts)
            {
                if (host != rh)
                {
                    rh.Total += host.Total;
                    Session.Delete(host);
                }
            }

            Session.Save(rh);
            return base.CreateOrUpdate(instance, sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            return acl;
        }
    }
}
