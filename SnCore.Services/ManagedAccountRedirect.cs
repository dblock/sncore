using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.IO;
using System.Text;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountRedirect : TransitService<AccountRedirect>
    {
        private string mSourceUri;

        public string SourceUri
        {
            get
            {

                return mSourceUri;
            }
            set
            {
                mSourceUri = value;
            }
        }

        private string mTargetUri;

        public string TargetUri
        {
            get
            {

                return mTargetUri;
            }
            set
            {
                mTargetUri = value;
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

        public TransitAccountRedirect()
        {

        }

        public TransitAccountRedirect(AccountRedirect instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountRedirect instance)
        {
            SourceUri = instance.SourceUri;
            TargetUri = instance.TargetUri;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override AccountRedirect GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountRedirect instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.TargetUri = this.TargetUri.Trim().Trim("/".ToCharArray());
            instance.SourceUri = this.SourceUri.Trim().Trim("/".ToCharArray());
            return instance;
        }
    }

    public class ManagedAccountRedirect : ManagedService<AccountRedirect, TransitAccountRedirect>
    {
        private static readonly object s_lock = new object();

        public class InvalidUriException : SoapException
        {
            public InvalidUriException(string uri)
                : base(string.Format("Invalid url: {0}. Make sure it is only composed of letters or digits.", uri), SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedAccountRedirect()
        {

        }

        public ManagedAccountRedirect(ISession session)
            : base(session)
        {

        }

        public ManagedAccountRedirect(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountRedirect(ISession session, AccountRedirect value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountRedirect>.GetSafeCollection(mInstance.Account.AccountRedirects).Remove(mInstance);
            base.Delete(sec);
        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        public static void CheckTargetUri(string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                throw new InvalidUriException(uri);
            }
        }

        public static void CheckSourceUri(string uri)
        {
            CheckTargetUri(uri);

            CharEnumerator e = uri.GetEnumerator();
            while (e.MoveNext())
            {
                if (!Char.IsLetterOrDigit(e.Current))
                {
                    throw new InvalidUriException(uri);
                }
            }
        }

        public static void UpdateMap(ISession session)
        {
            lock (s_lock)
            {
                StringBuilder sb = new StringBuilder();

                IList<AccountRedirect> redirects = session.CreateCriteria(typeof(AccountRedirect))
                    .List<AccountRedirect>();

                //sb.AppendLine("RewriteLog  c:\\temp\\iirfLog.out");
                //sb.AppendLine("RewriteLogLevel 3");

                foreach (AccountRedirect redirect in redirects)
                {
                    if (redirect.SourceUri == null || string.IsNullOrEmpty(redirect.SourceUri.Trim()))
                        continue;

                    if (redirect.TargetUri == null || string.IsNullOrEmpty(redirect.TargetUri.Trim()))
                        continue;

                    sb.AppendFormat("RewriteRule    ^/{0}([\\/]*)$    /{1}",
                        redirect.SourceUri, redirect.TargetUri);
                    sb.AppendLine();
                }

                string inipath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IsapiRewrite\\IsapiRewrite4.ini");
                Console.WriteLine(inipath);

                FileStream f = null;
                
                if (File.Exists(inipath))
                {
                    f = new FileStream(inipath, FileMode.Truncate, FileAccess.Write);
                }
                else
                {
                    f = new FileStream(inipath, FileMode.Create, FileAccess.Write);
                }

                StreamWriter sw = new StreamWriter(f);
                sw.Write(sb);
                sw.Close();
                f.Close();
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            CheckSourceUri(mInstance.SourceUri);
            CheckTargetUri(mInstance.TargetUri);
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountRedirect t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.Account.AccountRedirects);
            }
        }
    }
}
