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

    public class TransitPlacePropertyValue : TransitService
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

        public TransitPlacePropertyValue(PlacePropertyValue o)
            : base(o.Id)
        {
            if (o.Place != null) PlaceId = o.Place.Id;
            PlaceProperty = new TransitPlaceProperty(o.PlaceProperty);
            Created = o.Created;
            Modified = o.Modified;
            Value = o.Value;
        }

        public PlacePropertyValue GetPlacePropertyValue(ISession session)
        {
            PlacePropertyValue p = (Id != 0) ? (PlacePropertyValue)session.Load(typeof(PlacePropertyValue), Id) : new PlacePropertyValue();
            p.Place = (this.PlaceId > 0) ? (Place)session.Load(typeof(Place), PlaceId) : null;
            p.PlaceProperty = (this.PlaceProperty != null && this.PlaceProperty.Id > 0) ? (PlaceProperty)session.Load(typeof(PlaceProperty), this.PlaceProperty.Id) : null;
            p.Value = this.Value;
            return p;
        }
    }

    public class ManagedPlacePropertyValue : ManagedService<PlacePropertyValue>
    {
        private PlacePropertyValue mPlacePropertyValue = null;

        public ManagedPlacePropertyValue(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePropertyValue(ISession session, int id)
            : base(session)
        {
            mPlacePropertyValue = (PlacePropertyValue)session.Load(typeof(PlacePropertyValue), id);
        }

        public ManagedPlacePropertyValue(ISession session, PlacePropertyValue value)
            : base(session)
        {
            mPlacePropertyValue = value;
        }

        public ManagedPlacePropertyValue(ISession session, TransitPlacePropertyValue value)
            : base(session)
        {
            mPlacePropertyValue = value.GetPlacePropertyValue(session);
        }

        public int Id
        {
            get
            {
                return mPlacePropertyValue.Id;
            }
        }

        public int PlaceId
        {
            get
            {
                return mPlacePropertyValue.Place.Id;
            }
        }

        public TransitPlacePropertyValue TransitPlacePropertyValue
        {
            get
            {
                return new TransitPlacePropertyValue(mPlacePropertyValue);
            }
        }

        public void Delete()
        {
            Session.Delete(mPlacePropertyValue);
        }
    }
}
