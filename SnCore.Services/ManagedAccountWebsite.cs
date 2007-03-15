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
    public class TransitAccountWebsite : TransitService<AccountWebsite>
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

        public TransitAccountWebsite()
        {

        }

        public TransitAccountWebsite(AccountWebsite instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountWebsite instance)
        {
            Url = instance.Url;
            Name = instance.Name;
            Description = instance.Description;
            AccountId = instance.Account.Id;
            base.SetInstance(instance);
        }

        public override AccountWebsite GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountWebsite instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = base.GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Url = this.Url;
            return instance;
        }
    }

    public class ManagedAccountWebsite : ManagedService<AccountWebsite, TransitAccountWebsite>
    {
        public class InvalidUriException : SoapException
        {
            public InvalidUriException()
                : base("Invalid url format.\nPlease make sure it starts with http://.", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedAccountWebsite()
        {

        }

        public ManagedAccountWebsite(ISession session)
            : base(session)
        {

        }

        public ManagedAccountWebsite(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountWebsite(ISession session, AccountWebsite value)
            : base(session, value)
        {

        }

        public string Url
        {
            get
            {
                return mInstance.Url;
            }
        }

        public string Description
        {
            get
            {
                return mInstance.Description;
            }
        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountWebsite>.GetSafeCollection(mInstance.Account.AccountWebsites).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (!Uri.IsWellFormedUriString(mInstance.Url, UriKind.Absolute))
                throw new ManagedAccountWebsite.InvalidUriException();

            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountWebsite t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota().Check(mInstance.Account.AccountWebsites);
        }
    }
}
