using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugPriorityTest : WebServiceTest<WebBugService.TransitBugPriority>
    {
        public BugPriorityTest()
            : base("BugPriority", "BugPriorities", new WebBugService.WebBugService())
        {
        }

        public override WebBugService.TransitBugPriority GetTransitInstance()
        {
            WebBugService.TransitBugPriority t_instance = new WebBugService.TransitBugPriority();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
