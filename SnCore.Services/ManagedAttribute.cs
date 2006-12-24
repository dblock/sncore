using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools;

namespace SnCore.Services
{
    public class AttributeQueryOptions
    {
        public bool WithBitmap = false;

        public AttributeQueryOptions()
        {
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    }

    public class TransitAttributeWithBitmap : TransitAttribute
    {
        public byte[] Bitmap;

        public TransitAttributeWithBitmap()
        {

        }

        public TransitAttributeWithBitmap(Attribute b)
            : base(b)
        {
            Bitmap = b.Bitmap;
        }

        public override Attribute GetAttribute(ISession session)
        {
            Attribute b = base.GetAttribute(session);
            if (this.Bitmap != null) b.Bitmap = this.Bitmap;
            return b;
        }
    }

    public class TransitAttribute : TransitService
    {
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

        public TransitAttribute(Attribute b)
            : base(b.Id)
        {
            Name = b.Name;
            Description = b.Description;
            DefaultUrl = b.DefaultUrl;
            DefaultValue = b.DefaultValue;
            Created = b.Created;
            Modified = b.Modified;
            mHasBitmap = (b.Bitmap != null);
        }

        public virtual Attribute GetAttribute(ISession session)
        {
            Attribute b = (Id > 0) ? (Attribute)session.Load(typeof(Attribute), Id) : new Attribute();
            b.Name = this.Name;
            b.Description = this.Description;
            b.DefaultUrl = this.DefaultUrl;
            b.DefaultValue = this.DefaultValue;
            return b;
        }
    }

    /// <summary>
    /// Managed attributes.
    /// </summary>
    public class ManagedAttribute : ManagedService<Attribute>
    {
        private Attribute mAttribute = null;

        public ManagedAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedAttribute(ISession session, int id)
            : base(session)
        {
            mAttribute = (Attribute)session.Load(typeof(Attribute), id);
        }

        public ManagedAttribute(ISession session, Attribute value)
            : base(session)
        {
            mAttribute = value;
        }

        public int Id
        {
            get
            {
                return mAttribute.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAttribute.Name;
            }
        }

        public string Description
        {
            get
            {
                return mAttribute.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mAttribute.Bitmap;
            }
        }

        public string DefaultUrl
        {
            get
            {
                return mAttribute.DefaultUrl;
            }
        }

        public string DefaultValue
        {
            get
            {
                return mAttribute.DefaultValue;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAttribute.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAttribute.Modified;
            }
        }

        public TransitAttributeWithBitmap TransitAttributeWithBitmap
        {
            get
            {
                TransitAttributeWithBitmap pic = new TransitAttributeWithBitmap(mAttribute);
                return pic;
            }
        }

        public TransitAttribute TransitAttribute
        {
            get
            {
                return new TransitAttribute(mAttribute);
            }
        }

        public void CreateOrUpdate(TransitAttribute o)
        {
            mAttribute = o.GetAttribute(Session);
            mAttribute.Modified = DateTime.UtcNow;
            if (Id == 0) mAttribute.Created = mAttribute.Modified;
            Session.Save(mAttribute);
        }

        public void Delete()
        {
            Session.Delete(mAttribute);
        }
    }
}
