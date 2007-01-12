using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSystemServiceTests
{
    [TestFixture]
    public class AttributeTest : WebServiceTest<WebSystemService.TransitAttribute, WebSystemServiceNoCache>
    {
        public AttributeTest()
            : base("Attribute")
        {
        }


        public override WebSystemService.TransitAttribute GetTransitInstance()
        {
            WebSystemService.TransitAttribute t_instance = new WebSystemService.TransitAttribute();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.DefaultUrl = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.DefaultValue = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
