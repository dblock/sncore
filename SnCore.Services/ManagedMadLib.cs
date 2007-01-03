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
    public class TransitMadLib : TransitService<MadLib>
    {
        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mTemplate;

        public string Template
        {
            get
            {
                return mTemplate;
            }
            set
            {
                mTemplate = value;
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

        private int mAccountId;

        public int AccountId
        {
            get
            {
                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private string mAccountName;

        public string AccountName
        {
            get
            {
                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        public TransitMadLib()
        {

        }

        public TransitMadLib(MadLib instance)
            : base(instance)
        {

        }

        public override void SetInstance(MadLib instance)
        {
            Name = instance.Name;
            Template = instance.Template;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            base.SetInstance(instance);
        }

        public override MadLib GetInstance(ISession session, ManagedSecurityContext sec)
        {
            MadLib instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Template = this.Template;
            if (Id == 0 && AccountId > 0) instance.Account = (Account)session.Load(typeof(Account), AccountId);
            return instance;
        }
    }

    public class ManagedMadLib : ManagedService<MadLib, TransitMadLib>
    {
        public ManagedMadLib(ISession session)
            : base(session)
        {

        }

        public ManagedMadLib(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedMadLib(ISession session, MadLib value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}
