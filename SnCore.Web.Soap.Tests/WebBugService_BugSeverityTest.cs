using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugSeverityTest : WebServiceTest<WebBugService.TransitBugSeverity>
    {
        public BugSeverityTest()
            : base("BugSeverity", "BugSeverities", new WebBugService.WebBugService())
        {
        }

        public override WebBugService.TransitBugSeverity GetTransitInstance()
        {
            WebBugService.TransitBugSeverity t_instance = new WebBugService.TransitBugSeverity();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
