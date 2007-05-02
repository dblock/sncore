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
    public class TransitCity : TransitService<City>
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

        public TransitCity(City value)
            : base(value)
        {
        }

        public override void SetInstance(City value)
        {
            Name = value.Name;
            Tag = value.Tag;
            State = value.State == null ? string.Empty : value.State.Name;
            Country = value.Country.Name;
            base.SetInstance(value);
        }

        public override City GetInstance(ISession session, ManagedSecurityContext sec)
        {
            City instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Tag = this.Tag;
            instance.State = string.IsNullOrEmpty(State) ? null : ManagedState.Find(session, State, Country);
            instance.Country = string.IsNullOrEmpty(Country) ? null : ManagedCountry.Find(session, Country);
            return instance;
        }
    }

    public class ManagedCity : ManagedService<City, TransitCity>
    {
        public class InvalidCityException : Exception
        {
            public InvalidCityException()
                : base("Invalid city")
            {

            }
        }

        public ManagedCity()
        {

        }

        public ManagedCity(ISession session)
            : base(session)
        {

        }

        public ManagedCity(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedCity(ISession session, City value)
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

        public static bool TryGetCityId(ISession session, string name, string state, string country, out int id)
        {
            id = 0;
            try
            {
                id = Find(session, name, state, country).Id;
                return true;
            }
            catch (InvalidCityException)
            {
                return false;
            }
            catch (ManagedCountry.InvalidCountryException)
            {
                return false;
            }
            catch (ManagedState.InvalidStateException)
            {
                return false;
            }
        }

        public static int GetCityId(ISession session, string name, string state, string country)
        {
            return Find(session, name, state, country).Id;
        }

        public int Merge(ManagedSecurityContext sec, int id)
        {
            if (!sec.IsAdministrator())
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            if (id == mInstance.Id)
            {
                throw new Exception("Cannot merge city into self");
            }

            int count = 0;

            City merge = Session.Load<City>(id);

            // update places
            if (merge.Places != null)
            {
                count += merge.Places.Count;
                foreach (Place place in merge.Places)
                {
                    place.City = mInstance;
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
                account.City = mInstance.Name;
                Session.Save(account);
            }

            // update account addresses
            IList accountaddresses = Session.CreateCriteria(typeof(AccountAddress))
                .Add(Expression.Eq("City", merge.Name))
                .List();

            count += accountaddresses.Count;
            foreach (AccountAddress accountaddress in accountaddresses)
            {
                accountaddress.City = mInstance.Name;
                Session.Save(accountaddress);
            }

            Session.Delete(merge);
            return count;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
