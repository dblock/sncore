using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountLicenseTest : ManagedCRUDTest<AccountLicense, TransitAccountLicense, ManagedAccountLicense>
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

        public ManagedAccountLicenseTest()
        {

        }

        public override TransitAccountLicense GetTransitInstance()
        {
            TransitAccountLicense t_instance = new TransitAccountLicense();
            t_instance.ImageUrl = Guid.NewGuid().ToString();
            t_instance.LicenseUrl = Guid.NewGuid().ToString(); 
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.AccountId = _account.Instance.Id;
            return t_instance;
        }
    }
}
