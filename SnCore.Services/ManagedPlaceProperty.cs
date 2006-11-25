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
    public class TransitPlaceProperty : TransitService
    {
        private bool mPublish;

        public bool Publish
        {
            get
            {
                return mPublish;
            }
            set
            {
                mPublish = value;
            }
        }

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

        private string mDescription;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        private Type mType;

        public Type Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        private string mDefaultValue;

        public string DefaultValue
        {
            get
            {
                return mDefaultValue;
            }
            set
            {
                mDefaultValue = value;
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

        private int mPlacePropertyGroupId = 0;

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

        public TransitPlaceProperty()
        {

        }

        public TransitPlaceProperty(PlaceProperty o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Type = Type.GetType(o.TypeName);
            DefaultValue = o.DefaultValue;
            Publish = o.Publish;
            PlacePropertyGroupId = o.PlacePropertyGroup.Id;
            PlacePropertyGroupName = o.PlacePropertyGroup.Name;
        }

        public PlaceProperty GetPlaceProperty(ISession session)
        {
            PlaceProperty p = (Id != 0) ? (PlaceProperty)session.Load(typeof(PlaceProperty), Id) : new PlaceProperty();
            if (PlacePropertyGroupId > 0) p.PlacePropertyGroup = (PlacePropertyGroup)session.Load(typeof(PlacePropertyGroup), PlacePropertyGroupId);
            else if (!string.IsNullOrEmpty(PlacePropertyGroupName)) p.PlacePropertyGroup = ManagedPlacePropertyGroup.Find(session, PlacePropertyGroupName);
            p.Name = this.Name;
            p.Description = this.Description;
            p.DefaultValue = this.DefaultValue;
            p.TypeName = this.Type.ToString();
            p.Publish = this.Publish;
            return p;
        }
    }

    public class ManagedPlaceProperty : ManagedService
    {
        private PlaceProperty mPlaceProperty = null;

        public ManagedPlaceProperty(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceProperty(ISession session, int id)
            : base(session)
        {
            mPlaceProperty = (PlaceProperty)session.Load(typeof(PlaceProperty), id);
        }

        public ManagedPlaceProperty(ISession session, PlaceProperty value)
            : base(session)
        {
            mPlaceProperty = value;
        }

        public ManagedPlaceProperty(ISession session, TransitPlaceProperty value)
            : base(session)
        {
            mPlaceProperty = value.GetPlaceProperty(session);
        }

        public int Id
        {
            get
            {
                return mPlaceProperty.Id;
            }
        }

        public TransitPlaceProperty TransitPlaceProperty
        {
            get
            {
                return new TransitPlaceProperty(mPlaceProperty);
            }
        }

        public void CreateOrUpdate(TransitPlaceProperty o)
        {
            mPlaceProperty = o.GetPlaceProperty(Session);
            Session.Save(mPlaceProperty);
        }

        public void Delete()
        {
            Session.Delete(mPlaceProperty);
        }
    }
}
