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
            base.SetUp();
            _accountcontentgroup.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountcontentgroup.TearDown();
            base.TearDown();
        }


        public ManagedAccountContentTest()
        {

        }

        public override TransitAccountContent GetTransitInstance()
        {
            TransitAccountContent t_instance = new TransitAccountContent();
            t_instance.AccountContentGroupId = _accountcontentgroup.Instance.Id;
            t_instance.Tag = Guid.NewGuid().ToString();
            t_instance.Text = Guid.NewGuid().ToString();
            t_instance.Timestamp = DateTime.UtcNow;
            return t_instance;
        }
    }
}
