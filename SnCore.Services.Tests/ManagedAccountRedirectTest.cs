using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountRedirectTest : ManagedCRUDTest<AccountRedirect, TransitAccountRedirect, ManagedAccountRedirect>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            base.TearDown();
        }

        public ManagedAccountRedirectTest()
        {

        }

        public override TransitAccountRedirect GetTransitInstance()
        {
            TransitAccountRedirect t_instance = new TransitAccountRedirect();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.SourceUri = string.Format("{0}", _account.Instance.Id);
            t_instance.TargetUri = string.Format("/target/{0}", _account.Instance.Id);
            return t_instance;
        }
    }
}
