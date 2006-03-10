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
    public class ManagedAccountFriendTest : NHibernateTest
    {
        public ManagedAccountFriendTest()
        {

        }

        [Test]
        public void CreateAccountFriend()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow);
                b.VerifyAllEmails();

                a.CreateAccountFriendRequest(
                    b.Id,
                    "Please be my friend!");
            }
            finally
            {
                a.Delete();
                b.Delete();
            }
        }

        [Test]
        public void AcceptAccountFriend()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow);
                b.VerifyAllEmails();

                ManagedAccountFriendRequest r = new ManagedAccountFriendRequest(Session, a.CreateAccountFriendRequest(
                    b.Id,
                    "Please be my friend!"));

                r.Accept("thanks!");
            }
            finally
            {
                a.Delete();
                b.Delete();
            }
        }

        [Test]
        public void RejectAccountFriend()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow);
                b.VerifyAllEmails();

                ManagedAccountFriendRequest r = new ManagedAccountFriendRequest(Session, a.CreateAccountFriendRequest(
                    b.Id,
                    "Please be my friend!"));

                r.Reject("no thanks!");
            }
            finally
            {
                a.Delete();
                b.Delete();
            }
        }

    }
}
