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

        [Test, ExpectedException(typeof(ManagedAccount.NoVerifiedEmailException))]
        public void TestNoVerifiedEmail()
        {
            ManagedAccount account = new ManagedAccount(Session);
            try
            {
                string email = string.Format("{0}@localhost.com", Guid.NewGuid());
                TransitAccount t_instance = new TransitAccount();
                t_instance.Password = Guid.NewGuid().ToString();
                t_instance.Name = Guid.NewGuid().ToString();
                t_instance.Birthday = DateTime.UtcNow;
                int account_id = account.Create(email, t_instance, GetSecurityContext());

                TransitDiscussionPost t_post = GetTransitInstance();
                t_post.AccountId = account.Id;
                ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session);
                m_post.CreateOrUpdate(t_post, account.GetSecurityContext());
            }
            finally
            {
                account.Delete(GetSecurityContext());
            }
        }
    }
}
