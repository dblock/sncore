using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class AccountPlaceTypeTest : WebServiceTest<WebPlaceService.TransitAccountPlaceType, WebPlaceServiceNoCache>
    {
        public AccountPlaceTypeTest()
            : base("AccountPlaceType")
        {
        }

        public override WebPlaceService.TransitAccountPlaceType GetTransitInstance()
        {
            WebPlaceService.TransitAccountPlaceType t_instance = new WebPlaceService.TransitAccountPlaceType();
            t_instance.Name = GetNewString().Substring(0, 24);
            t_instance.CanWrite = true;
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}
