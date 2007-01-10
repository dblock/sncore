using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugTypeTest : WebServiceTest<WebBugService.TransitBugType, WebBugServiceNoCache>
    {
        public BugTypeTest()
            : base("BugType")
        {
        }


        public override WebBugService.TransitBugType GetTransitInstance()
        {
            WebBugService.TransitBugType t_instance = new WebBugService.TransitBugType();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
