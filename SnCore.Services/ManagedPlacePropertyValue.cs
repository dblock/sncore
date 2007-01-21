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
    public class TransitDistinctPlacePropertyValue
    {
        private string mValue;

        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        private int mCount;

        public int Count
        {
            get
            {
                return mCount;
            }
            set
            {
                mCount = value;
            }
        }
    }

    public class TransitPlacePropertyValue : TransitService<PlacePropertyValue>
    {
        private int mPlaceId;

        public int PlaceId
        {
            get
            {
                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
            }
        }

        private int mPlacePropertyId;

        public int PlacePropertyId
        {
            get
            {
                return mPlacePropertyId;
            }
            set
            {
                mPlacePropertyId = value;
            }
        }

        private string mPlacePropertyDescription;

        public string PlacePropertyDescription
        {
            get
            {
                return mPlacePropertyDescription;
            }
            set
            {
                mPlacePropertyDescription = value;
            }
        }

        private string mPlacePropertyName;

        public string PlacePropertyName
        {
            get
            {
                return mPlacePropertyName;
            }
            set
            {
                mPlacePropertyName = value;
            }
        }

        private string mPlacePropertyTypeName;

        public string PlacePropertyTypeName
        {
            get
            {
                return mPlacePropertyTypeName;
            }
            set
            {
                mPlacePropertyTypeName = value;
            }
        }

        private int mPlacePropertyGroupId;

        public int PlacePropertyGroupId
        {
            get
            {
                return mPlacePropertyGroupId;
            }
            set
            {
                mPlacePropertyGroupId = value;
            }
        }

        private string mPlacePropertyGroupName;

        public string PlacePropertyGroupName
        {
            get
            {
                return mPlacePropertyGroupName;
            }
            set
            {
                mPlacePropertyGroupName = value;
            }
        }

        private string mValue;

        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
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

        public TransitPlacePropertyValue()
        {

        }

        public TransitPlacePropertyValue(PlacePropertyValue instance)
            : base(instance)
        {

        }

        public override void SetInstance(PlacePropertyValue instance)
        {
            if (instance.Place != null) PlaceId = instance.Place.Id;
            PlacePropertyId = instance.PlaceProperty.Id;
            PlacePropertyGroupId = instance.PlaceProperty.PlacePropertyGroup.Id;
            PlacePropertyGroupName = instance.PlaceProperty.PlacePropertyGroup.Name;
            PlacePropertyTypeName = instance.PlaceProperty.TypeName;
            PlacePropertyName = instance.PlaceProperty.Name;
            PlacePropertyDescription = instance.PlaceProperty.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            Value = instance.Value;
            base.SetInstance(instance);
        }

        public override PlacePropertyValue GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePropertyValue instance = base.GetInstance(session, sec);
            instance.Place = (this.PlaceId > 0) ? session.Load<Place>(PlaceId) : null;
            instance.PlaceProperty = (PlacePropertyId > 0) ? session.Load<PlaceProperty>(PlacePropertyId) : null;
            instance.Value = this.Value;
            return instance;
        }
    }

    public class ManagedPlacePropertyValue : ManagedService<PlacePropertyValue, TransitPlacePropertyValue>
    {
        public ManagedPlacePropertyValue()
        {

        }

        public ManagedPlacePropertyValue(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePropertyValue(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlacePropertyValue(ISession session, PlacePropertyValue value)
            : base(session, value)
        {

        }

        public int PlaceId
        {
            get
            {
                return mInstance.Place.Id;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
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
