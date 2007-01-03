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
    public class TransitCountry : TransitService<Country>
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

        public TransitCountry()
        {

        }

        public TransitCountry(Country instance)
            : base(instance)
        {

        }

        public override void SetInstance(Country instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override Country GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Country instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedCountry : ManagedService<Country, TransitCountry>
    {
        public class InvalidCountryException : SoapException
        {
            public InvalidCountryException()
                : base("Invalid country", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedCountry(ISession session)
            : base(session)
        {

        }

        public ManagedCountry(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedCountry(ISession session, Country instance)
            : base(session, instance)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public static Country Find(ISession session, string name)
        {
            Country c = (Country)session.CreateCriteria(typeof(Country))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();

            if (c == null)
            {
                throw new ManagedCountry.InvalidCountryException();
            }

            return c;
        }

        public static int GetCountryId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
