using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEmailTest : ManagedServiceTest
    {
        public ManagedAccountEmailTest()
        {

        }

        [Test]
        public void AddAccountEmail()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                a.Create(e, AdminSecurityContext);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void DeleteAccountEmail()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                ManagedAccountEmail addedemail = new ManagedAccountEmail(Session, a.Create(e, AdminSecurityContext));
                addedemail.Delete(AdminSecurityContext);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(SoapException))]
        public void DeleteAccountEmailInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                IList list = Session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .List();

                foreach (AccountEmail e in list)
                {
                    ManagedAccountEmail email = new ManagedAccountEmail(Session, e);
                    email.Delete(AdminSecurityContext);
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void DeleteAccountEmailConfirmed()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                ManagedAccountEmail addedemail = new ManagedAccountEmail(Session, a.Create(e, AdminSecurityContext));
                addedemail.Delete(AdminSecurityContext);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(SoapException))]
        public void DeleteAccountEmailInvalidConfirmed()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();

                IList list = Session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .List();

                foreach (AccountEmail e in list)
                {
                    ManagedAccountEmail email = new ManagedAccountEmail(Session, e);
                    email.Delete(AdminSecurityContext);
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

    }
}
