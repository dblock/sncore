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
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitMadLibInstance : TransitService
    {
        private string mText;

        public string Text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
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

        private string mObjectName;

        public string ObjectName
        {
            get
            {
                return mObjectName;
            }
            set
            {
                mObjectName = value;
            }
        }

        private int mObjectId;

        public int ObjectId
        {
            get
            {
                return mObjectId;
            }
            set
            {
                mObjectId = value;
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

        private int mObjectAccountId = 0;

        public int ObjectAccountId
        {
            get
            {
                return mObjectAccountId;
            }
            set
            {
                mObjectAccountId = value;
            }
        }

        private string mObjectUri = string.Empty;

        public string ObjectUri
        {
            get
            {
                return mObjectUri;
            }
            set
            {
                mObjectUri = value;
            }
        }

        private string mAccountName;

        public string AccountName
        {
            get
            {
                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private int mAccountPictureId;

        public int AccountPictureId
        {
            get
            {
                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
            }
        }

        private int mMadLibId;

        public int MadLibId
        {
            get
            {
                return mMadLibId;
            }
            set
            {
                mMadLibId = value;
            }
        }

        public TransitMadLibInstance()
        {

        }

        public TransitMadLibInstance(MadLibInstance c)
            : base(c.Id)
        {
            AccountId = c.AccountId;
            Text = c.Text;
            ObjectId = c.ObjectId;
            ObjectName = c.DataObject.Name;
            MadLibId = c.MadLib.Id;
            Created = c.Created;
            Modified = c.Modified;
        }

        public MadLibInstance GetMadLibInstance(ISession session)
        {
            MadLibInstance p = (Id != 0) ? (MadLibInstance)session.Load(typeof(MadLibInstance), Id) : new MadLibInstance();
            if (Id == 0) p.MadLib = (MadLib)session.Load(typeof(MadLib), this.MadLibId);
            p.Text = this.Text;
            p.ObjectId = this.ObjectId;
            p.DataObject = ManagedDataObject.FindObject(session, this.ObjectName);
            p.AccountId = this.AccountId;
            return p;
        }
    }

    /// <summary>
    /// Managed MadLibInstance.
    /// </summary>
    public class ManagedMadLibInstance : ManagedService
    {
        private MadLibInstance mMadLibInstance = null;

        public ManagedMadLibInstance(ISession session)
            : base(session)
        {

        }

        public ManagedMadLibInstance(ISession session, int id)
            : base(session)
        {
            mMadLibInstance = (MadLibInstance)session.Load(typeof(MadLibInstance), id);
        }

        public ManagedMadLibInstance(ISession session, MadLibInstance value)
            : base(session)
        {
            mMadLibInstance = value;
        }

        public ManagedMadLibInstance(ISession session, TransitMadLibInstance value)
            : base(session)
        {
            mMadLibInstance = value.GetMadLibInstance(session);
        }

        public int Id
        {
            get
            {
                return mMadLibInstance.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mMadLibInstance.AccountId;
            }
        }

        public TransitMadLibInstance TransitMadLibInstance
        {
            get
            {
                TransitMadLibInstance instance = new TransitMadLibInstance(mMadLibInstance);

                try
                {
                    Account acct = (Account)Session.Load(typeof(Account), mMadLibInstance.AccountId);
                    instance.AccountName = (acct != null) ? acct.Name : "Unknown User";
                    instance.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(acct);
                }
                catch (ObjectNotFoundException)
                {

                }

                return instance;
            }
        }

        public void CreateOrUpdate(TransitMadLibInstance c)
        {
            mMadLibInstance = c.GetMadLibInstance(Session);
            mMadLibInstance.Modified = DateTime.UtcNow;
            
            if (mMadLibInstance.Id == 0)
            {
                mMadLibInstance.Created = mMadLibInstance.Modified;
            }

            Session.Save(mMadLibInstance);
            Session.Flush();

            if (c.Id == 0 && c.ObjectAccountId > 0)
            {
                try
                {
                    ManagedAccount ra = new ManagedAccount(Session, c.AccountId);
                    ManagedAccount ma = new ManagedAccount(Session, c.ObjectAccountId);

                    if (ra.Id != ma.Id)
                    {
                        string replyTo = ma.ActiveEmailAddress;
                        if (!string.IsNullOrEmpty(replyTo))
                        {
                            ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                                Session,
                                new MailAddress(replyTo, ma.Name).ToString(),
                                string.Format("EmailAccountMadLibInstance.aspx?aid={0}&ObjectName={1}&oid={2}&mid={3}&id={4}&ReturnUrl={5}",
                                    c.ObjectAccountId, mMadLibInstance.DataObject.Name, mMadLibInstance.ObjectId, 
                                    mMadLibInstance.MadLib.Id, mMadLibInstance.Id, Renderer.UrlEncode(c.ObjectUri)));
                        }
                    }
                }
                catch (ObjectNotFoundException)
                {
                    // replying to an account that does not exist
                }
            }
            
        }

        public void Delete()
        {
            Session.Delete(mMadLibInstance);
        }
    }
}
