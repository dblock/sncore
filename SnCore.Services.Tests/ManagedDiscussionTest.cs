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
        private ManagedDataObjectTest _object = new ManagedDataObjectTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _object.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            _object.TearDown();
            base.TearDown();
        }

        public ManagedDiscussionTest()
        {

        }

        public override TransitDiscussion GetTransitInstance()
        {
            TransitDiscussion t_instance = new TransitDiscussion();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.ObjectId = _object.Instance.Id;
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

                TransitDiscussion t = new TransitDiscussion();
                t.Name = Guid.NewGuid().ToString();
                t.Description = Guid.NewGuid().ToString();
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

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = Guid.NewGuid().ToString();
                t_d.Name = Guid.NewGuid().ToString();
                t_d.Personal = false;
                d.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = Guid.NewGuid().ToString();
                t_p.Body = Guid.NewGuid().ToString();
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

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = Guid.NewGuid().ToString();
                t_d.Name = Guid.NewGuid().ToString();
                t_d.Personal = false;
                d.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = Guid.NewGuid().ToString();
                t_p.Body = Guid.NewGuid().ToString();
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

                TransitDiscussion t_d = new TransitDiscussion();
                t_d.Description = Guid.NewGuid().ToString();
                t_d.Name = Guid.NewGuid().ToString();
                t_d.Personal = false;
                d1.CreateOrUpdate(t_d, a.GetSecurityContext());

                t_d.Name = Guid.NewGuid().ToString();
                d2.CreateOrUpdate(t_d, a.GetSecurityContext());

                TransitDiscussionPost t_p = new TransitDiscussionPost();
                t_p.Subject = Guid.NewGuid().ToString();
                t_p.Body = Guid.NewGuid().ToString();
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

    }
}
