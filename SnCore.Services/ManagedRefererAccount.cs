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
    public class TransitRefererAccount : TransitService
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
            : base(o.Id)
        {
            RefererHostName = o.RefererHost.Host;
            RefererHostLastRefererUri = o.RefererHost.LastRefererUri;
            RefererHostTotal = o.RefererHost.Total;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.Account);
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            Created = o.Created;
            Modified = o.Modified;
        }

        public RefererAccount GetRefererAccount(ISession session)
        {
            RefererAccount p = (Id != 0) ? (RefererAccount)session.Load(typeof(RefererAccount), Id) : new RefererAccount();
            p.RefererHost = string.IsNullOrEmpty(this.RefererHostName) ? null : ManagedRefererHost.Find(session, this.RefererHostName);
            p.Account = (AccountId != 0) ? (Account)session.Load(typeof(Account), AccountId) : null;
            return p;
        }
    }

    /// <summary>
    /// Managed RefererAccount.
    /// </summary>
    public class ManagedRefererAccount : ManagedService<RefererAccount>
    {
        private RefererAccount mRefererAccount = null;

        public ManagedRefererAccount(ISession session)
            : base(session)
        {

        }

        public ManagedRefererAccount(ISession session, int id)
            : base(session)
        {
            mRefererAccount = (RefererAccount)session.Load(typeof(RefererAccount), id);
        }

        public ManagedRefererAccount(ISession session, RefererAccount value)
            : base(session)
        {
            mRefererAccount = value;
        }

        public ManagedRefererAccount(ISession session, TransitRefererAccount value)
            : base(session)
        {
            mRefererAccount = value.GetRefererAccount(session);
        }

        public int Id
        {
            get
            {
                return mRefererAccount.Id;
            }
        }

        public TransitRefererAccount TransitRefererAccount
        {
            get
            {
                return new TransitRefererAccount(mRefererAccount);
            }
        }

        public void CreateOrUpdate(TransitRefererAccount o)
        {
            mRefererAccount = o.GetRefererAccount(Session);
            mRefererAccount.Modified = DateTime.UtcNow;
            if (Id == 0) mRefererAccount.Created = mRefererAccount.Modified;
            Session.Save(mRefererAccount);
        }

        public void Delete()
        {
            Session.Delete(mRefererAccount);
        }
    }
}
