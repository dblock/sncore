using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountStoryPictureTest : ManagedCRUDTest<AccountStoryPicture, TransitAccountStoryPicture, ManagedAccountStoryPicture>
    {
        private ManagedAccountStoryTest _accountstory = new ManagedAccountStoryTest();

        [SetUp]
        public override void SetUp()
        {
            _accountstory.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountstory.TearDown();
        }


        public ManagedAccountStoryPictureTest()
        {

        }

        public override TransitAccountStoryPicture GetTransitInstance()
        {
            TransitAccountStoryPicture t_instance = new TransitAccountStoryPicture();
            t_instance.Name = GetNewString();
            t_instance.AccountStoryId = _accountstory.Instance.Id;
            return t_instance;
        }
    }
}
