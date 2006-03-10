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
    public class ManagedDiscussionTest : NHibernateTest
    {
        public ManagedDiscussionTest()
        {

        }

        [Test]
        public void CreateDiscussion()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitDiscussion t = new TransitDiscussion();
                t.Name = Guid.NewGuid().ToString();
                t.Description = Guid.NewGuid().ToString();
                t.AccountId = a.Id;
                t.Personal = false;
                t.Created = t.Modified = DateTime.UtcNow;
                d.Create(t);

                Session.Flush();
            }
            finally
            {
                d.Delete();
                a.Delete();
            }
        }

        [Test]
        public void CreateDiscussionThread()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                d.Create("New Discussion", "Description", a.Id, false);
                d.CreatePost(a.Id, 0, "subject", "body");
                Session.Flush();
            }
            finally
            {
                d.Delete();
                a.Delete();
            }
        }

        [Test]
        public void CreateDiscussionThreadDeep()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedDiscussion d = new ManagedDiscussion(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                d.Create("New Discussion", "Description", a.Id, false);
                int newid = d.CreatePost(a.Id, 0, "subject", "body");
                d.CreatePost(a.Id, newid, "subject", "body");
                Session.Flush();
            }
            finally
            {
                d.Delete();
                a.Delete();
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
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                d1.Create("New Discussion1", "Description1", a.Id, false);
                int d1newid = d1.CreatePost(a.Id, 0, "subject1", "body1");

                d2.Create("New Discussion2", "Description2", a.Id, false);
                d2.CreatePost(a.Id, 0, "subject2", "body2");

                // can't create child of other discussion
                d2.CreatePost(a.Id, d1newid, "subject", "body");
                Session.Flush();
            }
            finally
            {
                d1.Delete();
                d2.Delete();
                a.Delete();
            }
        }

    }
}
