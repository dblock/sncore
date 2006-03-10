using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;

namespace SnCore.Services
{
    public class TransitAccountOpenId : TransitService
    {
        private string mIdentityUrl;

        public string IdentityUrl
        {
            get
            {

                return mIdentityUrl;
            }
            set
            {
                mIdentityUrl = value;
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

        public TransitAccountOpenId()
        {

        }

        public TransitAccountOpenId(AccountOpenId e)
            : base(e.Id)
        {
            AccountId = e.Account.Id;
            IdentityUrl = e.IdentityUrl;
            Created = e.Created;
            Modified = e.Modified;
        }

        public AccountOpenId GetAccountOpenId(ISession session)
        {
            AccountOpenId result = (Id > 0) ? (AccountOpenId)session.Load(typeof(AccountOpenId), Id) : new AccountOpenId();

            if (Id == 0)
            {
                result.IdentityUrl = IdentityUrl.Trim();
                if (AccountId > 0) result.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            return result;
        }
    }

    public class ManagedAccountOpenId : ManagedService
    {
        private AccountOpenId mAccountOpenId = null;

        public ManagedAccountOpenId(ISession session)
            : base(session)
        {

        }

        public ManagedAccountOpenId(ISession session, int id)
            : base(session)
        {
            mAccountOpenId = (AccountOpenId)session.Load(typeof(AccountOpenId), id);
        }

        public ManagedAccountOpenId(ISession session, TransitAccountOpenId tae)
            : base(session)
        {
            mAccountOpenId = tae.GetAccountOpenId(session);
        }

        public ManagedAccountOpenId(ISession session, AccountOpenId value)
            : base(session)
        {
            mAccountOpenId = value;
        }

        public int Id
        {
            get
            {
                return mAccountOpenId.Id;
            }
        }

        public string IdentityUrl
        {
            get
            {
                return mAccountOpenId.IdentityUrl;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountOpenId.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountOpenId.Modified;
            }
        }

        public TransitAccountOpenId TransitAccountOpenId
        {
            get
            {
                return new TransitAccountOpenId(mAccountOpenId);
            }
        }

        public void Delete()
        {
            bool canDelete = false;

            // can delete if more than one openid exists
            if (mAccountOpenId.Account.AccountOpenIds != null && mAccountOpenId.Account.AccountOpenIds.Count > 1)
                canDelete = true;

            // can delete if there're e-mails that allow login
            if (mAccountOpenId.Account.AccountEmails != null && mAccountOpenId.Account.AccountEmails.Count > 0)
                canDelete = true;

            if (!canDelete)
            {
                throw new SoapException(
                    "You cannot delete the last open id.", 
                    SoapException.ClientFaultCode);
            }

            mAccountOpenId.Account.AccountOpenIds.Remove(mAccountOpenId);
            Session.Delete(mAccountOpenId);
        }

        public ManagedAccount Account
        {
            get
            {
                return new ManagedAccount(Session, mAccountOpenId.Account);
            }
        }
    }
}
