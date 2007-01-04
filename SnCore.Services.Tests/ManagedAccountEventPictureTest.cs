using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventPictureTest : ManagedCRUDTest<AccountEventPicture, TransitAccountEventPicture, ManagedAccountEventPicture>
    {
        private ManagedAccountEventTest _accountevent = new ManagedAccountEventTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _accountevent.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountevent.TearDown();
            base.TearDown();
        }


        public ManagedAccountEventPictureTest()
        {

        }

        public override TransitAccountEventPicture GetTransitInstance()
        {
            TransitAccountEventPicture t_instance = new TransitAccountEventPicture();
            t_instance.AccountEventId = _accountevent.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
