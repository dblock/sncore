using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugProjectTest : WebServiceTest<WebBugService.TransitBugProject, WebBugServiceNoCache>
    {
        public BugProjectTest()
            : base("BugProject")
        {
        }


        public override WebBugService.TransitBugProject GetTransitInstance()
        {
            WebBugService.TransitBugProject t_instance = new WebBugService.TransitBugProject();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}
