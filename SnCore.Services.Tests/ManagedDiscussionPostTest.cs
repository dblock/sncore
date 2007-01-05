using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDiscussionPostTest : ManagedCRUDTest<DiscussionPost, TransitDiscussionPost, ManagedDiscussionPost>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedDiscussionThreadTest _thread = new ManagedDiscussionThreadTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _thread.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _thread.TearDown();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedDiscussionPostTest()
        {

        }

        public override TransitDiscussionPost GetTransitInstance()
        {
            TransitDiscussionPost t_instance = new TransitDiscussionPost();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Body = Guid.NewGuid().ToString();
            t_instance.DiscussionId = _thread.Instance.Instance.Discussion.Id;
            t_instance.DiscussionThreadId = _thread.Instance.Id;
            t_instance.Subject = Guid.NewGuid().ToString();
            t_instance.DiscussionPostParentId = 0;
            return t_instance;
        }
    }
}
