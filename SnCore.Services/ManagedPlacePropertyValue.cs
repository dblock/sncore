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

        private TransitPlaceProperty mPlaceProperty;

        public TransitPlaceProperty PlaceProperty
        {
            get
            {
                return mPlaceProperty;
            }
            set
            {
                mPlaceProperty = value;
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
            PlaceProperty = new TransitPlaceProperty(instance.PlaceProperty);
            Created = instance.Created;
            Modified = instance.Modified;
            Value = instance.Value;
            base.SetInstance(instance);
        }

        public override PlacePropertyValue GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePropertyValue instance = base.GetInstance(session, sec);
            instance.Place = (this.PlaceId > 0) ? (Place)session.Load(typeof(Place), PlaceId) : null;
            instance.PlaceProperty = (this.PlaceProperty != null && this.PlaceProperty.Id > 0) ? (PlaceProperty)session.Load(typeof(PlaceProperty), this.PlaceProperty.Id) : null;
            instance.Value = this.Value;
            return instance;
        }
    }

    public class ManagedPlacePropertyValue : ManagedService<PlacePropertyValue, TransitPlacePropertyValue>
    {
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
