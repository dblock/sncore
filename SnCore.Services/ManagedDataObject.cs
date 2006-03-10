using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services
{

    public class TransitDataObject : TransitService
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

        public TransitDataObject(DataObject o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public DataObject GetDataObject(ISession session)
        {
            DataObject p = (Id != 0) ? (DataObject)session.Load(typeof(DataObject), Id) : new DataObject();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed data object.
    /// </summary>
    public class ManagedDataObject : ManagedService
    {
        private DataObject mDataObject = null;

        public ManagedDataObject(ISession session)
            : base(session)
        {

        }

        public ManagedDataObject(ISession session, int id)
            : base(session)
        {
            mDataObject = (DataObject)session.Load(typeof(DataObject), id);
        }

        public ManagedDataObject(ISession session, DataObject value)
            : base(session)
        {
            mDataObject = value;
        }

        public int Id
        {
            get
            {
                return mDataObject.Id;
            }
        }

        public string Name
        {
            get
            {
                return mDataObject.Name;
            }
        }

        public TransitDataObject TransitDataObject
        {
            get
            {
                return new TransitDataObject(mDataObject);
            }
        }

        public static DataObject FindObject(ISession session, string name)
        {
            return (DataObject) session.CreateCriteria(typeof(DataObject))
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
    }
}
