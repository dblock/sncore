using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountAuditEntryTest : ManagedCRUDTest<AccountAuditEntry, TransitAccountAuditEntry, ManagedAccountAuditEntry>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
        }

        public ManagedAccountAuditEntryTest()
        {

        }

        public override TransitAccountAuditEntry GetTransitInstance()
        {
            TransitAccountAuditEntry t_instance = new TransitAccountAuditEntry();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.IsPrivate = false;
            t_instance.IsSystem = true;
            t_instance.Url = GetNewUri();
            return t_instance;
        }
    }
}
