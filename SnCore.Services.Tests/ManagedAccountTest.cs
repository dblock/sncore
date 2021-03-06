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
    public class ManagedAccountTest : ManagedCRUDTest<Account, TransitAccount, ManagedAccount>
    {
        public ManagedAccountTest()
        {

        }

        public override ManagedAccount Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ManagedAccount(Session);
                    _instance.Create(GetNewString(), "password", string.Format("{0}@domain.com", Guid.NewGuid()), 
                        DateTime.UtcNow, AdminSecurityContext);
                    // need verified e-mail to post
                    _instance.VerifyAllEmails();
                }
                return base.Instance;
            }
        }

        public override TransitAccount GetTransitInstance()
        {
            TransitAccount t_instance = new TransitAccount();
            t_instance.Birthday = DateTime.UtcNow;
            t_instance.LastLogin = DateTime.UtcNow;
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public override void TestCreateAndDelete()
        {
            Assert.IsTrue(Instance.Id > 0);
        }

        [Test]
        public override void TestUpdateAndRetrieve()
        {
            
        }

        [Test]
        public void CreateAccount()
        {
            ManagedAccount a = new ManagedAccount(Session);
            a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
            Session.Flush();
            a.Delete(AdminSecurityContext);
            Session.Flush();
        }

        [Test]
        public void Login()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount b = ManagedAccount.Login(Session, "logintest@localhost.com", "password");
                Assert.AreEqual(b.Id, a.Id);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void LoginMd5()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount b = ManagedAccount.LoginMd5(Session, "logintest@localhost.com", ManagedAccount.GetPasswordHash("password"));
                Assert.AreEqual(b.Id, a.Id);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.InvalidUsernamePasswordException))]
        public void LoginInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                Session.Flush();

                ManagedAccount.Login(Session, "logintest@localhost.com", "invalid password");
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.InvalidUsernamePasswordException))]
        public void LoginEmailUnverified()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();
                ManagedAccount.Login(Session, "logintest@localhost.com", "password");
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }
        }

        [Test]
        public void ChangePassword()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();
                a.ChangePassword("password", "newpassword");
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void ChangePasswordInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();
                a.ChangePassword("invalid password", "newpassword");
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedAccount.PasswordTooShortException))]
        public void ChangePasswordTooShort()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();
                a.ChangePassword("password", "new");
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestAdministrator()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("AdministratorTestUser", "password", "administrator@localhost.com", DateTime.UtcNow, AdminSecurityContext);
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

                a.PromoteAdministrator();
                Session.Flush();

                Account ar = Session.Load<Account>(a.Id);
                Session.Refresh(ar);

                ManagedAccount mar = new ManagedAccount(Session, ar);
                Assert.AreEqual(mar.IsAdministrator(), true);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestAddress()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();
                // TODO
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestAccountActivity()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("LoginTestUser", "password", "logintest@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                Session.Flush();

                ManagedAccountActivity m_activity = new ManagedAccountActivity(Session, a.Instance);
                TransitAccountActivity t_activity = m_activity.GetTransitInstance(GetSecurityContext());
                Console.WriteLine("New syndicated content: {0}", t_activity.NewSyndicatedContent);
                Console.WriteLine("New discussion posts: {0}", t_activity.NewDiscussionPosts);
                Console.WriteLine("New pictures: {0}", t_activity.NewPictures);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestAccountLocaleLCID()
        {
            TransitAccount t_a = GetTransitInstance();
            Console.WriteLine("LCID: {0}", t_a.LCID);
            Console.WriteLine("Culture: {0}", t_a.Culture);
            t_a.LCID = 1025;
            Console.WriteLine("LCID: {0}", t_a.LCID);
            Console.WriteLine("Culture: {0}", t_a.Culture);
            Assert.AreEqual(t_a.Culture, "ar-SA");
        }
    }
}
