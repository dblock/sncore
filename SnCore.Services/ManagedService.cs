using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using System.Web.Hosting;

namespace SnCore.Services
{
    public interface IManagedService
    {
        ISession Session { get; set; }
        IDbObject DbObjectInstance { get; set; }
        int CreateOrUpdateDbObject(ITransitService t_instance, ManagedSecurityContext sec);
        void Delete(ManagedSecurityContext sec);
        int Id { get; }
        void LoadInstance(ISession session, int id);
        void SetDbObjectInstance(ISession session, IDbObject instance);
        ITransitService GetTransitServiceInstance(ManagedSecurityContext sec);
        ACL GetACL();
    }

    public class ManagedServiceImpl
    {
        protected ISession mSession = null;

        public ISession Session
        {
            get
            {
                return mSession;
            }
            set
            {
                mSession = value;
            }
        }

        public ManagedServiceImpl()
        {

        }

        public ManagedServiceImpl(ISession session)
        {
            mSession = session;
        }
    }

    public class ManagedService<DatabaseType, TransitType> : ManagedServiceImpl, IManagedService
        where DatabaseType : IDbObject, new()
        where TransitType : ITransitService, new()
    {
        protected DatabaseType mInstance = default(DatabaseType);

        public ManagedService()
        {

        }

        public ManagedService(ISession session)
            : base(session)
        {

        }

        public ManagedService(ISession session, int id)
            : base(session)
        {
            LoadInstance(session, id);
        }

        public ManagedService(ISession session, DatabaseType instance)
            : base(session)
        {
            Instance = instance;
        }

        public ManagedService(ISession session, TransitType transitobject, ManagedSecurityContext sec)
            : base(session)
        {
            Instance = (DatabaseType) transitobject.GetDbObjectInstance(session, sec);
        }

        public void SetDbObjectInstance(ISession session, IDbObject instance)
        {
            SetInstance(session, (DatabaseType) instance);
        }

        public virtual void SetInstance(ISession session, DatabaseType instance)
        {
            Session = session;
            Instance = instance;
        }

        public virtual void LoadInstance(ISession session, int id)
        {
            Session = session;
            Instance = Session.Load<DatabaseType>(id);
        }

        public ITransitService GetTransitServiceInstance(ManagedSecurityContext sec)
        {
            return (ITransitService) GetTransitInstance(sec);
        }

        public virtual TransitType GetTransitInstance(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);
            TransitType t_instance = new TransitType();
            t_instance.SetDbObjectInstance(Instance);
            return t_instance;
        }

        public DatabaseType Instance
        {
            get
            {
                return mInstance;
            }
            set
            {
                mInstance = value;
            }
        }

        public virtual void Delete(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Delete);
            Session.Delete(mInstance);
            
            if (this is IAuditableService)
            {
                IList<AccountAuditEntry> audit_entries = ((IAuditableService)this).CreateAccountAuditEntries(
                    Session, sec, DataOperation.Delete);
                
                if (audit_entries != null)
                    foreach(AccountAuditEntry audit_entry in audit_entries)
                        Session.Save(audit_entry);
            }
        }

        public virtual int CreateOrUpdate(TransitType t_instance, ManagedSecurityContext sec)
        {
            mInstance = (DatabaseType)t_instance.GetDbObjectInstance(Session, sec);
            CheckQuota(t_instance, sec);
            Save(sec);
            return mInstance.Id;
        }

        private void CheckQuota(TransitType t_instance, ManagedSecurityContext sec)
        {
            try
            {
                Check(t_instance, sec);
            }
            catch (ManagedAccount.QuotaExceededException)
            {
                ManagedAccount admin = new ManagedAccount(Session, ManagedAccount.GetAdminAccount(Session));
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, admin,
                    string.Format("EmailAccountQuotaExceeded.aspx?id={0}", sec.Account.Id));
                throw;
            }
        }

        protected virtual void Check(TransitType t_instance, ManagedSecurityContext sec)
        {
            // check permissions
            GetACL().Check(sec, t_instance.Id > 0 ? DataOperation.Update : DataOperation.Create);
        }

        protected virtual ManagedQuota GetQuota(ManagedSecurityContext sec)
        {
            // check whether the default quota was overridden
            IQuery query = Session.CreateQuery(string.Format(
                "FROM AccountQuota AccountQuota WHERE AccountQuota.Account.Id = {0} AND AccountQuota.DataObject.Name = '{1}'", 
                sec.Account.Id, typeof(DatabaseType).Name));
            
            AccountQuota q = query.UniqueResult<AccountQuota>();

            if (q != null)
            {
                return new ManagedQuota(q.Limit);
            }

            return ManagedQuota.GetDefaultEnabledQuota();
        }

        protected virtual void Save(ManagedSecurityContext sec)
        {
            DataOperation op = (mInstance.Id == 0 ? DataOperation.Create : DataOperation.Update);
            Session.Save(mInstance);

            if (this is IAuditableService)
            {
                IList<AccountAuditEntry> audit_entries = ((IAuditableService)this).CreateAccountAuditEntries(
                    Session, sec, op);

                if (audit_entries != null)
                    foreach (AccountAuditEntry audit_entry in audit_entries)
                        Session.Save(audit_entry);
            }
        }

        public int Id
        {
            get
            {
                return Instance.Id;
            }
        }

        public DatabaseType Object
        {
            get
            {
                return (DatabaseType)Instance;
            }
            set
            {
                Instance = value;
            }
        }

        public static int GetRandomElementId(IList<DatabaseType> collection)
        {
            if (collection == null || collection.Count == 0)
                return 0;

            return collection[new Random().Next() % collection.Count].Id;
        }

        public IDbObject DbObjectInstance 
        { 
            get
            {
                return Instance;
            }
            set
            {
                Instance = (DatabaseType) value;
            }
        }

        public int CreateOrUpdateDbObject(ITransitService t_instance, ManagedSecurityContext sec)
        {
            return CreateOrUpdate((TransitType)t_instance, sec);
        }

        public virtual ACL GetACL()
        {
            return GetACL(typeof(DatabaseType));
        }

        public virtual ACL GetACL(Type type)
        {
            return ACL.GetAdministrativeACL(Session);
        }
    }
}
