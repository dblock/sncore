using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePropertyTest : WebServiceTest<WebPlaceService.TransitPlaceProperty, WebPlaceServiceNoCache>
    {
        public PlacePropertyGroupTest _group = new PlacePropertyGroupTest();
        public int _group_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group_id = _group.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
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
            t_instance.Name = GetNewString();
            t_instance.PlacePropertyGroupId = _group_id;
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
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
