using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using SnCore.Services;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;

namespace SnCore.Stress.Tests
{
    [TestFixture]
    public class CreatePlacesStressTest : NHibernateTest
    {
        public CreatePlacesStressTest()
        {

        }

        [Test]
        public void Create100Places()
        {
            CreateManyPlaces(100);
        }

        [Test]
        public void Create1000Places()
        {
            CreateManyPlaces(1000);
        }

        [Test]
        public void Create10000Places()
        {
            CreateManyPlaces(10000);
        }

        public void CreateManyPlaces(int count)
        {
            Random r = new Random();
            ManagedSecurityContext sec = ManagedAccount.GetAdminSecurityContext(Session);

            // country
            TransitCountry t_country = new TransitCountry();
            t_country.Name = Guid.NewGuid().ToString();
            ManagedCountry country = new ManagedCountry(Session);
            country.CreateOrUpdate(t_country, sec);
            // state
            TransitState t_state = new TransitState();
            t_state.Name = Guid.NewGuid().ToString();
            t_state.Country = t_country.Name;
            ManagedState state = new ManagedState(Session);
            state.CreateOrUpdate(t_state, sec);
            // city
            TransitCity t_city = new TransitCity();
            t_city.Name = Guid.NewGuid().ToString();
            t_city.State = t_state.Name;
            t_city.Country = t_country.Name;
            ManagedCity city = new ManagedCity(Session);
            city.CreateOrUpdate(t_city, sec);
            // place type
            TransitPlaceType t_placetype = new TransitPlaceType();
            t_placetype.Name = Guid.NewGuid().ToString();
            ManagedPlaceType placetype = new ManagedPlaceType(Session);
            placetype.CreateOrUpdate(t_placetype, sec);

            for (int i = 0; i < count; i++)
            {
                TransitPlace t_place = new TransitPlace();
                t_place.Name = Guid.NewGuid().ToString();
                t_place.AccountId = sec.Account.Id;
                t_place.City = t_city.Name;
                t_place.Country = t_country.Name;
                t_place.State = t_state.Name;
                t_place.Street = string.Format("{0} {1} St.", r.Next(), Guid.NewGuid().ToString());
                t_place.Zip = r.Next().ToString();
                t_place.Type = t_placetype.Name;

                ManagedPlace place = new ManagedPlace(Session);
                place.CreateOrUpdate(t_place, sec);
            }
        }
    }
}
