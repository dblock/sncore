using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class AttributeTest : WebServiceTest<WebObjectService.TransitAttribute, WebObjectServiceNoCache>
    {
        public AttributeTest()
            : base("Attribute")
        {
        }


        public override WebObjectService.TransitAttribute GetTransitInstance()
        {
            WebObjectService.TransitAttribute t_instance = new WebObjectService.TransitAttribute();
            t_instance.Name = GetNewString();
            t_instance.DefaultUrl = GetNewUri();
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
            return t_instance;
        }

        [Test]
        public void GetAttributeIfModifiedSinceTest()
        {
            WebObjectService.TransitAttribute t_attribute = GetTransitInstance();
            t_attribute.Id = Create(GetAdminTicket(), t_attribute);
            Assert.IsNotNull(EndPoint.GetAttributeIfModifiedSinceById(GetAdminTicket(), t_attribute.Id, DateTime.UtcNow.AddHours(-1)));
            Assert.IsNull(EndPoint.GetAttributeIfModifiedSinceById(GetAdminTicket(), t_attribute.Id, DateTime.UtcNow));
            Delete(GetAdminTicket(), t_attribute.Id);
        }
    }
}
