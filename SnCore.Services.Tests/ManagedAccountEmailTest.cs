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
    public class ManagedAccountEmailTest : NHibernateTest
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
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                a.Create(e);
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void DeleteAccountEmail()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                ManagedAccountEmail addedemail = new ManagedAccountEmail(Session, a.Create(e));
                addedemail.Delete();
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(SoapException))]
        public void DeleteAccountEmailInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                IList list = Session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .List();

                foreach (AccountEmail e in list)
                {
                    ManagedAccountEmail email = new ManagedAccountEmail(Session, e);
                    email.Delete();
                }
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void DeleteAccountEmailConfirmed()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();
                TransitAccountEmail e = new TransitAccountEmail();
                e.Address = "foo2@localhost.com";
                e.Created = e.Modified = DateTime.UtcNow;
                ManagedAccountEmail addedemail = new ManagedAccountEmail(Session, a.Create(e));
                addedemail.Delete();
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(SoapException))]
        public void DeleteAccountEmailInvalidConfirmed()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();

                IList list = Session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .List();

                foreach (AccountEmail e in list)
                {
                    ManagedAccountEmail email = new ManagedAccountEmail(Session, e);
                    email.Delete();
                }
            }
            finally
            {
                a.Delete();
            }
        }

    }
}
