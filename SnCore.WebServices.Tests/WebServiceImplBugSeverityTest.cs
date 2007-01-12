using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using SnCore.Services;
using NUnit.Framework;

namespace SnCore.WebServices.Tests
{
    [TestFixture]
    public class WebServiceImplBugSeverityTest
        : WebServiceImplTest<TransitBugSeverity, ManagedBugSeverity, BugSeverity>
    {
        public override TransitBugSeverity CreateTransitInstance()
        {
            TransitBugSeverity t_instance = new TransitBugSeverity();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        public WebServiceImplBugSeverityTest()
        {

        }
    }
}
