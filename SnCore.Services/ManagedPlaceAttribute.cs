using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitPlaceAttribute : TransitService<PlaceAttribute>
    {
        private TransitAttribute mAttribute;

        public TransitAttribute Attribute
        {
            get
            {
                return mAttribute;
            }
            set
            {
                mAttribute = value;
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

        private string mUrl;

        public string Url
        {
            get
            {

                return mUrl;
            }
            set
            {
                mUrl = value;
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

        private int mAttributeId;

        public int AttributeId
        {
            get
            {

                return mAttributeId;
            }
            set
            {
                mAttributeId = value;
            }
        }

        public TransitPlaceAttribute()
        {

        }

        public TransitPlaceAttribute(PlaceAttribute value)
            : base(value)
        {
        }

        public override void  SetInstance(PlaceAttribute value)
        {
            PlaceId = value.Place.Id;
            AttributeId = value.Attribute.Id;
            Value = value.Value;
            Url = value.Url;
            Created = value.Created;
            Attribute = new TransitAttribute(value.Attribute);
            base.SetInstance(value);
        }

        public override PlaceAttribute GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceAttribute instance = base.GetInstance(session, sec);
            instance.Url = Url;
            instance.Value = Value;

            if (Id == 0)
            {
                if (PlaceId > 0) instance.Place = (Place)session.Load(typeof(Place), PlaceId);
                if (AttributeId > 0) instance.Attribute = (Attribute)session.Load(typeof(Attribute), AttributeId);
            }
            else
            {
                if (AttributeId != instance.Attribute.Id)
                {
                    throw new InvalidOperationException();
                }

                if (PlaceId != instance.Place.Id)
                {
                    throw new InvalidOperationException();
                }
            }

            return instance;
        }
    }

    public class ManagedPlaceAttribute : ManagedService<PlaceAttribute, TransitPlaceAttribute>
    {
        public ManagedPlaceAttribute()
        {

        }

        public ManagedPlaceAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceAttribute(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceAttribute(ISession session, PlaceAttribute value)
            : base(session, value)
        {

        }

        public string Value
        {
            get
            {
                return mInstance.Value;
            }
        }

        public string Url
        {
            get
            {
                return mInstance.Url;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<PlaceAttribute>.GetSafeCollection(mInstance.Place.PlaceAttributes).Remove(mInstance);
            base.Delete(sec);
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
