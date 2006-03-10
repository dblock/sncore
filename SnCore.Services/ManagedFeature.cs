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

    public class TransitFeature : TransitService
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

        public TransitFeature(ISession session, Feature f)
            : base(f.Id)
        {
            DataRowId = f.DataRowId;
            DataObjectName = ((DataObject) session.Load(typeof(DataObject), f.DataObjectId)).Name;
            Created = f.Created;
        }

        public Feature GetFeature(ISession session)
        {
            Feature feature = (Id != 0) ? (Feature) session.Load(typeof(Feature), Id) : new Feature();
            feature.DataRowId = DataRowId;            
            if (! string.IsNullOrEmpty(DataObjectName)) 
                feature.DataObjectId = ((DataObject) session.CreateCriteria(typeof(DataObject))
                    .Add(Expression.Eq("Name", DataObjectName))
                    .UniqueResult()).Id;
            feature.Created = (Id != 0) ? Created : DateTime.UtcNow;
            return feature;
        }
    }

    /// <summary>
    /// Managed feature.
    /// </summary>
    public class ManagedFeature : ManagedService
    {
        private Feature mFeature;

        public ManagedFeature(ISession session)
            : base(session)
        {

        }

        public ManagedFeature(ISession session, int id)
            : base(session)
        {
            mFeature = (Feature)Session.Load(typeof(Feature), id);
        }

        public ManagedFeature(ISession session, Feature value)
            : base(session)
        {
            mFeature = value;
        }

        public int Id
        {
            get
            {
                return mFeature.Id;
            }
        }

        public ManagedDataObject DataObject
        {
            get
            {
                return new ManagedDataObject(Session, mFeature.DataObjectId);
            }
        }

        public void Delete()
        {
            Session.Delete(mFeature);
        }

        public void CreateOrUpdate(TransitFeature f)
        {
            mFeature = f.GetFeature(Session);
            Session.Save(mFeature);
        }

        public TransitFeature TransitFeature
        {
            get
            {
                return new TransitFeature(Session, mFeature);
            }
        }

        public static int GetLatestFeatureId(ISession session, string objectname)
        {            
            return GetLatestFeatureId(session, ManagedDataObject.Find(session, objectname));
        }

        public static int GetLatestFeatureId(ISession session, int objectid)
        {
            Feature feature = (Feature) session.CreateCriteria(typeof(Feature))
                .Add(Expression.Eq("DataObjectId", objectid))
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
                .Add(Expression.Eq("DataObjectId", objectid))
                .Add(Expression.Eq("DataRowId", rowid))
                .AddOrder(Order.Desc("Created"))
                .SetMaxResults(1)
                .UniqueResult();

            if (feature == null)
                return 0;

            return feature.Id;
        }
    }
}
