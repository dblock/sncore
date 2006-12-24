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
    public class TransitNeighborhood : TransitService
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

        public TransitNeighborhood(Neighborhood c)
            : base(c.Id)
        {
            Name = c.Name;
            if (c.City != null) City = c.City.Name;
            if (c.City != null && c.City.State != null) State = c.City.State.Name;
            if (c.City != null && c.City.Country != null) Country = c.City.Country.Name;
        }

        public Neighborhood GetNeighborhood(ISession session)
        {
            Neighborhood c = (Id != 0) ? (Neighborhood)session.Load(typeof(Neighborhood), Id) : new Neighborhood();
            c.Name = this.Name;
            if (! string.IsNullOrEmpty(City)) c.City = ManagedCity.Find(session, City, State, Country);
            return c;
        }

    }

    public class ManagedNeighborhood : ManagedService<Neighborhood>
    {
        public class InvalidNeighborhoodException : SoapException
        {
            public InvalidNeighborhoodException()
                : base("Invalid Neighborhood", SoapException.ClientFaultCode)
            {

            }
        }

        private Neighborhood mNeighborhood = null;

        public ManagedNeighborhood(ISession session)
            : base(session)
        {

        }

        public ManagedNeighborhood(ISession session, int id)
            : base(session)
        {
            mNeighborhood = (Neighborhood)session.Load(typeof(Neighborhood), id);
        }

        public ManagedNeighborhood(ISession session, Neighborhood value)
            : base(session)
        {
            mNeighborhood = value;
        }

        public int Id
        {
            get
            {
                return mNeighborhood.Id;
            }
        }

        public string Name
        {
            get
            {
                return mNeighborhood.Name;
            }
        }

        public TransitNeighborhood TransitNeighborhood
        {
            get
            {
                return new TransitNeighborhood(mNeighborhood);
            }
        }

        public void Create(TransitNeighborhood s)
        {
            mNeighborhood = s.GetNeighborhood(Session);
            Session.Save(mNeighborhood);
        }

        public void Delete()
        {
            Session.Delete(mNeighborhood);
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

            Neighborhood neighborhood = (Neighborhood) session.CreateCriteria(typeof(Neighborhood))
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

        public int Merge(int id)
        {
            if (id == mNeighborhood.Id)
            {
                throw new Exception("Cannot merge neighborhood into self");
            }

            int count = 0;

            Neighborhood merge = (Neighborhood)Session.Load(typeof(Neighborhood), id);

            // update places
            if (merge.Places != null)
            {
                count += merge.Places.Count;
                foreach (Place place in merge.Places)
                {
                    place.Neighborhood = mNeighborhood;
                    Session.Save(place);
                }
            }

            // update accounts - TODO when extended
            // update account addresses - TODO when extended

            Session.Delete(merge);
            return count;
        }
    }
}
