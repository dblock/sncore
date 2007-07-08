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
    public class TransitNeighborhood : TransitService<Neighborhood>
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

        private string mCity;

        public string City
        {
            get
            {

                return mCity;
            }
            set
            {
                mCity = value;
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

        public TransitNeighborhood()
        {

        }

        public TransitNeighborhood(Neighborhood value)
            : base(value)
        {

        }

        public override void SetInstance(Neighborhood value)
        {
            Name = value.Name;
            if (value.City != null) City = value.City.Name;
            if (value.City != null && value.City.State != null) State = value.City.State.Name;
            if (value.City != null && value.City.Country != null) Country = value.City.Country.Name;
            base.SetInstance(value);
        }

        public override Neighborhood GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Neighborhood instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            if (!string.IsNullOrEmpty(City)) instance.City = ManagedCity.Find(session, City, State, Country);
            return instance;
        }
    }

    public class ManagedNeighborhood : ManagedService<Neighborhood, TransitNeighborhood>
    {
        public class InvalidNeighborhoodException : Exception
        {
            public InvalidNeighborhoodException()
                : base("Invalid Neighborhood")
            {

            }
        }

        public ManagedNeighborhood()
        {

        }

        public ManagedNeighborhood(ISession session)
            : base(session)
        {

        }

        public ManagedNeighborhood(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedNeighborhood(ISession session, Neighborhood value)
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

        public static Neighborhood FindOrCreate(ISession session, string name, string city, string state, string country)
        {
            try
            {
                return Find(session, name, city, state, country);
            }
            catch (InvalidNeighborhoodException)
            {
                Neighborhood neighborhood = new Neighborhood();
                neighborhood.City = ManagedCity.Find(session, city, state, country);
                neighborhood.Name = name;
                session.Save(neighborhood);
                return neighborhood;
            }
        }

        public static Neighborhood Find(ISession session, string name, string city, string state, string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                throw new ManagedCountry.InvalidCountryException();
            }

            if (string.IsNullOrEmpty(city))
            {
                throw new ManagedCity.InvalidCityException();
            }

            City c = ManagedCity.Find(session, city, state, country);

            Neighborhood neighborhood = (Neighborhood)session.CreateCriteria(typeof(Neighborhood))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("City.Id", c.Id))
                .UniqueResult();

            if (neighborhood == null)
            {
                throw new InvalidNeighborhoodException();
            }

            return neighborhood;
        }

        public static int GetNeighborhoodId(ISession session, string name, string city, string state, string country)
        {
            return Find(session, name, city, state, country).Id;
        }

        public int Merge(ManagedSecurityContext sec, int id)
        {
            if (! sec.IsAdministrator())
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            if (id == mInstance.Id)
            {
                throw new Exception("Cannot merge neighborhood into self");
            }

            int count = 0;

            Neighborhood merge = Session.Load<Neighborhood>(id);

            // update places
            if (merge.Places != null)
            {
                count += merge.Places.Count;
                foreach (Place place in merge.Places)
                {
                    place.Neighborhood = mInstance;
                    place.City = mInstance.City;
                    Session.Save(place);
                }
            }

            // update accounts - TODO when extended
            // update account addresses - TODO when extended

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
