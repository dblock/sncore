using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;

namespace SnCore.Services
{
    public interface ITransitService
    {
        int Id { get; set; }
        IDbObject GetDbObjectInstance(ISession session, ManagedSecurityContext sec);
        void SetDbObjectInstance(IDbObject value);
    }

    [Serializable()]
    public abstract class TransitService<DatabaseType> : ITransitService
        where DatabaseType: IDbObject, new()
    {
        private int mId;

        public TransitService()
        {
            Id = 0;
        }

        public TransitService(int id)
        {
            Id = id;
        }

        public TransitService(DatabaseType o)
        {
            Id = o.Id;
            SetInstance(o);
        }

        public virtual void SetInstance(IDbObject instance)
        {
            SetInstance((DatabaseType) instance);
        }

        public virtual void SetInstance(DatabaseType instance)
        {
            Id = instance.Id;
        }

        public int Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        public IDbObject GetDbObjectInstance(ISession session, ManagedSecurityContext sec)
        {
            return GetInstance(session, sec);
        }

        public void SetDbObjectInstance(IDbObject value)
        {
            SetInstance(value);
        }

        public virtual DatabaseType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DatabaseType o = (Id != 0) ? (DatabaseType)session.Load(typeof(DatabaseType), Id) : new DatabaseType();
            return o;
        }

        public Account GetOwner(ISession session, int id, ManagedSecurityContext sec)
        {
            if (id == 0) return sec.Account; // current security context id
            if (id != 0 && id == sec.Account.Id) return sec.Account; // the id requested matches the security context id
            if (id != 0 && sec.IsAdministrator()) return (Account) session.Load(typeof(Account), id); // the administrator can get any id
            if (id != 0 && !sec.IsAdministrator()) throw new ManagedAccount.AccessDeniedException(); // attempt to change ownership
            return sec.Account; // whatever is in the security context
        }
    }

    public class TransitServiceCollection<TransitType>
        where TransitType : ITransitService
    {
        private IEnumerable<ITransitService> mCollection = null;

        public TransitServiceCollection()
        {

        }

        public IEnumerable<ITransitService> Collection
        {
            get
            {
                return mCollection;
            }
            set
            {
                mCollection = value;
            }
        }

        public TransitServiceCollection(IEnumerable<ITransitService> value)
        {
            mCollection = value;
        }

        public bool Contains(ITransitService item)
        {
            return ContainsId(item.Id);
        }

        public bool ContainsId(int id)
        {
            if (mCollection == null)
                return false;

            foreach (ITransitService instance in mCollection)
                if (id == instance.Id)
                    return true;

            return false;
        }
    }
}
