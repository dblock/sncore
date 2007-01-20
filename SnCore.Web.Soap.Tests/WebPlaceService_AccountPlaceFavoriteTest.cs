using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class AccountPlaceFavoriteTest : WebServiceTest<WebPlaceService.TransitAccountPlaceFavorite, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
        }

        public AccountPlaceFavoriteTest()
            : base("AccountPlaceFavorite")
        {
        }

        public override WebPlaceService.TransitAccountPlaceFavorite GetTransitInstance()
        {
            WebPlaceService.TransitAccountPlaceFavorite t_instance = new WebPlaceService.TransitAccountPlaceFavorite();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.PlaceId = _place_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _place_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _place_id };
            return args;
        }

        [Test]
        public void IsAccountPlaceFavoriteTest()
        {
            WebPlaceService.TransitAccountPlaceFavorite t_instance = GetTransitInstance();
            Assert.IsFalse(EndPoint.IsAccountPlaceFavorite(GetAdminTicket(), t_instance.AccountId, _place_id));
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            Assert.IsTrue(EndPoint.IsAccountPlaceFavorite(GetAdminTicket(), t_instance.AccountId, _place_id));
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetAccountPlaceFavoritesByAccountIdTest()
        {
            WebPlaceService.TransitAccountPlaceFavorite t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            int count = EndPoint.GetAccountPlaceFavoritesCountByAccountId(GetAdminTicket(), t_instance.AccountId);
            Console.WriteLine("Count: {0}", count);
            WebPlaceService.TransitAccountPlaceFavorite[] favorites = EndPoint.GetAccountPlaceFavoritesByAccountId(GetAdminTicket(), t_instance.AccountId, null);
            Console.WriteLine("Length: {0}", favorites.Length);
            Assert.AreEqual(count, favorites.Length);
            Assert.IsTrue(new TransitServiceCollection<WebPlaceService.TransitAccountPlaceFavorite>(favorites).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetFavoritePlacesTest()
        {
            WebPlaceService.TransitAccountPlaceFavorite t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            int count = EndPoint.GetFavoritePlacesCount(GetAdminTicket());
            Console.WriteLine("Count: {0}", count);
            WebPlaceService.TransitPlace[] places = EndPoint.GetFavoritePlaces(GetAdminTicket(), null);
            Console.WriteLine("Length: {0}", places.Length);
            Assert.AreEqual(count, places.Length);
            Assert.IsTrue(new TransitServiceCollection<WebPlaceService.TransitPlace>(places).ContainsId(t_instance.PlaceId));
            Delete(GetAdminTicket(), t_instance.Id);
        }
    }
}
