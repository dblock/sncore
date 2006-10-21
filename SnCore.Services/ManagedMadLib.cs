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
    public class TransitMadLib : TransitService
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

        private string mTemplate;

        public string Template
        {
            get
            {
                return mTemplate;
            }
            set
            {
                mTemplate = value;
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

        public TransitMadLib()
        {

        }

        public TransitMadLib(MadLib c)
            : base(c.Id)
        {
            Name = c.Name;
            Template = c.Template;
            Created = c.Created;
            Modified = c.Modified;
            AccountId = c.Account.Id;
            AccountName = c.Account.Name;
        }

        public MadLib GetMadLib(ISession session)
        {
            MadLib p = (Id != 0) ? (MadLib)session.Load(typeof(MadLib), Id) : new MadLib();
            p.Name = this.Name;
            p.Template = this.Template;
            if (Id == 0 && AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            return p;
        }
    }

    /// <summary>
    /// Managed MadLib.
    /// </summary>
    public class ManagedMadLib : ManagedService
    {
        private MadLib mMadLib = null;

        public ManagedMadLib(ISession session)
            : base(session)
        {

        }

        public ManagedMadLib(ISession session, int id)
            : base(session)
        {
            mMadLib = (MadLib)session.Load(typeof(MadLib), id);
        }

        public ManagedMadLib(ISession session, MadLib value)
            : base(session)
        {
            mMadLib = value;
        }

        public ManagedMadLib(ISession session, TransitMadLib value)
            : base(session)
        {
            mMadLib = value.GetMadLib(session);
        }

        public int Id
        {
            get
            {
                return mMadLib.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mMadLib.Account.Id;
            }
        }

        public TransitMadLib TransitMadLib
        {
            get
            {
                return new TransitMadLib(mMadLib);
            }
        }

        public void CreateOrUpdate(TransitMadLib c)
        {
            mMadLib = c.GetMadLib(Session);
            mMadLib.Modified = DateTime.UtcNow;
            if (mMadLib.Id == 0) mMadLib.Created = mMadLib.Modified;
            Session.Save(mMadLib);
        }

        public void Delete()
        {
            Session.Delete(mMadLib);
        }
    }
}
