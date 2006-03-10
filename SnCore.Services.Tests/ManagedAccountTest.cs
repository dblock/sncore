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
    public class ManagedAccountTest : NHibernateTest
    {
        public ManagedAccountTest()
        {

        }

        [Test]
        public void CreateAccount()
        {
            ManagedAccount a = new ManagedAccount(Session);
            a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
            Session.Flush();
            a.Delete();
            Session.Flush();
        }

        [Test]
        public void Login()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount b = ManagedAccount.Login(Session, "logintest@localhost.com", "password");
                Assert.AreEqual(b.Id, a.Id);
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void LoginMd5()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount b = ManagedAccount.LoginMd5(Session, "logintest@localhost.com", ManagedAccount.GetPasswordHash("password"));
                Assert.AreEqual(b.Id, a.Id);
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void LoginInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount.Login(Session, "logintest@localhost.com", "invalid password");
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void LoginEmailUnverified()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                b.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                Session.Flush();
                ManagedAccount.Login(Session, "logintest@localhost.com", "password");
            }
            finally
            {
                a.Delete();
                b.Delete();
            }
        }

        [Test]
        public void ChangePassword()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                Session.Flush();
                a.ChangePassword("password", "newpassword");
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void ChangePasswordInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                Session.Flush();
                a.ChangePassword("invalid password", "newpassword");
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.PasswordTooShortException))]
        public void ChangePasswordTooShort()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                Session.Flush();
                a.ChangePassword("password", "new");
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void TestAdministrator()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("AdministratorTestUser", "password", "administrator@localhost.com", DateTime.UtcNow);
                Session.Flush();
                Assert.AreEqual(a.IsAdministrator(), false);

                IList dataobjects = Session.CreateCriteria(typeof(DataObject))
                    .Add(Expression.Eq("Name", typeof(Account).Name)).List();

                // fetch the account data object
                DataObject o = null;
                if (dataobjects.Count == 0)
                {
                    o = new DataObject();
                    o.Name = typeof(Account).Name;
                    Session.Save(o);
                    Session.Flush();
                }
                else
                {
                    o = (DataObject)dataobjects[0];
                }

                a.CreateAccountRight(o, new ManagedAccountRightBits(true));
                Session.Flush();

                Account ar = (Account)Session.Load(typeof(Account), a.Id);
                Session.Refresh(ar);

                ManagedAccount mar = new ManagedAccount(Session, ar);
                Assert.AreEqual(mar.IsAdministrator(), true);
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void TestAddress()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow);
                Session.Flush();
                // TODO
            }
            finally
            {
                a.Delete();
            }
        }

    }
}
