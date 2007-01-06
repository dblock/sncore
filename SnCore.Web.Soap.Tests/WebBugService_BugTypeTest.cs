using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugService_BugTypeTest : WebServiceTest<WebBugService.TransitBugType>
    {
        public WebBugService_BugTypeTest()
            : base("BugType", new WebBugService.WebBugService())
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
