using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDiscussionTest : ManagedCRUDTest<Discussion, TransitDiscussion, ManagedDiscussion>
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

        public ManagedDiscussionTest()
        {

        }

        public override TransitDiscussion GetTransitInstance()
        {
            TransitDiscussion t_instance = new TransitDiscussion();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public void CreateDiscussion()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                TransitDiscussion t = new TransitDiscussion();
                t.Name = GetNewString();
                t.Description = GetNewString();
                t.AccountId = a.Id;
                t.Personal = false;
                t.Created = t.Modified = DateTime.UtcNow;
                d.CreateOrUpdate(t, a.GetSecurityContext());

                Session.Flush();
            }
            finally
            {
                d.Delete(a.GetSecurityContext());
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        public void CreateDiscussionThread()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);
            ManagedDiscussionPost p = new ManagedDiscussionPost(Session);
            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = GetNewString();
                t_d.Name = GetNewString();
                t_d.Personal = false;
                d.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = GetNewString();
                t_p.Body = GetNewString();
                t_p.DiscussionId = d.Id;
                p.CreateOrUpdate(t_p, a.GetSecurityContext());

                Session.Flush();
            }
            finally
            {
                p.Delete(a.GetSecurityContext());
                d.Delete(a.GetSecurityContext());
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        public void CreateDiscussionThreadDeep()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);
            ManagedDiscussionPost p = new ManagedDiscussionPost(Session);
            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = GetNewString();
                t_d.Name = GetNewString();
                t_d.Personal = false;
                d.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = GetNewString();
                t_p.Body = GetNewString();
                t_p.DiscussionId = d.Id;

                int id1 = p.CreateOrUpdate(t_p, a.GetSecurityContext());
                Assert.AreNotEqual(0, id1);

                t_p.DiscussionPostParentId = id1;
                int id2 = p.CreateOrUpdate(t_p, a.GetSecurityContext());
                Assert.AreNotEqual(0, id2);

                Session.Flush();
            }
            finally
            {
                p.Delete(a.GetSecurityContext());
                d.Delete(a.GetSecurityContext());
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateDiscussionThreadInvalidChild()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d1 = new ManagedDiscussion(Session);
            ManagedDiscussion d2 = new ManagedDiscussion(Session);
            ManagedDiscussionPost p = new ManagedDiscussionPost(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = GetNewString();
                t_d.Name = GetNewString();
                t_d.Personal = false;
                d1.CreateOrUpdate(t_d, a.GetSecurityContext());

                t_d.Name = GetNewString();
                d2.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = GetNewString();
                t_p.Body = GetNewString();
                t_p.DiscussionId = d1.Id;

                int id1 = p.CreateOrUpdate(t_p, a.GetSecurityContext());
                Assert.AreNotEqual(0, id1);

                t_p.DiscussionId = d2.Id;
                int id2 = p.CreateOrUpdate(t_p, a.GetSecurityContext());
                Assert.AreNotEqual(0, id2);

                // can't create child of other discussion
                t_p.DiscussionId = d1.Id;
                t_p.DiscussionPostParentId = id2;
                int id3 = p.CreateOrUpdate(t_p, a.GetSecurityContext());

                Session.Flush();
            }
            finally
            {
                d1.Delete(AdminSecurityContext);
                d2.Delete(AdminSecurityContext);
                a.Delete(AdminSecurityContext);
            }
        }

        [Test] // foodcandy bug #413 - Discuss: Threads and posts count missing.
        public void GetDiscussionPostAndThreadCountTest()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);
            ManagedDiscussionPost p = new ManagedDiscussionPost(Session);
            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = GetNewString();
                t_d.Name = GetNewString();
                t_d.Personal = false;
                d.CreateOrUpdate(t_d, a.GetSecurityContext());

                Session.Flush();

                TransitDiscussion t_instance1 = d.GetTransitInstance(AdminSecurityContext);
                Assert.AreEqual(0, t_instance1.PostCount);
                Assert.AreEqual(0, t_instance1.ThreadCount);

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = GetNewString();
                t_p.Body = GetNewString();
                t_p.DiscussionId = d.Id;
                p.CreateOrUpdate(t_p, a.GetSecurityContext());

                Session.Flush();

                TransitDiscussion t_instance2 = d.GetTransitInstance(AdminSecurityContext);
                Assert.AreEqual(1, t_instance2.PostCount);
                Assert.AreEqual(1, t_instance2.ThreadCount);
            }
            finally
            {
                p.Delete(a.GetSecurityContext());
                d.Delete(a.GetSecurityContext());
                a.Delete(a.GetSecurityContext());
            }
        }

    }
}
