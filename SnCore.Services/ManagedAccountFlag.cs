using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountFlag : TransitService<AccountFlag>
    {
        private string mUrl;

        public string Url
        {
            get
            {
                return mUrl;
            }
            set
            {
                mUrl = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private int mFlaggedAccountId;

        public int FlaggedAccountId
        {
            get
            {
                return mFlaggedAccountId;
            }
            set
            {
                mFlaggedAccountId = value;
            }
        }

        private int mFlaggedAccountPictureId;

        public int FlaggedAccountPictureId
        {
            get
            {
                return mFlaggedAccountPictureId;
            }
            set
            {
                mFlaggedAccountPictureId = value;
            }
        }

        private string mFlaggedAccountName;

        public string FlaggedAccountName
        {
            get
            {
                return mFlaggedAccountName;
            }
            set
            {
                mFlaggedAccountName = value;
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

        private string mAccountFlagType;

        public string AccountFlagType
        {
            get
            {
                return mAccountFlagType;
            }
            set
            {
                mAccountFlagType = value;
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

        public TransitAccountFlag()
        {

        }

        public TransitAccountFlag(AccountFlag instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountFlag instance)
        {
            Description = instance.Description;
            AccountId = instance.Account.Id;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account); 
            AccountName = instance.Account.Name;
            FlaggedAccountId = instance.FlaggedAccount.Id;
            FlaggedAccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.FlaggedAccount);
            FlaggedAccountName = instance.FlaggedAccount.Name;
            AccountFlagType = instance.AccountFlagType.Name;
            Url = instance.Url;
            Created = instance.Created;            
            base.SetInstance(instance);
        }

        public override AccountFlag GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFlag instance = base.GetInstance(session, sec);
            
            if (Id == 0)
            {
                instance.Account = base.GetOwner(session, AccountId, sec);
                instance.FlaggedAccount = session.Load<Account>(FlaggedAccountId);
                instance.AccountFlagType = ManagedAccountFlagType.Find(session, mAccountFlagType);
            }

            instance.Url = this.Url;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedAccountFlag : ManagedService<AccountFlag, TransitAccountFlag>
    {
        public class AccountFlaggedException : Exception
        {
            public AccountFlaggedException()
                : base("Your account activity has been flagged by other users and is temporarily blocked.")
            {

            }
        }

        // how many people does it take to block someone from sending?
        public const int DefaultAccountFlagThreshold = 5; // TODO: export into configuration settings

        public ManagedAccountFlag()
        {

        }

        public ManagedAccountFlag(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFlag(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFlag(ISession session, AccountFlag value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountFlag>.GetSafeCollection(mInstance.Account.AccountFlags).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Check(TransitAccountFlag t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);

            if (t_instance.AccountId == t_instance.FlaggedAccountId)
            {
                throw new Exception("You cannot flag yourself.");
            }

            AccountFlag existing = Session.CreateCriteria(typeof(AccountFlag))
                .Add(Expression.Eq("Account.Id", mInstance.Account.Id))
                .Add(Expression.Eq("FlaggedAccount.Id", mInstance.FlaggedAccount.Id))
                .UniqueResult<AccountFlag>();

            if (existing != null)
            {
                throw new Exception(string.Format("You have already reported {0} from {1}. It will be checked shortly.",
                    existing.AccountFlagType.Name, existing.FlaggedAccount.Name));
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            bool fNew = (mInstance.Id == 0);
            if (fNew) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);

            if (fNew)
            {
                ManagedAccount admin = new ManagedAccount(Session, ManagedAccount.GetAdminAccount(Session));
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, admin,
                    string.Format("EmailAccountFlag.aspx?id={0}", mInstance.Id));
            }
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        public static IList<AccountFlag> GetAccountFlagsByFlaggedAccountId(ISession session, int id)
        {
            return session.CreateCriteria(typeof(AccountFlag))
                .Add(Expression.Eq("FlaggedAccount.Id", id))
                .List<AccountFlag>();
        }
    }
}
