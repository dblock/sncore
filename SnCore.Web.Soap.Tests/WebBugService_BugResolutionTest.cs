using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugResolutionTest : WebServiceTest<WebBugService.TransitBugResolution, WebBugServiceNoCache>
    {
        public BugResolutionTest()
            : base("BugResolution")
        {
        }


        public override WebBugService.TransitBugResolution GetTransitInstance()
        {
            WebBugService.TransitBugResolution t_instance = new WebBugService.TransitBugResolution();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
