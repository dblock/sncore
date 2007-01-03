using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using SnCore.Services;
using NUnit.Framework;

namespace SnCore.WebServices.Tests
{
    [TestFixture]
    public class WebServiceImplBugPriorityTest 
        : WebServiceImplTest<TransitBugPriority, ManagedBugPriority, BugPriority>
    {
        public override TransitBugPriority CreateTransitInstance()
        {
            TransitBugPriority t_instance = new TransitBugPriority();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        public WebServiceImplBugPriorityTest()
        {

        }
    }
}
