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
    public class TransitCampaign : TransitService<Campaign>
    {
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

        private string mSenderName;

        public string SenderName
        {
            get
            {
                return mSenderName;
            }
            set
            {
                mSenderName = value;
            }
        }

        private string mSenderEmailAddress;

        public string SenderEmailAddress
        {
            get
            {
                return mSenderEmailAddress;
            }
            set
            {
                mSenderEmailAddress = value;
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

        private bool mActive;

        public bool Active
        {
            get
            {
                return mActive;
            }
            set
            {
                mActive = value;
            }
        }

        private DateTime mProcessed;

        public DateTime Processed
        {
            get
            {
                return mProcessed;
            }
            set
            {
                mProcessed = value;
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

        public TransitCampaign()
        {

        }

        public TransitCampaign(Campaign instance)
            : base(instance)
        {

        }

        public override void SetInstance(Campaign instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            SenderName = instance.SenderName;
            SenderEmailAddress = instance.SenderEmailAddress;
            Url = instance.Url;
            Active = instance.Active;
            Created = instance.Created;
            Modified = instance.Modified;
            Processed = instance.Processed;
            base.SetInstance(instance);
        }

        public override Campaign GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Campaign instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.SenderName = this.SenderName;
            instance.SenderEmailAddress = this.SenderEmailAddress;
            instance.Url = this.Url;
            instance.Active = this.Active;
            return instance;
        }
    }

    public class ManagedCampaign : ManagedService<Campaign, TransitCampaign>
    {
        public ManagedCampaign()
        {

        }

        public ManagedCampaign(ISession session)
            : base(session)
        {

        }

        public ManagedCampaign(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedCampaign(ISession session, Campaign value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Processed = mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }
}
