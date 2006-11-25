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
    public class TransitCity : TransitService
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

        private string mTag;

        public string Tag
        {
            get
            {

                return mTag;
            }
            set
            {
                mTag = value;
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

        private string mState;

        public string State
        {
            get
            {

                return mState;
            }
            set
            {
                mState = value;
            }
        }

        public TransitCity()
        {

        }

        public TransitCity(City c)
            : base(c.Id)
        {
            Name = c.Name;
            Tag = c.Tag;
            State = c.State == null ? string.Empty : c.State.Name;
            Country = c.Country.Name;
        }

        public City GetCity(ISession session)
        {
            City c = (Id != 0) ? (City)session.Load(typeof(City), Id) : new City();
            c.Name = this.Name;
            c.Tag = this.Tag;
            c.State = string.IsNullOrEmpty(State) ? null : ManagedState.Find(session, State, Country);
            c.Country = string.IsNullOrEmpty(Country) ? null : ManagedCountry.Find(session, Country);
            return c;
        }

    }

    public class ManagedCity : ManagedService
    {
        public class InvalidCityException : SoapException
        {
            public InvalidCityException()
                : base("Invalid city", SoapException.ClientFaultCode)
            {

            }
        }

        private City mCity = null;

        public ManagedCity(ISession session)
            : base(session)
        {

        }

        public ManagedCity(ISession session, int id)
            : base(session)
        {
            mCity = (City)session.Load(typeof(City), id);
        }

        public ManagedCity(ISession session, City value)
            : base(session)
        {
            mCity = value;
        }

        public int Id
        {
            get
            {
                return mCity.Id;
            }
        }

        public string Name
        {
            get
            {
                return mCity.Name;
            }
        }

        public TransitCity TransitCity
        {
            get
            {
                return new TransitCity(mCity);
            }
        }

        public void Create(TransitCity s)
        {
            mCity = s.GetCity(Session);
            Session.Save(mCity);
        }

        public void Delete()
        {
            Session.Delete(mCity);
        }

        public static City FindOrCreate(ISession session, string name, string state, string country)
        {
            try
            {
                return Find(session, name, state, country);
            }
            catch (InvalidCityException)
            {
                City city = new City();
                city.Country = ManagedCountry.Find(session, country);
                if (!string.IsNullOrEmpty(state)) city.State = ManagedState.Find(session, state, country);
                city.Name = name;
                session.Save(city);
                return city;
            }
        }

        public static City Find(ISession session, string name, string state, string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new ManagedCountry.InvalidCountryException();
            }

            ICriteria cr = session.CreateCriteria(typeof(City))
                .Add(Expression.Or(Expression.Eq("Name", name), Expression.Eq("Tag", name)));

            Country c = ManagedCountry.Find(session, country);
            cr.Add(Expression.Eq("Country.Id", c.Id));

            if (c.States != null && c.States.Count > 0 && string.IsNullOrEmpty(state))
            {
                throw new ManagedState.InvalidStateException();
            }

            if (!string.IsNullOrEmpty(state))
            {
                State s = ManagedState.Find(session, state, country);
                cr.Add(Expression.Eq("State.Id", s.Id));
            }

            City city = (City)cr.UniqueResult();

            if (city == null)
            {
                throw new InvalidCityException();
            }

            return city;
        }

        public static int GetCityId(ISession session, string name, string state, string country)
        {
            return Find(session, name, state, country).Id;
        }

        public int Merge(int id)
        {
            if (id == mCity.Id)
            {
                throw new Exception("Cannot merge city into self");
            }

            int count = 0;

            City merge = (City) Session.Load(typeof(City), id);

            // update places
            if (merge.Places != null)
            {
                count += merge.Places.Count;
                foreach (Place place in merge.Places)
                {
                    place.City = mCity;
                    Session.Save(place);
                }
            }

            // update accounts
            IList accounts = Session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("City", merge.Name))
                .List();

            count += accounts.Count;
            foreach (Account account in accounts)
            {
                account.City = mCity.Name;
                Session.Save(account);
            }

            // update account addresses
            IList accountaddresses = Session.CreateCriteria(typeof(AccountAddress))
                .Add(Expression.Eq("City", merge.Name))
                .List();

            count += accountaddresses.Count;
            foreach (AccountAddress accountaddress in accountaddresses)
            {
                accountaddress.City = mCity.Name;
                Session.Save(accountaddress);
            }

            Session.Delete(merge);
            return count;
        }
    }
}
