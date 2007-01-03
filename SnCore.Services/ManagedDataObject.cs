using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services
{
    public class TransitDataObject : TransitService<DataObject>
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

        public TransitDataObject()
        {

        }

        public TransitDataObject(DataObject instance)
            : base(instance)
        {

        }

        public override void SetInstance(DataObject instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override DataObject GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DataObject instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedDataObject : ManagedService<DataObject, TransitDataObject>
    {
        public ManagedDataObject(ISession session)
            : base(session)
        {

        }

        public ManagedDataObject(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedDataObject(ISession session, DataObject instance)
            : base(session, instance)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public static DataObject FindObject(ISession session, string name)
        {
            return (DataObject)session.CreateCriteria(typeof(DataObject))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int Find(ISession session, string name)
        {
            DataObject dtao = FindObject(session, name);

            if (dtao == null)
            {
                return 0;
            }

            return dtao.Id;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
