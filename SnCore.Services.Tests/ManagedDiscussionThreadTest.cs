using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDiscussionThreadTest : ManagedCRUDTest<DiscussionThread, TransitDiscussionThread, ManagedDiscussionThread>
    {
        private ManagedDiscussionTest _discussion = new ManagedDiscussionTest();

        [SetUp]
        public override void SetUp()
        {
            _discussion.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _discussion.TearDown();
        }

        public ManagedDiscussionThreadTest()
        {

        }

        public override TransitDiscussionThread GetTransitInstance()
        {
            TransitDiscussionThread t_instance = new TransitDiscussionThread();
            t_instance.DiscussionId = _discussion.Instance.Id;
            return t_instance;
        }
    }
}
