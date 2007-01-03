using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Expression;
using SnCore.Data.Tests;
using System.Collections;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEmailConfirmationTest : ManagedServiceTest
    {
        public ManagedAccountEmailConfirmationTest()
        {

        }

        [Test]
        public void VerifyEmail()
        {
            ManagedAccount a = new ManagedAccount(Session);
            a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
            Session.Flush();

            try
            {
                a.VerifyAllEmails();
            }
            finally
            {
                a.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccountEmailConfirmation.InvalidCodeException))]
        public void VerifyEmailInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);
            a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
            Session.Flush();

            try
            {
                IList list = Session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .List();

                foreach (AccountEmail e in list)
                {
                    IList confirmationslist = Session.CreateCriteria(typeof(AccountEmailConfirmation))
                        .Add(Expression.Eq("AccountEmail.Id", e.Id))
                        .List();

                    foreach (AccountEmailConfirmation c in confirmationslist)
                    {
                        Assert.AreEqual(false, c.AccountEmail.Verified);
                        new ManagedAccountEmailConfirmation(Session, c.Id).Verify("invalid code");
                        Assert.AreEqual(false, c.AccountEmail.Verified);
                    }
                }
                Session.Flush();
            }
            finally
            {
                a.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }

        [Test]
        public void VerifyEmailWithReclaim()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("TestVerifyEmailReclaim1", "password", "reclaim@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                b.Create("TestVerifyEmailReclaim2", "password", "reclaim@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.VerifyAllEmails();

                {
                    IList list = Session.CreateCriteria(typeof(AccountEmail))
                        .Add(Expression.Eq("Account.Id", a.Id))
                        .List();

                    foreach (AccountEmail e in list)
                    {
                        Assert.AreEqual(false, e.Verified);
                    }
                }

                {
                    IList list = Session.CreateCriteria(typeof(AccountEmail))
                        .Add(Expression.Eq("Account.Id", b.Id))
                        .List();

                    foreach (AccountEmail e in list)
                    {
                        Assert.AreEqual(true, e.Verified);
                    }
                }
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }

            Session.Flush();
        }
    }
}
