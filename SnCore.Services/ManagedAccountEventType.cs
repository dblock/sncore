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
    public class TransitAccountEventType : TransitService
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

        public TransitAccountEventType()
        {

        }

        public TransitAccountEventType(AccountEventType o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public AccountEventType GetAccountEventType(ISession session)
        {
            AccountEventType p = (Id != 0) ? (AccountEventType)session.Load(typeof(AccountEventType), Id) : new AccountEventType();
            p.Name = this.Name;
            return p;
        }
    }

    public class ManagedAccountEventType : ManagedService<AccountEventType>
    {
        private AccountEventType mAccountEventType = null;

        public ManagedAccountEventType(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEventType(ISession session, int id)
            : base(session)
        {
            mAccountEventType = (AccountEventType)session.Load(typeof(AccountEventType), id);
        }

        public ManagedAccountEventType(ISession session, AccountEventType value)
            : base(session)
        {
            mAccountEventType = value;
        }

        public ManagedAccountEventType(ISession session, TransitAccountEventType value)
            : base(session)
        {
            mAccountEventType = value.GetAccountEventType(session);
        }

        public int Id
        {
            get
            {
                return mAccountEventType.Id;
            }
        }

        public TransitAccountEventType TransitAccountEventType
        {
            get
            {
                return new TransitAccountEventType(mAccountEventType);
            }
        }

        public void CreateOrUpdate(TransitAccountEventType o)
        {
            mAccountEventType = o.GetAccountEventType(Session);
            Session.Save(mAccountEventType);
        }

        public void Delete()
        {
            Session.Delete(mAccountEventType);
        }

        public static AccountEventType Find(ISession session, string name)
        {
            return (AccountEventType)session.CreateCriteria(typeof(AccountEventType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
