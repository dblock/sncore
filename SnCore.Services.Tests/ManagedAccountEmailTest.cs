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
                
                TransitAccountEmail t_instance = new TransitAccountEmail();
                t_instance.Address = "foo2@localhost.com";
                t_instance.AccountId = a.Id;
                ManagedAccountEmail m_instance = new ManagedAccountEmail(Session);
                m_instance.CreateOrUpdate(t_instance, a.GetSecurityContext());
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

                TransitAccountEmail t_instance = new TransitAccountEmail();
                t_instance.Address = "foo2@localhost.com";
                t_instance.AccountId = a.Id;
                ManagedAccountEmail m_instance = new ManagedAccountEmail(Session);
                m_instance.CreateOrUpdate(t_instance, a.GetSecurityContext());
                m_instance.Delete(AdminSecurityContext);
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
