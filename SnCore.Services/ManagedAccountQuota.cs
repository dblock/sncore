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
using System.Reflection;
using SnCore.Data;
using System.Collections.Generic;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountQuota : TransitService<AccountQuota>
    {
        private string mDataObjectName;
        private int mLimit;
        private DateTime mCreated;
        private DateTime mModified;
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

        public string DataObjectName
        {
            get
            {
                return mDataObjectName;
            }
            set
            {
                mDataObjectName = value;
            }
        }

        public int Limit
        {
            get
            {
                return mLimit;
            }
            set
            {
                mLimit = value;
            }
        }

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

        public TransitAccountQuota()
        {

        }

        public TransitAccountQuota(ISession session, AccountQuota instance)
            : base(instance)
        {
            DataObjectName = (session.Load<DataObject>(instance.DataObject.Id)).Name;
        }

        public TransitAccountQuota(AccountQuota instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountQuota instance)
        {
            AccountId = instance.Account.Id;
            Limit = instance.Limit;
            DataObjectName = instance.DataObject.Name;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override AccountQuota GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountQuota instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = base.GetOwner(session, AccountId, sec);
            instance.Limit = Limit;
            
            if (!string.IsNullOrEmpty(DataObjectName))
            {
                instance.DataObject = (DataObject)session.CreateCriteria(typeof(DataObject))
                    .Add(Expression.Eq("Name", DataObjectName))
                    .UniqueResult();
            }

            return instance;
        }
    }

    public class ManagedAccountQuota : ManagedService<AccountQuota, TransitAccountQuota>
    {
        public ManagedAccountQuota()
        {

        }

        public ManagedAccountQuota(ISession session)
            : base(session)
        {

        }

        public ManagedAccountQuota(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountQuota(ISession session, AccountQuota value)
            : base(session, value)
        {

        }

        public ManagedDataObject DataObject
        {
            get
            {
                return new ManagedDataObject(Session, mInstance.DataObject);
            }
        }

        public override int CreateOrUpdate(TransitAccountQuota t_instance, ManagedSecurityContext sec)
        {
            if (t_instance.Limit <= 0)
            {
                throw new ArgumentOutOfRangeException("Limit", t_instance.Limit, 
                    "Limit must be greater or equal to zero.");
            }

            return base.CreateOrUpdate(t_instance, sec);
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
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.Retreive));
            return acl;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountQuota>.GetSafeCollection(mInstance.Account.AccountQuotas).Remove(mInstance);
            base.Delete(sec);
        }
    }
}
