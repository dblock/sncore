using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugService_BugProjectTest : WebServiceTest<WebBugService.TransitBugProject>
    {
        public WebBugService_BugProjectTest()
            : base("BugProject", new WebBugService.WebBugService())
        {
        }


        public override WebBugService.TransitBugProject GetTransitInstance()
        {
            WebBugService.TransitBugProject t_instance = new WebBugService.TransitBugProject();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
