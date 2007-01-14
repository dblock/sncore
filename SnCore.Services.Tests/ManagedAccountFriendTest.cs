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
    public class ManagedAccountFriendTest : ManagedServiceTest
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
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.VerifyAllEmails();

                a.CreateAccountFriendRequest(
                    AdminSecurityContext,
                    b.Id,
                    "Please be my friend!");
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }
        }

        [Test]
        public void AcceptAccountFriend()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.VerifyAllEmails();

                ManagedAccountFriendRequest r = new ManagedAccountFriendRequest(Session, a.CreateAccountFriendRequest(
                    AdminSecurityContext,
                    b.Id,
                    "Please be my friend!"));

                r.Accept(AdminSecurityContext, "thanks!");
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }
        }

        [Test]
        public void RejectAccountFriend()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User 1", "testpassword", "foo1@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.VerifyAllEmails();

                ManagedAccountFriendRequest r = new ManagedAccountFriendRequest(Session, a.CreateAccountFriendRequest(
                    AdminSecurityContext,
                    b.Id,
                    "Please be my friend!"));

                r.Reject(AdminSecurityContext, "no thanks!");
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }
        }

    }
}
