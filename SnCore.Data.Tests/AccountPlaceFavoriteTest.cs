using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountPlaceFavoriteTest : NHibernateTest
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

            AccountPlaceFavorite fav = new AccountPlaceFavorite();
            fav.Account = acct;
            fav.Place = place;
            fav.Created = DateTime.UtcNow;

            Session.Save(placetype);
            Session.Save(country);
            Session.Save(city);
            Session.Save(acct);
            Session.Save(place);
            Session.Save(fav);
            Session.Flush();

            Assert.IsTrue(acct.Id > 0);
            Assert.IsTrue(place.Id > 0);
            Assert.IsTrue(fav.Id > 0);

            Session.Delete(acct);
            Session.Delete(placetype);
            Session.Delete(city);
            Session.Delete(country);
            Session.Flush();
        }

        [Test]
        public void TestCrudMultipleSelect()
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

            Place place2 = new Place();
            place2.Account = acct;
            place2.Name = Guid.NewGuid().ToString();
            place2.Created = place2.Modified = DateTime.UtcNow;
            place2.City = city;
            place2.Type = placetype;
            place2.Website = string.Empty;

            AccountPlaceFavorite fav = new AccountPlaceFavorite();
            fav.Account = acct;
            fav.Place = place;
            fav.Created = DateTime.UtcNow;

            AccountPlaceFavorite fav2 = new AccountPlaceFavorite();
            fav2.Account = acct;
            fav2.Place = place2;
            fav2.Created = DateTime.UtcNow;

            Session.Save(placetype);
            Session.Save(country);
            Session.Save(city);
            Session.Save(acct);
            Session.Save(place);
            Session.Save(place2);
            Session.Save(fav);
            Session.Save(fav2);
            Session.Flush();

            Assert.IsTrue(acct.Id > 0);
            Assert.IsTrue(place.Id > 0);
            Assert.IsTrue(place2.Id > 0);
            Assert.IsTrue(fav.Id > 0);
            Assert.IsTrue(fav2.Id > 0);

            IQuery q = Session.CreateQuery("SELECT COUNT(DISTINCT apf.Place) FROM AccountPlaceFavorite apf");
            Assert.AreEqual(1, q.List().Count, "Expected an integer as a result of a COUNT(DISTINCT).");

            long result = (long) q.UniqueResult();
            Assert.IsTrue(result >= 2, "Expected at least two results.");

            Session.Delete(acct);
            Session.Delete(placetype);
            Session.Delete(city);
            Session.Delete(country);
            Session.Flush();
        }
    }
}
