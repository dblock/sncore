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
            _accountevent.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountevent.TearDown();
        }


        public ManagedAccountEventPictureTest()
        {

        }

        public override TransitAccountEventPicture GetTransitInstance()
        {
            TransitAccountEventPicture t_instance = new TransitAccountEventPicture();
            t_instance.AccountEventId = _accountevent.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
