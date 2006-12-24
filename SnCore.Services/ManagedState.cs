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
    public class TransitState : TransitService
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

        public TransitState(State s)
            : base(s.Id)
        {
            Name = s.Name;
            Country = s.Country.Name;
        }

        public State GetState(ISession session)
        {
            State p = (Id != 0) ? (State)session.Load(typeof(State), Id) : new State();
            p.Name = this.Name;
            p.Country = (Country)session.Load(typeof(Country), ManagedCountry.GetCountryId(session, Country));
            return p;
        }

    }

    public class ManagedState : ManagedService<State>
    {
        public class InvalidStateException : SoapException
        {
            public InvalidStateException()
                : base("Invalid state", SoapException.ClientFaultCode)
            {

            }
        }

        private State mState = null;

        public ManagedState(ISession session)
            : base(session)
        {

        }

        public ManagedState(ISession session, int id)
            : base(session)
        {
            mState = (State)session.Load(typeof(State), id);
        }

        public ManagedState(ISession session, State value)
            : base(session)
        {
            mState = value;
        }

        public int Id
        {
            get
            {
                return mState.Id;
            }
        }

        public string Name
        {
            get
            {
                return mState.Name;
            }
        }

        public TransitState TransitState
        {
            get
            {
                return new TransitState(mState);
            }
        }

        public void Create(TransitState s)
        {
            mState = s.GetState(Session);
            Session.Save(mState);
        }

        public void Delete()
        {
            Session.Delete(mState);
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

    }
}
