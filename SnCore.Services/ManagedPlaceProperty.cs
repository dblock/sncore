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
    public class TransitPlaceProperty : TransitService<PlaceProperty>
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

        public TransitPlaceProperty(PlaceProperty value)
            : base(value)
        {

        }

        public override void SetInstance(PlaceProperty instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            Type = Type.GetType(instance.TypeName);
            DefaultValue = instance.DefaultValue;
            Publish = instance.Publish;
            PlacePropertyGroupId = instance.PlacePropertyGroup.Id;
            PlacePropertyGroupName = instance.PlacePropertyGroup.Name;
            base.SetInstance(instance);
        }

        public override PlaceProperty GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceProperty instance = (Id != 0) ? (PlaceProperty)session.Load(typeof(PlaceProperty), Id) : new PlaceProperty();
            if (PlacePropertyGroupId > 0) instance.PlacePropertyGroup = (PlacePropertyGroup)session.Load(typeof(PlacePropertyGroup), PlacePropertyGroupId);
            else if (!string.IsNullOrEmpty(PlacePropertyGroupName)) instance.PlacePropertyGroup = ManagedPlacePropertyGroup.Find(session, PlacePropertyGroupName);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.DefaultValue = this.DefaultValue;
            instance.TypeName = this.Type.ToString();
            instance.Publish = this.Publish;
            return instance;
        }
    }

    public class ManagedPlaceProperty : ManagedService<PlaceProperty, TransitPlaceProperty>
    {
        public ManagedPlaceProperty(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceProperty(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceProperty(ISession session, PlaceProperty value)
            : base(session, value)
        {

        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
