using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugService_BugStatusTest : WebServiceTest<WebBugService.TransitBugStatus>
    {
        public WebBugService_BugStatusTest()
            : base("BugStatus", "BugStatuses", new WebBugService.WebBugService())
        {
        }


        public override WebBugService.TransitBugStatus GetTransitInstance()
        {
            WebBugService.TransitBugStatus t_instance = new WebBugService.TransitBugStatus();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
