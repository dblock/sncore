using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.IO;
using System.Text;

namespace SnCore.Services
{
    public class TransitAccountRedirect : TransitService
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

        public TransitAccountRedirect(AccountRedirect r)
            : base(r.Id)
        {
            SourceUri = r.SourceUri;
            TargetUri = r.TargetUri;
            AccountId = r.Account.Id;
            AccountName = r.Account.Name;
            Created = r.Created;
            Modified = r.Modified;
        }

        public AccountRedirect GetAccountRedirect(ISession session)
        {
            AccountRedirect r = (Id != 0) ? (AccountRedirect)session.Load(typeof(AccountRedirect), Id) : new AccountRedirect();

            if (Id == 0)
            {
                if (AccountId > 0) r.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            r.TargetUri = this.TargetUri.Trim().Trim("/".ToCharArray());
            r.SourceUri = this.SourceUri.Trim().Trim("/".ToCharArray());
            return r;
        }

    }

    public class ManagedAccountRedirect : ManagedService<AccountRedirect>
    {
        private static readonly object s_lock = new object();

        public class InvalidUriException : SoapException
        {
            public InvalidUriException(string uri)
                : base(string.Format("Invalid url: {0}. Make sure it is only composed of letters or digits.", uri), SoapException.ClientFaultCode)
            {

            }
        }

        private AccountRedirect mAccountRedirect = null;

        public ManagedAccountRedirect(ISession session)
            : base(session)
        {

        }

        public ManagedAccountRedirect(ISession session, int id)
            : base(session)
        {
            mAccountRedirect = (AccountRedirect)session.Load(typeof(AccountRedirect), id);
        }

        public ManagedAccountRedirect(ISession session, AccountRedirect value)
            : base(session)
        {
            mAccountRedirect = value;
        }

        public int Id
        {
            get
            {
                return mAccountRedirect.Id;
            }
        }

        public TransitAccountRedirect TransitAccountRedirect
        {
            get
            {
                return new TransitAccountRedirect(mAccountRedirect);
            }
        }

        public void Delete()
        {
            mAccountRedirect.Account.AccountRedirects.Remove(mAccountRedirect);
            Session.Delete(mAccountRedirect);
        }

        public Account Account
        {
            get
            {
                return mAccountRedirect.Account;
            }
        }

        public static void CheckTargetUri(string uri)
        {
            if (! Uri.IsWellFormedUriString(uri, UriKind.Relative))
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

                IList redirects = session.CreateCriteria(typeof(AccountRedirect))
                    .List();

                //sb.AppendLine("RewriteLog  c:\\temp\\iirfLog.out");
                //sb.AppendLine("RewriteLogLevel 3");

                foreach (AccountRedirect redirect in redirects)
                {
                    sb.AppendFormat("RewriteRule    ^/{0}([\\/]*)$    /{1}",
                        redirect.SourceUri, redirect.TargetUri);
                    sb.AppendLine();
                }

                string inipath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IsapiRewrite\\IsapiRewrite4.ini");
                Console.WriteLine(inipath);

                FileStream f = new FileStream(inipath, FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(f);
                sw.Write(sb);
                sw.Close();
                f.Close();
            }
        }
    }
}
