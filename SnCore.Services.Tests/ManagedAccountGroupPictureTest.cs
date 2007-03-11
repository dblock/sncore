using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupPictureTest : ManagedCRUDTest<AccountGroupPicture, TransitAccountGroupPicture, ManagedAccountGroupPicture>
    {
        private ManagedAccountGroupTest _group = new ManagedAccountGroupTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _group.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            _group.TearDown();
            base.TearDown();
        }

        public ManagedAccountGroupPictureTest()
        {

        }

        public override TransitAccountGroupPicture GetTransitInstance()
        {
            TransitAccountGroupPicture t_instance = new TransitAccountGroupPicture();
            t_instance.Bitmap = new byte[128];
            t_instance.Name = GetNewString();
            t_instance.AccountGroupId = _group.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
