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
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
        }

        public ManagedAccountPictureTest()
        {

        }

        public override TransitAccountPicture GetTransitInstance()
        {
            TransitAccountPicture t_instance = new TransitAccountPicture();
            t_instance.Name = GetNewString();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Bitmap = new byte[128];
            return t_instance;
        }
    }
}
