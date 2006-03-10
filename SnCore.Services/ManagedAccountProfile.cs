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

namespace SnCore.Services
{
    public class TransitAccountProfile : TransitService
    {
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

        private string mAboutSelf;

        public string AboutSelf
        {
            get
            {

                return mAboutSelf;
            }
            set
            {
                mAboutSelf = value;
            }
        }

        private DateTime mUpdated;

        public DateTime Updated
        {
            get
            {

                return mUpdated;
            }
            set
            {
                mUpdated = value;
            }
        }

        public TransitAccountProfile()
        {

        }

        public TransitAccountProfile(AccountProfile o)
            : base(o.Id)
        {
            AboutSelf = o.AboutSelf;
            AccountId = o.Account.Id;
            Updated = o.Updated;
        }

        public AccountProfile GetAccountProfile(ISession session)
        {
            AccountProfile p = (Id != 0) ? (AccountProfile)session.Load(typeof(AccountProfile), Id) : new AccountProfile();
            p.AboutSelf = this.AboutSelf;

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            return p;
        }
    }

    public class ManagedAccountProfile : ManagedService
    {
        private AccountProfile mAccountProfile = null;

        public ManagedAccountProfile(ISession session)
            : base(session)
        {

        }

        public ManagedAccountProfile(ISession session, int id)
            : base(session)
        {
            mAccountProfile = (AccountProfile)session.Load(typeof(AccountProfile), id);
        }

        public ManagedAccountProfile(ISession session, AccountProfile value)
            : base(session)
        {
            mAccountProfile = value;
        }

        public ManagedAccountProfile(ISession session, TransitAccountProfile value)
            : base(session)
        {
            mAccountProfile = value.GetAccountProfile(session);
        }

        public int Id
        {
            get
            {
                return mAccountProfile.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountProfile.Account.Id;
            }
        }

        public TransitAccountProfile TransitAccountProfile
        {
            get
            {
                return new TransitAccountProfile(mAccountProfile);
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountProfile);
        }
    }
}