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
    public class TransitAccountPlaceType : TransitService
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

        private bool mCanWrite;

        public bool CanWrite
        {
            get
            {

                return mCanWrite;
            }
            set
            {
                mCanWrite = value;
            }
        }

        public TransitAccountPlaceType()
        {

        }

        public TransitAccountPlaceType(AccountPlaceType o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            CanWrite = o.CanWrite;
        }

        public AccountPlaceType GetAccountPlaceType(ISession session)
        {
            AccountPlaceType p = (Id != 0) ? (AccountPlaceType)session.Load(typeof(AccountPlaceType), Id) : new AccountPlaceType();
            p.Name = this.Name;
            p.Description = this.Description;
            p.CanWrite = this.CanWrite;
            return p;
        }
    }

    public class ManagedAccountPlaceType : ManagedService
    {
        private AccountPlaceType mAccountPlaceType = null;

        public ManagedAccountPlaceType(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceType(ISession session, int id)
            : base(session)
        {
            mAccountPlaceType = (AccountPlaceType)session.Load(typeof(AccountPlaceType), id);
        }

        public ManagedAccountPlaceType(ISession session, AccountPlaceType value)
            : base(session)
        {
            mAccountPlaceType = value;
        }

        public ManagedAccountPlaceType(ISession session, TransitAccountPlaceType value)
            : base(session)
        {
            mAccountPlaceType = value.GetAccountPlaceType(session);
        }

        public int Id
        {
            get
            {
                return mAccountPlaceType.Id;
            }
        }

        public TransitAccountPlaceType TransitAccountPlaceType
        {
            get
            {
                return new TransitAccountPlaceType(mAccountPlaceType);
            }
        }

        public void CreateOrUpdate(TransitAccountPlaceType o)
        {
            mAccountPlaceType = o.GetAccountPlaceType(Session);
            Session.Save(mAccountPlaceType);
        }

        public void Delete()
        {
            Session.Delete(mAccountPlaceType);
        }

        public static AccountPlaceType Find(ISession session, string name)
        {
            return (AccountPlaceType)session.CreateCriteria(typeof(AccountPlaceType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
