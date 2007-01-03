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

namespace SnCore.Services
{

    public class TransitFeature : TransitService<Feature>
    {
        private string mDataObjectName;
        private int mDataRowId;
        private DateTime mCreated;

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

        public int DataRowId
        {
            get
            {
                return mDataRowId;
            }
            set
            {
                mDataRowId = value;
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

        public TransitFeature()
        {

        }

        public TransitFeature(ISession session, Feature instance)
            : base(instance)
        {
            DataObjectName = ((DataObject) session.Load(typeof(DataObject), instance.DataObject.Id)).Name;
        }

        public TransitFeature(Feature instance)
            : base(instance)
        {

        }

        public override void SetInstance(Feature instance)
        {
            DataRowId = instance.DataRowId;
            Created = instance.Created;
            base.SetInstance(instance);
        }

        public override Feature GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Feature instance = base.GetInstance(session, sec);
            instance.DataRowId = DataRowId;
            if (!string.IsNullOrEmpty(DataObjectName))
                instance.DataObject = (DataObject)session.CreateCriteria(typeof(DataObject))
                    .Add(Expression.Eq("Name", DataObjectName))
                    .UniqueResult();
            instance.Created = (Id != 0) ? Created : DateTime.UtcNow;
            return instance;
        }
    }

    public class ManagedFeature : ManagedService<Feature, TransitFeature>
    {
        public ManagedFeature(ISession session)
            : base(session)
        {

        }

        public ManagedFeature(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedFeature(ISession session, Feature value)
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

        public static int GetLatestFeatureId(ISession session, string objectname)
        {
            return GetLatestFeatureId(session, ManagedDataObject.Find(session, objectname));
        }

        public static int GetLatestFeatureId(ISession session, int objectid)
        {
            Feature feature = (Feature)session.CreateCriteria(typeof(Feature))
                .Add(Expression.Eq("DataObject.Id", objectid))
                .AddOrder(Order.Desc("Created"))
                .SetMaxResults(1)
                .UniqueResult();

            if (feature == null)
                return 0;

            return feature.Id;
        }

        public static int GetLatestFeatureId(ISession session, string objectname, int rowid)
        {
            return GetLatestFeatureId(session, ManagedDataObject.Find(session, objectname), rowid);
        }

        public static int GetLatestFeatureId(ISession session, int objectid, int rowid)
        {
            Feature feature = (Feature)session.CreateCriteria(typeof(Feature))
                .Add(Expression.Eq("DataObject.Id", objectid))
                .Add(Expression.Eq("DataRowId", rowid))
                .AddOrder(Order.Desc("Created"))
                .SetMaxResults(1)
                .UniqueResult();

            if (feature == null)
                return 0;

            return feature.Id;
        }

        public static int Delete(ISession session, string table, int id)
        {
            return session.Delete(string.Format("from Feature f where f.DataObject.Id = {0} AND f.DataRowId = {1}",
                ManagedDataObject.Find(session, table), id));
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
