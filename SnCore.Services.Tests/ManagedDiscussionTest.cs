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

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                d.Create("New Discussion", "Description", false, a.GetSecurityContext());
                d.CreatePost(a.Id, 0, "subject", "body", a.GetSecurityContext());
                Session.Flush();
            }
            finally
            {
                d.Delete(a.GetSecurityContext());
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        public void CreateDiscussionThreadDeep()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                d.Create("New Discussion", "Description", false, a.GetSecurityContext());
                int newid = d.CreatePost(a.Id, 0, "subject", "body", a.GetSecurityContext());
                d.CreatePost(a.Id, newid, "subject", "body", a.GetSecurityContext());
                Session.Flush();
            }
            finally
            {
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

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                
                d1.Create("New Discussion1", "Description1", false, a.GetSecurityContext());
                int d1newid = d1.CreatePost(a.Id, 0, "subject1", "body1", a.GetSecurityContext());

                d2.Create("New Discussion2", "Description2", false, a.GetSecurityContext());
                d2.CreatePost(a.Id, 0, "subject2", "body2", a.GetSecurityContext());

                // can't create child of other discussion
                d2.CreatePost(a.Id, d1newid, "subject", "body", a.GetSecurityContext());
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
