using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererHostDupTest : ManagedCRUDTest<RefererHostDup, TransitRefererHostDup, ManagedRefererHostDup>
    {
        private ManagedRefererHostTest _host = new ManagedRefererHostTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _host.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _host.TearDown();
            base.TearDown();
        }

        public ManagedRefererHostDupTest()
        {

        }

        public override TransitRefererHostDup GetTransitInstance()
        {
            TransitRefererHostDup t_instance = new TransitRefererHostDup();
            t_instance.RefererHost = _host.Instance.Instance.Host;
            t_instance.Host = string.Format("something.{0}", _host.Instance.Instance.Host);
            return t_instance;
        }
    }
}
