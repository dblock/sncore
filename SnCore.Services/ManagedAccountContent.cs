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
using System.Collections.Generic;
using System.Web;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using Rss;
using Atom.Core;

namespace SnCore.Services
{
    public class TransitAccountContent : TransitService<AccountContent>
    {
        private string mTag;

        public string Tag
        {
            get
            {

                return mTag;
            }
            set
            {
                mTag = value;
            }
        }

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

        private DateTime mTimestamp;

        public DateTime Timestamp
        {
            get
            {
                return mTimestamp;
            }
            set
            {
                mTimestamp = value;
            }
        }

        private int mInstanceGroupId;

        public int AccountContentGroupId
        {
            get
            {
                return mInstanceGroupId;
            }
            set
            {
                mInstanceGroupId = value;
            }
        }

        private bool mInstanceGroupTrusted;

        public bool AccountContentGroupTrusted
        {
            get
            {
                return mInstanceGroupTrusted;
            }
            set
            {
                mInstanceGroupTrusted = value;
            }
        }

        public TransitAccountContent()
        {

        }

        public TransitAccountContent(AccountContent instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountContent instance)
        {
            Tag = instance.Tag;
            Text = instance.Text;
            Timestamp = instance.Timestamp;
            AccountContentGroupId = instance.AccountContentGroup.Id;
            AccountContentGroupTrusted = instance.AccountContentGroup.Trusted;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override AccountContent GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountContent instance = base.GetInstance(session, sec);
            instance.AccountContentGroup = (AccountContentGroup)session.Load(typeof(AccountContentGroup), AccountContentGroupId);
            instance.Tag = this.Tag;
            instance.Text = this.Text;
            instance.Timestamp = this.Timestamp;
            return instance;
        }
    }

    public class ManagedAccountContent : ManagedService<AccountContent, TransitAccountContent>
    {
        public ManagedAccountContent(ISession session)
            : base(session)
        {

        }

        public ManagedAccountContent(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountContent(ISession session, AccountContent value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.AccountContentGroup.Account.Id;
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