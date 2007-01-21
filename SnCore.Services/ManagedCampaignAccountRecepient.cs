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
    public class TransitCampaignAccountRecepient : TransitService<CampaignAccountRecepient>
    {
        private int mCampaignId;

        public int CampaignId
        {
            get
            {
                return mCampaignId;
            }
            set
            {
                mCampaignId = value;
            }
        }

        private int mAccountId;

        public int AccountId
        {
            get
            {
                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private TransitAccount mAccount;

        public TransitAccount Account
        {
            get
            {
                return mAccount;
            }
        }

        private bool mSent;

        public bool Sent
        {
            get
            {
                return mSent;
            }
            set
            {
                mSent = value;
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

        private string mLastError;

        public string LastError
        {
            get
            {
                return mLastError;
            }
            set
            {
                mLastError = value;
            }
        }


        public TransitCampaignAccountRecepient()
        {

        }

        public TransitCampaignAccountRecepient(CampaignAccountRecepient value)
            : base(value)
        {

        }

        public override void SetInstance(CampaignAccountRecepient value)
        {
            CampaignId = value.Campaign.Id;
            AccountId = value.Account.Id;
            Sent = value.Sent;
            Created = value.Created;
            Modified = value.Modified;
            LastError = value.LastError;
            mAccount = new TransitAccount(value.Account);
            base.SetInstance(value);
        }

        public override CampaignAccountRecepient GetInstance(ISession session, ManagedSecurityContext sec)
        {
            CampaignAccountRecepient instance = base.GetInstance(session, sec);
            if (Id == 0)
            {
                instance.Campaign = session.Load<Campaign>(CampaignId);
                instance.Account = GetOwner(session, AccountId, sec);
            }
            instance.Sent = this.Sent;
            return instance;
        }
    }

    public class ManagedCampaignAccountRecepient : ManagedService<CampaignAccountRecepient, TransitCampaignAccountRecepient>
    {
        public ManagedCampaignAccountRecepient()
        {

        }

        public ManagedCampaignAccountRecepient(ISession session)
            : base(session)
        {

        }

        public ManagedCampaignAccountRecepient(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedCampaignAccountRecepient(ISession session, CampaignAccountRecepient value)
            : base(session, value)
        {

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
            return acl;
        }
    }
}
