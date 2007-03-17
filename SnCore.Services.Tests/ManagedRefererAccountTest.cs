using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererAccountTest : ManagedCRUDTest<RefererAccount, TransitRefererAccount, ManagedRefererAccount>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedRefererHostTest _host = new ManagedRefererHostTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _host.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _host.TearDown();
            _account.TearDown();
        }

        public ManagedRefererAccountTest()
        {

        }

        public override TransitRefererAccount GetTransitInstance()
        {
            TransitRefererAccount t_instance = new TransitRefererAccount();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.RefererHostLastRefererUri = GetNewUri();
            t_instance.RefererHostName = _host.Instance.Instance.Host;
            t_instance.RefererHostTotal = 1;
            return t_instance;
        }
    }
}
