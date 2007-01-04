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
            base.SetUp();
            _accountstory.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountstory.TearDown();
            base.TearDown();
        }


        public ManagedAccountStoryPictureTest()
        {

        }

        public override TransitAccountStoryPicture GetTransitInstance()
        {
            TransitAccountStoryPicture t_instance = new TransitAccountStoryPicture();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.AccountStoryId = _accountstory.Instance.Id;
            return t_instance;
        }
    }
}
