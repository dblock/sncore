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
    public class TransitCampaign : TransitService
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

        public TransitCampaign(Campaign c)
            : base(c.Id)
        {
            Name = c.Name;
            Description = c.Description;
            SenderName = c.SenderName;
            SenderEmailAddress = c.SenderEmailAddress;
            Url = c.Url;
            Active = c.Active;
            Created = c.Created;
            Modified = c.Modified;
            Processed = c.Processed;
        }

        public Campaign GetCampaign(ISession session)
        {
            Campaign p = (Id != 0) ? (Campaign)session.Load(typeof(Campaign), Id) : new Campaign();
            p.Name = this.Name;
            p.Description = this.Description;
            p.SenderName = this.SenderName;
            p.SenderEmailAddress = this.SenderEmailAddress;
            p.Url = this.Url;
            p.Active = this.Active;
            return p;
        }
    }

    /// <summary>
    /// Managed Campaign.
    /// </summary>
    public class ManagedCampaign : ManagedService<Campaign>
    {
        private Campaign mCampaign = null;

        public ManagedCampaign(ISession session)
            : base(session)
        {

        }

        public ManagedCampaign(ISession session, int id)
            : base(session)
        {
            mCampaign = (Campaign)session.Load(typeof(Campaign), id);
        }

        public ManagedCampaign(ISession session, Campaign value)
            : base(session)
        {
            mCampaign = value;
        }

        public ManagedCampaign(ISession session, TransitCampaign value)
            : base(session)
        {
            mCampaign = value.GetCampaign(session);
        }

        public int Id
        {
            get
            {
                return mCampaign.Id;
            }
        }

        public TransitCampaign TransitCampaign
        {
            get
            {
                return new TransitCampaign(mCampaign);
            }
        }

        public void CreateOrUpdate(TransitCampaign c)
        {
            mCampaign = c.GetCampaign(Session);
            mCampaign.Modified = DateTime.UtcNow;
            if (mCampaign.Id == 0) mCampaign.Processed = mCampaign.Created = mCampaign.Modified;
            Session.Save(mCampaign);
        }

        public void Delete()
        {
            Session.Delete(mCampaign);
        }
    }
}
