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
    public class TransitCampaignAccountRecepient : TransitService
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

        public TransitCampaignAccountRecepient(CampaignAccountRecepient c)
            : base(c.Id)
        {
            CampaignId = c.Campaign.Id;
            AccountId = c.Account.Id;
            Sent = c.Sent;
            Created = c.Created;
            Modified = c.Modified;
            LastError = c.LastError;
            mAccount = new TransitAccount(c.Account);
        }

        public CampaignAccountRecepient GetCampaignAccountRecepient(ISession session)
        {
            CampaignAccountRecepient p = (Id != 0) ? (CampaignAccountRecepient)session.Load(typeof(CampaignAccountRecepient), Id) : new CampaignAccountRecepient();
            if (CampaignId > 0) p.Campaign = (Campaign)session.Load(typeof(Campaign), CampaignId);
            if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            p.Sent = this.Sent;
            return p;
        }
    }

    /// <summary>
    /// Managed CampaignAccountRecepient.
    /// </summary>
    public class ManagedCampaignAccountRecepient : ManagedService<CampaignAccountRecepient>
    {
        private CampaignAccountRecepient mCampaignAccountRecepient = null;

        public ManagedCampaignAccountRecepient(ISession session)
            : base(session)
        {

        }

        public ManagedCampaignAccountRecepient(ISession session, int id)
            : base(session)
        {
            mCampaignAccountRecepient = (CampaignAccountRecepient)session.Load(typeof(CampaignAccountRecepient), id);
        }

        public ManagedCampaignAccountRecepient(ISession session, CampaignAccountRecepient value)
            : base(session)
        {
            mCampaignAccountRecepient = value;
        }

        public int Id
        {
            get
            {
                return mCampaignAccountRecepient.Id;
            }
        }

        public TransitCampaignAccountRecepient TransitCampaignAccountRecepient
        {
            get
            {
                return new TransitCampaignAccountRecepient(mCampaignAccountRecepient);
            }
        }

        public void CreateOrUpdate(TransitCampaignAccountRecepient c)
        {
            mCampaignAccountRecepient = c.GetCampaignAccountRecepient(Session);
            mCampaignAccountRecepient.Modified = DateTime.UtcNow;
            if (mCampaignAccountRecepient.Id == 0) mCampaignAccountRecepient.Created = mCampaignAccountRecepient.Modified;
            Session.Save(mCampaignAccountRecepient);
        }

        public void Delete()
        {
            Session.Delete(mCampaignAccountRecepient);
        }
    }
}
