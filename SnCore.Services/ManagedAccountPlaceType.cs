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
    public class TransitAccountPlaceType : TransitService<AccountPlaceType>
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

        public TransitAccountPlaceType(AccountPlaceType instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountPlaceType instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            CanWrite = instance.CanWrite;
            base.SetInstance(instance);
        }

        public override AccountPlaceType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPlaceType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.CanWrite = this.CanWrite;
            return instance;
        }
    }

    public class ManagedAccountPlaceType : ManagedService<AccountPlaceType, TransitAccountPlaceType>
    {
        public ManagedAccountPlaceType()
        {

        }

        public ManagedAccountPlaceType(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPlaceType(ISession session, AccountPlaceType value)
            : base(session, value)
        {

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

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
