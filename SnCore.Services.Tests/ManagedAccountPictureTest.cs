using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPictureTest : ManagedCRUDTest<AccountPicture, TransitAccountPicture, ManagedAccountPicture>
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

        public ManagedAccountPictureTest()
        {

        }

        public override TransitAccountPicture GetTransitInstance()
        {
            TransitAccountPicture t_instance = new TransitAccountPicture();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Bitmap = new byte[128];
            return t_instance;
        }
    }
}
