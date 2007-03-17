using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountContentTest : ManagedCRUDTest<AccountContent, TransitAccountContent, ManagedAccountContent>
    {
        private ManagedAccountContentGroupTest _accountcontentgroup = new ManagedAccountContentGroupTest();

        [SetUp]
        public override void SetUp()
        {
            _accountcontentgroup.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountcontentgroup.TearDown();
        }

        public ManagedAccountContentTest()
        {

        }

        public override TransitAccountContent GetTransitInstance()
        {
            TransitAccountContent t_instance = new TransitAccountContent();
            t_instance.AccountContentGroupId = _accountcontentgroup.Instance.Id;
            t_instance.Tag = GetNewString();
            t_instance.Text = GetNewString();
            t_instance.Timestamp = DateTime.UtcNow;
            return t_instance;
        }
    }
}
