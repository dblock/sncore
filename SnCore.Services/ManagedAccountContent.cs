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
    public class TransitAccountContent : TransitService
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

        private int mAccountContentGroupId;

        public int AccountContentGroupId
        {
            get
            {
                return mAccountContentGroupId;
            }
            set
            {
                mAccountContentGroupId = value;
            }
        }

        private bool mAccountContentGroupTrusted;

        public bool AccountContentGroupTrusted
        {
            get
            {
                return mAccountContentGroupTrusted;
            }
            set
            {
                mAccountContentGroupTrusted = value;
            }
        }

        public TransitAccountContent()
        {

        }

        public TransitAccountContent(AccountContent o)
            : base(o.Id)
        {
            Tag = o.Tag;
            Text = o.Text;
            Timestamp = o.Timestamp;
            AccountContentGroupId = o.AccountContentGroup.Id;
            AccountContentGroupTrusted = o.AccountContentGroup.Trusted;
            Created = o.Created;
            Modified = o.Modified;
        }

        public AccountContent GetAccountContent(ISession session)
        {
            AccountContent p = (Id != 0) ? (AccountContent)session.Load(typeof(AccountContent), Id) : new AccountContent();
            p.AccountContentGroup = (AccountContentGroup)session.Load(typeof(AccountContentGroup), AccountContentGroupId);
            p.Tag = this.Tag;
            p.Text = this.Text;
            p.Timestamp = this.Timestamp;
            return p;
        }
    }

    public class ManagedAccountContent : ManagedService<AccountContent>
    {
        private AccountContent mAccountContent = null;

        public ManagedAccountContent(ISession session)
            : base(session)
        {

        }

        public ManagedAccountContent(ISession session, int id)
            : base(session)
        {
            mAccountContent = (AccountContent)session.Load(typeof(AccountContent), id);
        }

        public ManagedAccountContent(ISession session, AccountContent value)
            : base(session)
        {
            mAccountContent = value;
        }

        public ManagedAccountContent(ISession session, TransitAccountContent value)
            : base(session)
        {
            mAccountContent = value.GetAccountContent(session);
        }

        public int Id
        {
            get
            {
                return mAccountContent.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountContent.AccountContentGroup.Account.Id;
            }
        }

        public TransitAccountContent TransitAccountContent
        {
            get
            {
                return new TransitAccountContent(mAccountContent);
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountContent);
        }
    }
}