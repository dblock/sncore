using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebLocationServiceTests;
using System.Threading;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceTest : WebServiceTest<WebPlaceService.TransitPlace, WebPlaceServiceNoCache>
    {
        public NeighborhoodTest _neighborhood = new NeighborhoodTest();
        public int _neighborhood_id = 0;
        public PlaceTypeTest _type = new PlaceTypeTest();
        public int _type_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _neighborhood.SetUp();
            _neighborhood_id = _neighborhood.Create(GetAdminTicket());
            _type.SetUp();
            _type_id = _type.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            _neighborhood.Delete(GetAdminTicket(), _neighborhood_id);
            _neighborhood.TearDown();
            _type.Delete(GetAdminTicket(), _type_id);
            _type.TearDown();
            DeleteUser(_user.id);
        }

        public PlaceTest()
            : base("Place")
        {
        }

        public override WebPlaceService.TransitPlace GetTransitInstance()
        {
            WebPlaceService.TransitPlace t_instance = new WebPlaceService.TransitPlace();
            t_instance.Name = GetNewString();
            t_instance.CrossStreet = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Email = GetNewEmailAddress();
            t_instance.Fax = "(123) 123-4567";
            t_instance.Phone = "(123) 123-4567";
            t_instance.Street = GetNewString();
            t_instance.Zip = "12345";
            t_instance.Neighborhood = (string) _neighborhood.GetInstancePropertyById(GetAdminTicket(), _neighborhood_id, "Name");
            t_instance.City = (string) _neighborhood._city.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city_id, "Name");
            t_instance.State = (string)_neighborhood._city._state.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city._state_id, "Name");
            t_instance.Country = (string)_neighborhood._city._state._country.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city._state._country_id, "Name");
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebPlaceService.TransitPlaceQueryOptions qopt = new WebPlaceService.TransitPlaceQueryOptions();
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebPlaceService.TransitPlaceQueryOptions qopt = new WebPlaceService.TransitPlaceQueryOptions();
            object[] args = { ticket, qopt, options };
            return args;
        }

        [Test]
        public void GetNewPlacesTest()
        {
            WebPlaceService.TransitPlace t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebPlaceService.TransitPlacePicture t_picture = new WebPlaceService.TransitPlacePicture();
            t_picture.AccountId = _user.id;
            t_picture.Bitmap = GetNewBitmap();
            t_picture.Description = GetNewString();
            t_picture.Name = GetNewString();
            t_picture.PlaceId = t_instance.Id;
            t_picture.Id = EndPoint.CreateOrUpdatePlacePicture(_user.ticket, t_picture);
            Assert.IsTrue(t_picture.Id > 0);
            WebPlaceService.TransitPlace[] places = EndPoint.GetNewPlaces(GetAdminTicket(), null);
            Assert.IsNotNull(places);
            Console.WriteLine("Places: {0}", places.Length);
            Assert.IsTrue(places.Length > 0);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void FindPlaceTest()
        {
            WebPlaceService.TransitPlace t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            string tag = (string) _neighborhood._city.GetInstancePropertyById(_user.ticket, _neighborhood._city_id, "Tag");
            Console.WriteLine("City tag: {0}", tag);
            WebPlaceService.TransitPlace t_found = EndPoint.FindPlace(_user.ticket, tag, t_instance.Name);
            Assert.IsNotNull(t_found);
            Assert.AreEqual(t_found.Id, t_instance.Id);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void SearchPlacesTest()
        {
            WebPlaceService.TransitPlace t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // give time to reindex
            Thread.Sleep(2000);
            WebPlaceService.TransitPlace[] t_found = EndPoint.SearchPlaces(_user.ticket, t_instance.Name, null);
            Assert.IsNotNull(t_found);
            Console.WriteLine("Found: {0}", t_found.Length);
            Assert.IsTrue(t_found.Length > 0);
            Assert.IsTrue(new TransitServiceCollection<WebPlaceService.TransitPlace>(t_found).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void SearchPlacesEmptyTest()
        {
            WebPlaceService.TransitPlace[] t_found = EndPoint.SearchPlaces(
                _user.ticket, string.Empty, null);
            Assert.AreEqual(0, t_found.Length);
        }

        [Test]
        protected void CreateOrUpdatePlaceWithPropertyValuesTest()
        {
            // create a place that has property values (test permissions to create property values)
        }

        [Test]
        public void ClearPlaceNeighborhoodTest()
        {
            WebPlaceService.TransitPlace t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            try
            {
                WebPlaceService.TransitPlace t_retreived = EndPoint.GetPlaceById(GetAdminTicket(), t_instance.Id);
                Assert.IsNotNull(t_retreived);
                Assert.AreEqual(t_retreived.Id, t_instance.Id);
                // update a neighborhood to blank, make sure it's saved properly
                t_retreived.Neighborhood = string.Empty;
                EndPoint.CreateOrUpdatePlace(GetAdminTicket(), t_retreived);
                WebPlaceService.TransitPlace t_changed = EndPoint.GetPlaceById(GetAdminTicket(), t_instance.Id);
                Assert.IsTrue(string.IsNullOrEmpty(t_changed.Neighborhood), string.Format("Neighborhood hasn't been cleared on save, is {0}.", t_changed.Neighborhood));
            }
            finally
            {
                Delete(GetAdminTicket(), t_instance.Id);
            }
        }

        [Test]
        public void MergePlacesTest()
        {
            WebPlaceService.TransitPlace t_instance1 = GetTransitInstance();
            t_instance1.Id = Create(GetAdminTicket(), t_instance1);
            WebPlaceService.TransitPlace t_instance2 = GetTransitInstance();
            t_instance2.Id = Create(GetAdminTicket(), t_instance2);
            // merge
            string oldname = t_instance1.Name;
            t_instance1.Name = GetNewString();
            EndPoint.MergePlaces(GetAdminTicket(), t_instance2.Id, t_instance1);
            WebPlaceService.TransitPlace t_instance_new = EndPoint.GetPlaceById(GetAdminTicket(), t_instance1.Id);
            Console.WriteLine("Renamed {0} to {1}", oldname, t_instance_new.Name);
            Assert.AreEqual(t_instance1.Name, t_instance_new.Name);
            Assert.AreNotEqual(t_instance_new.Name, oldname);

            try
            {
                WebPlaceService.TransitPlace t_instance_deleted = EndPoint.GetPlaceById(GetAdminTicket(), t_instance2.Id);
                Assert.IsFalse(true, "Expected merged place to be deleted.");
            }
            catch
            {

            }

            Delete(GetAdminTicket(), t_instance1.Id);
        }

    }
}
