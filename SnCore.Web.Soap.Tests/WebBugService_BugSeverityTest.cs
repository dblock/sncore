using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugSeverityTest : WebServiceTest<WebBugService.TransitBugSeverity, WebBugServiceNoCache>
    {
        public BugSeverityTest()
            : base("BugSeverity", "BugSeverities")
        {
        }

        public override WebBugService.TransitBugSeverity GetTransitInstance()
        {
            WebBugService.TransitBugSeverity t_instance = new WebBugService.TransitBugSeverity();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
