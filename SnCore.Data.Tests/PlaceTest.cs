using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class PlaceTest : NHibernateTest
    {
        [Test]
        public void TestCrud()
        {
            Country country = new Country();
            country.Name = Guid.NewGuid().ToString();

            City city = new City();
            city.Name = Guid.NewGuid().ToString();
            city.Country = country;

            Account acct = new Account();
            acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
            acct.Name = "Test User";
            acct.Password = "password";
            acct.Birthday = new DateTime(1976, 9, 7);

            PlaceType placetype = new PlaceType();
            placetype.Name = Guid.NewGuid().ToString();

            Place place = new Place();
            place.Account = acct;
            place.Name = Guid.NewGuid().ToString();
            place.Created = place.Modified = DateTime.UtcNow;
            place.City = city;
            place.Type = placetype;
            place.Website = string.Empty;

            Session.Save(placetype);
            Session.Save(country);
            Session.Save(city);
            Session.Save(acct);
            Session.Save(place);
            Session.Flush();

            Assert.IsTrue(acct.Id > 0);
            Assert.IsTrue(place.Id > 0);

            Session.Delete(acct);
            Session.Delete(placetype);
            Session.Delete(city);
            Session.Delete(country);
            Session.Flush();
        }
    }
}
