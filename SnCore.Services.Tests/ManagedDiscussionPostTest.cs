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
            _account.SetUp();
            _thread.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _thread.TearDown();
            _account.TearDown();
        }

        public ManagedDiscussionPostTest()
        {

        }

        public override TransitDiscussionPost GetTransitInstance()
        {
            TransitDiscussionPost t_instance = new TransitDiscussionPost();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Body = GetNewString();
            t_instance.DiscussionId = _thread.Instance.Instance.Discussion.Id;
            t_instance.DiscussionThreadId = _thread.Instance.Id;
            t_instance.Subject = GetNewString();
            t_instance.DiscussionPostParentId = 0;
            return t_instance;
        }

        [Test] // foodcandy bug #421 : Discuss: edit button not visible for author 
        public void TestCanEdit()
        {
            ManagedAccount account = new ManagedAccount(Session);
            ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session);
            try
            {
                string email = GetNewEmailAddress();
                TransitAccount t_instance = new TransitAccount();
                t_instance.Password = GetNewString();
                t_instance.Name = GetNewString();
                t_instance.Birthday = DateTime.UtcNow;
                int account_id = account.Create(email, t_instance, GetSecurityContext());

                TransitDiscussionPost t_post = GetTransitInstance();
                t_post.AccountId = GetSecurityContext().Account.Id;
                m_post.CreateOrUpdate(t_post, GetSecurityContext());
                Session.Flush();

                ManagedDiscussionPost m_post1 = new ManagedDiscussionPost(Session, m_post.Id);
                
                TransitDiscussionPost t_post1 = m_post1.GetTransitInstance(GetSecurityContext());
                Assert.IsTrue(t_post1.CanDelete, "Owner should be able to delete his own message.");
                Assert.IsTrue(t_post1.CanEdit, "Owner should be able to edit his own message.");

                TransitDiscussionPost t_post2 = m_post1.GetTransitInstance(account.GetSecurityContext());
                Assert.IsFalse(t_post2.CanDelete, "User shouldn't be able to delete someone else's message.");
                Assert.IsFalse(t_post2.CanEdit, "User shouldn't be able to edit someone else's message.");
            }
            finally
            {
                account.Delete(GetSecurityContext());
                m_post.Delete(GetSecurityContext());
            }
        }

        [Test, ExpectedException(typeof(ManagedAccount.NoVerifiedEmailException))]
        public void TestNoVerifiedEmail()
        {
            ManagedAccount account = new ManagedAccount(Session);
            try
            {
                string email = GetNewEmailAddress();
                TransitAccount t_instance = new TransitAccount();
                t_instance.Password = GetNewString();
                t_instance.Name = GetNewString();
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

        [Test]
        public void TestGetDiscussionPosts()
        {
            IList<DiscussionPost> posts = ManagedDiscussionPost.GetDiscussionPosts(
                Session, _account.Instance.Id, DateTime.UtcNow.AddDays(-1));
            Console.WriteLine("Posts: {0}", posts.Count);
        }
    }
}
