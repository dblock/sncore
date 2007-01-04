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
    public class TransitState : TransitService<State>
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

        private string mCountry;

        public string Country
        {
            get
            {

                return mCountry;
            }
            set
            {
                mCountry = value;
            }
        }

        public TransitState()
        {

        }

        public TransitState(State instance)
            : base(instance)
        {

        }

        public override void SetInstance(State instance)
        {
            Name = instance.Name;
            Country = instance.Country.Name;
            base.SetInstance(instance);
        }

        public override State GetInstance(ISession session, ManagedSecurityContext sec)
        {
            State instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Country = (Country)session.Load(typeof(Country), ManagedCountry.GetCountryId(session, Country));
            return instance;
        }
    }

    public class ManagedState : ManagedService<State, TransitState>
    {
        public class InvalidStateException : SoapException
        {
            public InvalidStateException()
                : base("Invalid state", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedState()
        {

        }

        public ManagedState(ISession session)
            : base(session)
        {

        }

        public ManagedState(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedState(ISession session, State value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public static State Find(ISession session, string name, string country)
        {
            State s = (State)session.CreateCriteria(typeof(State))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Country.Id", string.IsNullOrEmpty(country) ? 0 : ManagedCountry.GetCountryId(session, country)))
                .UniqueResult();

            if (s == null)
            {
                throw new InvalidStateException();
            }

            return s;
        }

        public static int GetStateId(ISession session, string name, string country)
        {
            return Find(session, name, country).Id;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
