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
            t_instance.Message = Guid.NewGuid().ToString();
            t_instance.PlaceId = _place_id;
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            return t_instance;
        }

        [Test]
        protected void AcceptAccountPlaceRequestTest()
        {

        }

        [Test]
        protected void RejectAccountPlaceRequestTest()
        {

        }
    }
}
