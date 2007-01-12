using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePropertyTest : WebServiceTest<WebPlaceService.TransitPlaceProperty, WebPlaceServiceNoCache>
    {
        public PlacePropertyGroupTest _group = new PlacePropertyGroupTest();
        public int _group_id = 0;

        [SetUp]
        public void SetUp()
        {
            _group_id = _group.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _group.Delete(GetAdminTicket(), _group_id);
        }

        public PlacePropertyTest()
            : base("PlaceProperty", "PlaceProperties")
        {
        }

        public override WebPlaceService.TransitPlaceProperty GetTransitInstance()
        {
            WebPlaceService.TransitPlaceProperty t_instance = new WebPlaceService.TransitPlaceProperty();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.PlacePropertyGroupId = _group_id;
            t_instance.DefaultValue = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.TypeName = "System.String";
            t_instance.Publish = true;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _group_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _group_id, options };
            return args;
        }
    }
}
