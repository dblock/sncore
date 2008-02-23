using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class AccountPlaceRequestTest : WebServiceTest<WebPlaceService.TransitAccountPlaceRequest, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;
        private AccountPlaceTypeTest _type = new AccountPlaceTypeTest();
        private int _type_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _type_id = _type.Create(GetAdminTicket());
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _type.Delete(GetAdminTicket(), _type_id);
            _place.TearDown();
        }

        public AccountPlaceRequestTest()
            : base("AccountPlaceRequest")
        {
        }

        public override WebPlaceService.TransitAccountPlaceRequest GetTransitInstance()
        {
            WebPlaceService.TransitAccountPlaceRequest t_instance = new WebPlaceService.TransitAccountPlaceRequest();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Message = GetNewString();
            t_instance.PlaceId = _place_id;
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            return t_instance;
        }

        [Test]
        public void AcceptAccountPlaceRequestTest()
        {
            // convince ourselves that the user cannot edit the place
            WebPlaceService.TransitPlace t_place = _place.EndPoint.GetPlaceById(GetUserTicket(), _place_id);
            Console.WriteLine("Place: {0}", t_place.Name);
            try
            {
                EndPoint.CreateOrUpdatePlace(GetUserTicket(), t_place);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
            // create a request
            WebPlaceService.TransitAccountPlaceRequest t_instance = GetTransitInstance();
            t_instance.Id = Create(GetUserTicket(), t_instance);
            // admin accepts the request
            EndPoint.AcceptAccountPlaceRequest(GetAdminTicket(), t_instance.Id, GetNewString());
            // check that requester is owner
            WebPlaceService.TransitAccountPlace[] places = EndPoint.GetAccountPlaces(GetAdminTicket(), GetUserAccount().Id, null);
            Console.WriteLine("Places: {0}", places.Length);
            Assert.IsTrue(places.Length > 0);
            bool bFound = false;
            foreach (WebPlaceService.TransitAccountPlace place in places)
            {
                if (place.PlaceId == t_instance.PlaceId)
                {
                    bFound = true;
                    break;
                }
            }
            Assert.IsTrue(bFound);
            // the requester can now edit the place
            EndPoint.CreateOrUpdatePlace(GetUserTicket(), t_place);
            // the requester can now add an alternate name
            WebPlaceService.TransitPlaceName t_name = new WebPlaceService.TransitPlaceName();
            t_name.Name = GetNewString();
            t_name.PlaceId = _place_id;
            t_name.Id = EndPoint.CreateOrUpdatePlaceName(GetUserTicket(), t_name);
            EndPoint.DeletePlaceName(GetUserTicket(), t_name.Id);
            // the requester can now delete a picture
            WebPlaceService.TransitPlacePicture t_pic = new WebPlaceService.TransitPlacePicture();
            t_pic.AccountId = GetAdminAccount().Id;
            t_pic.Bitmap = GetNewBitmap();
            t_pic.Name = GetNewString();
            t_pic.PlaceId = _place_id;
            t_pic.Id = EndPoint.CreateOrUpdatePlacePicture(GetAdminTicket(), t_pic);
            EndPoint.DeletePlacePicture(GetUserTicket(), t_pic.Id);
        }

        [Test]
        public void RejectAccountPlaceRequestTest()
        {
            // convince ourselves that the user cannot edit the place
            WebPlaceService.TransitPlace t_place = _place.EndPoint.GetPlaceById(GetUserTicket(), _place_id);
            Console.WriteLine("Place: {0}", t_place.Name);
            try
            {
                EndPoint.CreateOrUpdatePlace(GetUserTicket(), t_place);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
            // create a request
            WebPlaceService.TransitAccountPlaceRequest t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // admin accepts the request
            EndPoint.RejectAccountPlaceRequest(GetAdminTicket(), t_instance.Id, GetNewString());
            // check that requester is owner
            WebPlaceService.TransitAccountPlace[] places = EndPoint.GetAccountPlaces(GetAdminTicket(), GetUserAccount().Id, null);
            Console.WriteLine("Places: {0}", places.Length);
            bool bFound = false;
            foreach (WebPlaceService.TransitAccountPlace place in places)
            {
                if (place.PlaceId == t_instance.PlaceId)
                {
                    bFound = true;
                    break;
                }
            }
            Assert.IsFalse(bFound);
            // convince ourselves that the user still cannot edit the place
            WebPlaceService.TransitPlace t_place2 = _place.EndPoint.GetPlaceById(GetUserTicket(), _place_id);
            Console.WriteLine("Place: {0}", t_place2.Name);
            try
            {
                EndPoint.CreateOrUpdatePlace(GetUserTicket(), t_place2);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
        }

        [Test]
        public void GetAccountPlaceRequestsByAccountIdTest()
        {
            // create a brand new user
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            // create a new place that belongs to that user
            WebPlaceService.TransitPlace t_place = _place.GetTransitInstance();
            t_place.AccountId = user.id;
            t_place.Id = EndPoint.CreateOrUpdatePlace(user.ticket, t_place);
            // the new user doesn't have any requests
            WebPlaceService.TransitAccountPlaceRequest[] requests_1 = EndPoint.GetAccountPlaceRequestsByAccountId(
                user.ticket, user.id, null);
            Console.WriteLine("Requests: {0}", requests_1.Length);
            Assert.AreEqual(0, requests_1.Length);
            // create a request
            WebPlaceService.TransitAccountPlaceRequest t_instance = GetTransitInstance();
            t_instance.PlaceId = t_place.Id;
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // the new user is the owner of the place and now has a request
            WebPlaceService.TransitAccountPlaceRequest[] requests_2 = EndPoint.GetAccountPlaceRequestsByAccountId(
                user.ticket, user.id, null);
            Console.WriteLine("Requests: {0}", requests_2.Length);
            Assert.AreEqual(1, requests_2.Length);
            Assert.AreEqual(t_instance.Id, requests_2[0].Id);
            // delete place
            EndPoint.DeletePlace(user.ticket, t_place.Id);
            DeleteUser(user.id);
        }

        [Test]
        public void GetAccountPlaceRequestsByOwnerAccountIdTest()
        {
            // create a brand new user
            UserInfo user_place_owner = CreateUserWithVerifiedEmailAddress();
            Console.WriteLine("Place owner: {0}", user_place_owner.id);
            // create a new place that belongs to that user
            WebPlaceService.TransitPlace t_place = _place.GetTransitInstance();
            t_place.AccountId = user_place_owner.id;
            t_place.Id = EndPoint.CreateOrUpdatePlace(user_place_owner.ticket, t_place);
            Console.WriteLine("Place: {0}", t_place.Id);

            // third user requests ownership
            WebPlaceService.TransitAccountPlaceRequest t_request_2 = GetTransitInstance();
            t_request_2.PlaceId = t_place.Id;
            t_request_2.AccountId = GetUserAccount().Id;
            t_request_2.Id = Create(GetUserTicket(), t_request_2);
            Console.WriteLine("Request (2): {0}", t_request_2.Id);

            // the owner user has one request
            WebPlaceService.TransitAccountPlaceRequest[] requests_1 = EndPoint.GetAccountPlaceRequestsByAccountId(
                user_place_owner.ticket, user_place_owner.id, null);
            Console.WriteLine("Requests: {0}", requests_1.Length);
            Assert.AreEqual(1, requests_1.Length);

            // create a brand new user
            UserInfo user_requester = CreateUserWithVerifiedEmailAddress();
            Console.WriteLine("Requester: {0}", user_requester.id);
            
            // have user_requester request ownership of the place
            WebPlaceService.TransitAccountPlaceRequest t_request_1 = GetTransitInstance();
            t_request_1.PlaceId = t_place.Id;
            t_request_1.AccountId = user_requester.id;
            t_request_1.Id = Create(user_requester.ticket, t_request_1);
            Console.WriteLine("Request: {0}", t_request_1.Id);

            // the user has no requests
            WebPlaceService.TransitAccountPlaceRequest[] requests_2 = EndPoint.GetAccountPlaceRequestsByAccountId(
                user_requester.ticket, user_requester.id, null);
            Console.WriteLine("Requests: {0}", requests_2.Length);
            Assert.AreEqual(0, requests_2.Length);

            // approve the request
            EndPoint.AcceptAccountPlaceRequest(user_place_owner.ticket, t_request_1.Id, GetNewString());

            // the user has one request since he's also the owner of the place
            WebPlaceService.TransitAccountPlaceRequest[] requests_3 = EndPoint.GetAccountPlaceRequestsByAccountId(
                user_requester.ticket, user_requester.id, null);
            Console.WriteLine("Requests: {0}", requests_3.Length);
            Assert.AreEqual(1, requests_3.Length);

            // delete everything
            EndPoint.DeletePlace(GetAdminTicket(), t_place.Id);
            DeleteUser(user_requester.id);
            DeleteUser(user_place_owner.id);
        }

        [Test]
        public void GetAccountPlaceRequestsByPlaceIdTest()
        {
            WebPlaceService.TransitAccountPlaceRequest[] t_requests_1 = EndPoint.GetAccountPlaceRequestsByPlaceId(
                GetAdminTicket(), _place_id, null);
            Console.WriteLine("Requests: {0}", t_requests_1.Length);
            Assert.AreEqual(0, t_requests_1.Length);
            // create a request
            WebPlaceService.TransitAccountPlaceRequest t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // count
            WebPlaceService.TransitAccountPlaceRequest[] t_requests_2 = EndPoint.GetAccountPlaceRequestsByPlaceId(
                GetAdminTicket(), _place_id, null);
            Console.WriteLine("Requests: {0}", t_requests_2.Length);
            Assert.AreEqual(1, t_requests_2.Length);
            // reject
            EndPoint.RejectAccountPlaceRequest(GetAdminTicket(), t_instance.Id, GetNewString());
            // count
            WebPlaceService.TransitAccountPlaceRequest[] t_requests_3 = EndPoint.GetAccountPlaceRequestsByPlaceId(
                GetAdminTicket(), _place_id, null);
            Console.WriteLine("Requests: {0}", t_requests_3.Length);
            Assert.AreEqual(0, t_requests_3.Length);
        }
    }
}
