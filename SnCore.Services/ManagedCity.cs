using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
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

            // merge neighborhoods
            foreach (Neighborhood nh in merge.Neighborhoods)
            {
                Neighborhood t_nh = Session.CreateCriteria(typeof(Neighborhood))
                    .Add(Expression.Eq("City.Id", mInstance.Id))
                    .Add(Expression.Eq("Name", nh.Name))
                    .UniqueResult<Neighborhood>();

                if (t_nh != null)
                {
                    ManagedNeighborhood m_nh = new ManagedNeighborhood(Session, t_nh);
                    count += m_nh.Merge(sec, nh.Id);
                }
                else
                {
                    nh.City = mInstance;
                    Session.Save(nh);
                }
            }

            // merge places that don't have a neighborhood
            if (merge.Places != null)
            {
                count += merge.Places.Count;
                foreach (Place place in merge.Places)
                {
                    place.City = mInstance;
                    Session.Save(place);
                }
            }

            Session.Delete(merge);
            return count;
        }

        public int Merge(ManagedSecurityContext sec, string name, string state, string country)
        {
            if (!sec.IsAdministrator())
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            int count = 0;

            // update accounts
            ICriteria accounts_criteria = Session.CreateCriteria(typeof(Account)).Add(Expression.Eq("City", name));
            if (string.IsNullOrEmpty(state)) accounts_criteria.Add(Expression.IsNull("State")); else accounts_criteria.Add(Expression.Eq("State.Id", ManagedState.GetStateId(Session, state, country)));
            if (string.IsNullOrEmpty(country)) accounts_criteria.Add(Expression.IsNull("Country")); else accounts_criteria.Add(Expression.Eq("Country.Id", ManagedCountry.GetCountryId(Session, country)));
            IList<Account> accounts = accounts_criteria.List<Account>();
            count += accounts.Count;
            foreach (Account account in accounts)
            {
                account.City = mInstance.Name;
                account.Country = mInstance.Country;
                account.State = mInstance.State;
                Session.Save(account);
            }

            // update account addresses
            ICriteria accountaddresses_criteria = Session.CreateCriteria(typeof(AccountAddress)).Add(Expression.Eq("City", name));
            if (string.IsNullOrEmpty(state)) accountaddresses_criteria.Add(Expression.IsNull("State")); else accountaddresses_criteria.Add(Expression.Eq("State.Id", ManagedState.GetStateId(Session, state, country)));
            if (string.IsNullOrEmpty(country)) accountaddresses_criteria.Add(Expression.IsNull("Country")); else accountaddresses_criteria.Add(Expression.Eq("Country.Id", ManagedCountry.GetCountryId(Session, country)));
            IList<AccountAddress> accountaddresses = accountaddresses_criteria.List<AccountAddress>();
            count += accountaddresses.Count;
            foreach (AccountAddress accountaddress in accountaddresses)
            {
                accountaddress.City = mInstance.Name;
                accountaddress.Country = mInstance.Country;
                accountaddress.State = mInstance.State;
                Session.Save(accountaddress);
            }

            return count;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Session.Delete(string.Format("FROM Neighborhood nh WHERE nh.City.Id = {0}", Id));
            base.Delete(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            base.Save(sec);
            
            // update accounts to match spelling of this city
            IList<Account> accounts = Session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("City", mInstance.Name))
                .Add(Expression.Eq("State", mInstance.State))
                .Add(Expression.Eq("Country", mInstance.Country))
                .List<Account>();

            foreach (Account account in accounts)
            {
                account.City = mInstance.Name;
                Session.Save(account);
            }
        }
    }
}
