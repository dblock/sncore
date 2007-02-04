using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class AccountPlaceTest : WebServiceTest<WebPlaceService.TransitAccountPlace, WebPlaceServiceNoCache>
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

        public AccountPlaceTest()
            : base("AccountPlace")
        {
        }

        public override WebPlaceService.TransitAccountPlace GetTransitInstance()
        {
            WebPlaceService.TransitAccountPlace t_instance = new WebPlaceService.TransitAccountPlace();
            t_instance.Description = GetNewString();
            t_instance.PlaceId = _place_id;
            t_instance.Type = (string)_type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }
    }
}
