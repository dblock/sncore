using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountContentGroupTest : ManagedCRUDTest<AccountContentGroup, TransitAccountContentGroup, ManagedAccountContentGroup>
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

        public ManagedAccountContentGroupTest()
        {

        }

        public override TransitAccountContentGroup GetTransitInstance()
        {
            TransitAccountContentGroup t_instance = new TransitAccountContentGroup();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Trusted = true;
            t_instance.Login = false;
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
