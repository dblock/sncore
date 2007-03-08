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
    public class TransitRefererAccount : TransitService<RefererAccount>
    {
        private int mAccountPictureId;

        public int AccountPictureId
        {
            get
            {

                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
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

        private string mRefererHostLastRefererUri;

        public string RefererHostLastRefererUri
        {
            get
            {
                return mRefererHostLastRefererUri;
            }
            set
            {
                mRefererHostLastRefererUri = value;
            }
        }

        private string mRefererHostName;

        public string RefererHostName
        {
            get
            {

                return mRefererHostName;
            }
            set
            {
                mRefererHostName = value;
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

        private long mRefererHostTotal = 0;

        public long RefererHostTotal
        {
            get
            {

                return mRefererHostTotal;
            }
            set
            {
                mRefererHostTotal = value;
            }
        }

        public TransitRefererAccount()
        {

        }

        public TransitRefererAccount(RefererAccount o)
            : base(o)
        {
        }

        public override void SetInstance(RefererAccount value)
        {
            RefererHostName = value.RefererHost.Host;
            RefererHostLastRefererUri = value.RefererHost.LastRefererUri;
            RefererHostTotal = value.RefererHost.Total;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override RefererAccount GetInstance(ISession session, ManagedSecurityContext sec)
        {
            RefererAccount instance = base.GetInstance(session, sec);
            instance.RefererHost = string.IsNullOrEmpty(this.RefererHostName) ? null : ManagedRefererHost.Find(session, this.RefererHostName);
            instance.Account = (AccountId != 0) ? session.Load<Account>(AccountId) : null;
            return instance;
        }
    }

    public class ManagedRefererAccount : ManagedService<RefererAccount, TransitRefererAccount>
    {
        public ManagedRefererAccount()
        {

        }

        public ManagedRefererAccount(ISession session)
            : base(session)
        {

        }

        public ManagedRefererAccount(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedRefererAccount(ISession session, RefererAccount value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            bool fNew = (mInstance.Id == 0);
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);

            if (fNew)
            {
                Session.Flush();

                ManagedAccount acct = new ManagedAccount(Session, mInstance.Account);

                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, acct, string.Format("EmailRefererAccount.aspx?id={0}", mInstance.Id));
            }
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
