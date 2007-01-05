using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererHostTest : ManagedCRUDTest<RefererHost, TransitRefererHost, ManagedRefererHost>
    {
        public ManagedRefererHostTest()
        {

        }

        public override TransitRefererHost GetTransitInstance()
        {
            TransitRefererHost t_instance = new TransitRefererHost();
            t_instance.Host = Guid.NewGuid().ToString();
            t_instance.LastRefererUri = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.LastRequestUri = string.Format("http://uri/{0}", Guid.NewGuid()); 
            return t_instance;
        }
    }
}
