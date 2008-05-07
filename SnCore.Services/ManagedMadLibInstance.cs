using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
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

    public class ManagedMadLibInstance : ManagedAuditableService<MadLibInstance, TransitMadLibInstance>
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

        public override int CreateOrUpdate(TransitMadLibInstance t_instance, ManagedSecurityContext sec)
        {
            Nullable<DateTime> lastModified = new Nullable<DateTime>();
            if (mInstance != null) lastModified = mInstance.Modified;
            int id = base.CreateOrUpdate(t_instance, sec);

            if (t_instance.ObjectAccountId == 0)
                return id;

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, t_instance.AccountId);
                ManagedAccount ma = new ManagedAccount(Session, t_instance.ObjectAccountId);

                // if the author is editing the post, don't notify within 30 minute periods
                if (ra.Id != ma.Id && (t_instance.Id == 0 || 
                    (lastModified.HasValue && lastModified.Value.AddMinutes(30) > DateTime.UtcNow)))
                {
                    Session.Flush();

                    ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, ma,
                        string.Format("EmailAccountMadLibInstance.aspx?aid={0}&ObjectName={1}&oid={2}&mid={3}&id={4}&ReturnUrl={5}",
                            t_instance.ObjectAccountId, mInstance.DataObject.Name, mInstance.ObjectId,
                            mInstance.MadLib.Id, mInstance.Id, Renderer.UrlEncode(t_instance.ObjectUri)));
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }

            return id;
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

            if (t_instance.Id != 0)
                return;

            sec.CheckVerifiedEmail();

            // check whether the sender was flagged
            new ManagedQuota(ManagedAccountFlag.DefaultAccountFlagThreshold).Check<AccountFlag, ManagedAccountFlag.AccountFlaggedException>(
                ManagedAccountFlag.GetAccountFlagsByFlaggedAccountId(Session, sec.Account.Id));
        }

        public static void Delete(ISession session, ManagedSecurityContext sec, string table, int id)
        {
            IList<MadLibInstance> madlibs = GetMadLibs(session, table, id);
            foreach (MadLibInstance madlib in madlibs)
            {
                ManagedMadLibInstance m_instance = new ManagedMadLibInstance(session, madlib);
                m_instance.Delete(sec);
            }
        }

        public static IList<MadLibInstance> GetMadLibs(ISession session, string table, int id)
        {
            return session.CreateCriteria(typeof(MadLibInstance))
                .Add(Expression.Eq("DataObject.Id", ManagedDataObject.Find(session, table)))
                .Add(Expression.Eq("ObjectId", id))
                .List<MadLibInstance>();
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] posted a new mad lib in [{1}:{2}]",
                        mInstance.AccountId, mInstance.DataObject.Name.ToLower(), mInstance.ObjectId),
                        string.Format("{0}View.aspx?id={1}", mInstance.DataObject.Name, mInstance.ObjectId)));
                    break;
            }
            return result;
        }
    }
}
