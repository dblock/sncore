using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitAttribute : TransitService<Attribute>
    {
        private byte[] mBitmap;

        public byte[] Bitmap
        {
            get
            {
                return mBitmap;
            }
            set
            {
                mBitmap = value;
            }
        }

        private bool mHasBitmap;

        public bool HasBitmap
        {
            get
            {
                return mHasBitmap;
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

        private string mDefaultUrl;

        public string DefaultUrl
        {
            get
            {
                return mDefaultUrl;
            }
            set
            {
                mDefaultUrl = value;
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

        public TransitAttribute()
        {

        }

        public TransitAttribute(Attribute instance)
            : base(instance)
        {
        }

        public override void SetInstance(Attribute instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            DefaultUrl = instance.DefaultUrl;
            DefaultValue = instance.DefaultValue;
            Created = instance.Created;
            Modified = instance.Modified;
            mHasBitmap = (instance.Bitmap != null);
            Bitmap = instance.Bitmap;
            base.SetInstance(instance);
        }

        public override Attribute GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Attribute instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.DefaultUrl = this.DefaultUrl;
            instance.DefaultValue = this.DefaultValue;
            if (Bitmap != null) instance.Bitmap = this.Bitmap;
            return instance;
        }
    }

    public class ManagedAttribute : ManagedService<Attribute, TransitAttribute>
    {
        public ManagedAttribute()
        {

        }

        public ManagedAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedAttribute(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAttribute(ISession session, Attribute value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public string Description
        {
            get
            {
                return mInstance.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mInstance.Bitmap;
            }
        }

        public string DefaultUrl
        {
            get
            {
                return mInstance.DefaultUrl;
            }
        }

        public string DefaultValue
        {
            get
            {
                return mInstance.DefaultValue;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
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
