using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;

namespace SnCore.Services
{
    public class TransitPlaceAttribute : TransitService
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

        public TransitPlaceAttribute(PlaceAttribute attribute)
            : base(attribute.Id)
        {
            PlaceId = attribute.Place.Id;
            AttributeId = attribute.Attribute.Id;
            Value = attribute.Value;
            Url = attribute.Url;
            Created = attribute.Created;
            Attribute = new TransitAttribute(attribute.Attribute);
        }

        public PlaceAttribute GetPlaceAttribute(ISession session)
        {
            PlaceAttribute result = (Id > 0) ? (PlaceAttribute)session.Load(typeof(PlaceAttribute), Id) : new PlaceAttribute();
            result.Url = Url;
            result.Value = Value;

            if (Id == 0)
            {
                if (PlaceId > 0) result.Place = (Place)session.Load(typeof(Place), PlaceId);
                if (AttributeId > 0) result.Attribute = (Attribute)session.Load(typeof(Attribute), AttributeId);
            }
            else
            {
                if (AttributeId != result.Attribute.Id)
                {
                    throw new InvalidOperationException();
                }

                if (PlaceId != result.Place.Id)
                {
                    throw new InvalidOperationException();
                }
            }

            return result;
        }
    }

    public class ManagedPlaceAttribute : ManagedService
    {
        private PlaceAttribute mPlaceAttribute = null;

        public ManagedPlaceAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceAttribute(ISession session, int id)
            : base(session)
        {
            mPlaceAttribute = (PlaceAttribute)session.Load(typeof(PlaceAttribute), id);
        }

        public ManagedPlaceAttribute(ISession session, TransitPlaceAttribute tae)
            : base(session)
        {
            mPlaceAttribute = tae.GetPlaceAttribute(session);
        }

        public ManagedPlaceAttribute(ISession session, PlaceAttribute value)
            : base(session)
        {
            mPlaceAttribute = value;
        }

        public int Id
        {
            get
            {
                return mPlaceAttribute.Id;
            }
        }

        public string Value
        {
            get
            {
                return mPlaceAttribute.Value;
            }
        }

        public string Url
        {
            get
            {
                return mPlaceAttribute.Url;
            }
        }

        public DateTime Created
        {
            get
            {
                return mPlaceAttribute.Created;
            }
        }

        public TransitPlaceAttribute TransitPlaceAttribute
        {
            get
            {
                return new TransitPlaceAttribute(mPlaceAttribute);
            }
        }

        public void Delete()
        {
            mPlaceAttribute.Place.PlaceAttributes.Remove(mPlaceAttribute);
            Session.Delete(mPlaceAttribute);
        }

        public ManagedPlace Place
        {
            get
            {
                return new ManagedPlace(Session, mPlaceAttribute.Place);
            }
        }
    }
}
