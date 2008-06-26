using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceWebsiteTest : WebServiceTest<WebPlaceService.TransitPlaceWebsite, WebPlaceServiceNoCache>
    {
        private UserInfo _user = null;
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
            DeleteUser(_user.id);
        }

        public PlaceWebsiteTest()
            : base("PlaceWebsite")
        {

        }

        public override WebPlaceService.TransitPlaceWebsite GetTransitInstance()
        {
            WebPlaceService.TransitPlaceWebsite t_instance = new WebPlaceService.TransitPlaceWebsite();
            t_instance.AccountId = _user.id;
            t_instance.PlaceId = _place_id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Url = GetNewUri();
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
    }
}
