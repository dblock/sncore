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
    public class TransitMadLibInstance : TransitService<MadLibInstance>
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

        public TransitMadLibInstance(MadLibInstance instance)
            : base(instance)
        {

        }

        public override void SetInstance(MadLibInstance instance)
        {
            AccountId = instance.AccountId;
            Text = instance.Text;
            ObjectId = instance.ObjectId;
            ObjectName = instance.DataObject.Name;
            MadLibId = instance.MadLib.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override MadLibInstance GetInstance(ISession session, ManagedSecurityContext sec)
        {
            MadLibInstance instance = base.GetInstance(session, sec);
            if (Id == 0) instance.MadLib = session.Load<MadLib>(this.MadLibId);
            instance.Text = this.Text;
            instance.ObjectId = this.ObjectId;
            instance.DataObject = ManagedDataObject.FindObject(session, this.ObjectName);
            instance.AccountId = this.AccountId;
            return instance;
        }
    }

    public class ManagedMadLibInstance : ManagedService<MadLibInstance, TransitMadLibInstance>
    {
        public ManagedMadLibInstance()
        {

        }

        public ManagedMadLibInstance(ISession session)
            : base(session)
        {

        }

        public ManagedMadLibInstance(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedMadLibInstance(ISession session, MadLibInstance value)
            : base(session, value)
        {

        }

        public override TransitMadLibInstance GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitMadLibInstance instance = base.GetTransitInstance(sec);
            instance.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, mInstance.AccountId);
            instance.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, mInstance.AccountId);
            return instance;
        }

        public override int CreateOrUpdate(TransitMadLibInstance instance, ManagedSecurityContext sec)
        {
            base.CreateOrUpdate(instance, sec);

            if (instance.Id == 0 && instance.ObjectAccountId > 0)
            {
                try
                {
                    ManagedAccount ra = new ManagedAccount(Session, instance.AccountId);
                    ManagedAccount ma = new ManagedAccount(Session, instance.ObjectAccountId);

                    if (ra.Id != ma.Id)
                    {
                        Session.Flush();

                        ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, ma,
                            string.Format("EmailAccountMadLibInstance.aspx?aid={0}&ObjectName={1}&oid={2}&mid={3}&id={4}&ReturnUrl={5}",
                                instance.ObjectAccountId, mInstance.DataObject.Name, mInstance.ObjectId,
                                mInstance.MadLib.Id, mInstance.Id, Renderer.UrlEncode(instance.ObjectUri)));
                    }
                }
                catch (ObjectNotFoundException)
                {
                    // replying to an account that does not exist
                }
            }

            return mInstance.Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitMadLibInstance t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerifiedEmail();
        }
    }
}
