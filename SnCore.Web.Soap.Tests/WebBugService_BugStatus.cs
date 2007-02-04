using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugStatusTest : WebServiceTest<WebBugService.TransitBugStatus, WebBugServiceNoCache>
    {
        public BugStatusTest()
            : base("BugStatus", "BugStatuses")
        {
        }


        public override WebBugService.TransitBugStatus GetTransitInstance()
        {
            WebBugService.TransitBugStatus t_instance = new WebBugService.TransitBugStatus();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
