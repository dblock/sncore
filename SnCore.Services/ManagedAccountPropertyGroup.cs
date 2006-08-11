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
    public class TransitAccountPropertyGroup : TransitService
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

        private string mDescription;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        public TransitAccountPropertyGroup()
        {

        }

        public TransitAccountPropertyGroup(AccountPropertyGroup o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
        }

        public AccountPropertyGroup GetAccountPropertyGroup(ISession session)
        {
            AccountPropertyGroup p = (Id != 0) ? (AccountPropertyGroup)session.Load(typeof(AccountPropertyGroup), Id) : new AccountPropertyGroup();
            p.Name = this.Name;
            p.Description = this.Description;
            return p;
        }
    }

    public class ManagedAccountPropertyGroup : ManagedService
    {
        private AccountPropertyGroup mAccountPropertyGroup = null;

        public ManagedAccountPropertyGroup(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPropertyGroup(ISession session, int id)
            : base(session)
        {
            mAccountPropertyGroup = (AccountPropertyGroup)session.Load(typeof(AccountPropertyGroup), id);
        }

        public ManagedAccountPropertyGroup(ISession session, AccountPropertyGroup value)
            : base(session)
        {
            mAccountPropertyGroup = value;
        }

        public ManagedAccountPropertyGroup(ISession session, TransitAccountPropertyGroup value)
            : base(session)
        {
            mAccountPropertyGroup = value.GetAccountPropertyGroup(session);
        }

        public int Id
        {
            get
            {
                return mAccountPropertyGroup.Id;
            }
        }

        public TransitAccountPropertyGroup TransitAccountPropertyGroup
        {
            get
            {
                return new TransitAccountPropertyGroup(mAccountPropertyGroup);
            }
        }

        public void CreateOrUpdate(TransitAccountPropertyGroup o)
        {
            mAccountPropertyGroup = o.GetAccountPropertyGroup(Session);
            Session.Save(mAccountPropertyGroup);
        }

        public void Delete()
        {
            Session.Delete(mAccountPropertyGroup);
        }

        public static AccountPropertyGroup Find(ISession session, string name)
        {
            return (AccountPropertyGroup)session.CreateCriteria(typeof(AccountPropertyGroup))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
