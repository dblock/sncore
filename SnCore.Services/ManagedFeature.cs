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
            DataObjectName = (session.Load<DataObject>(instance.DataObject.Id)).Name;
        }

        public TransitFeature(Feature instance)
            : base(instance)
        {

        }

        public override void SetInstance(Feature instance)
        {
            DataRowId = instance.DataRowId;
            DataObjectName = instance.DataObject.Name;
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
        public ManagedFeature()
        {

        }

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
            bool fNew = (mInstance.Id == 0);
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);

            if (fNew)
            {
                Session.Flush();

                ManagedAccount acct = new ManagedAccount(Session, GetInstanceAccount());
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, acct, string.Format("EmailFeature.aspx?id={0}&aid={1}", mInstance.Id, acct.Id));
            }
        }

        public Account GetInstanceAccount()
        {
            IDbObject instance = GetInstance();
            if (instance is Account)
            {
                return (Account) instance;
            }
            else if (instance is IDbObject)
            {
                return (Account) instance.GetType().BaseType.GetProperty("Account").GetValue(instance, null);
            }
            else
            {
                throw new Exception(string.Format("Unsupported type: {0}", instance.GetType().FullName));
            }
        }

        public IDbObject GetInstance()
        {
            Type objecttype = Assembly.GetAssembly(typeof(Account)).GetType(mInstance.DataObject.Name);
            return (IDbObject) Session.Load(objecttype, mInstance.DataRowId);
        }

        public T GetInstance<T>()
        {
            return Session.Load<T>(mInstance.DataRowId);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
