using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebEventServiceTests
{
    [TestFixture]
    public class AccountEventTypeTest : WebServiceTest<WebEventService.TransitAccountEventType, WebEventServiceNoCache>
    {
        public AccountEventTypeTest()
            : base("AccountEventType")
        {
        }

        public override WebEventService.TransitAccountEventType GetTransitInstance()
        {
            WebEventService.TransitAccountEventType t_instance = new WebEventService.TransitAccountEventType();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
