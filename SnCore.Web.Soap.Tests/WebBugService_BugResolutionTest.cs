using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugResolutionTest : WebServiceTest<WebBugService.TransitBugResolution>
    {
        public BugResolutionTest()
            : base("BugResolution", new WebBugService.WebBugService())
        {
        }


        public override WebBugService.TransitBugResolution GetTransitInstance()
        {
            WebBugService.TransitBugResolution t_instance = new WebBugService.TransitBugResolution();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
