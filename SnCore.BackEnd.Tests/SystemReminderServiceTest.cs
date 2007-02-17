using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;
using SnCore.Services;
using SnCore.Tools.Web;
using NHibernate.Expression;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemReminderServicesTest : NHibernateTest
    {
        private SystemReminderService service = null;

        [SetUp]
        public override void SetUp()
        {
            service = new SystemReminderService();
#if DEBUG
            ContentPage.EnableRemoteContent = false;
#endif
            base.SetUp();
        }

        [Test]
        public void TestCleanupStaleAccounts_2()
        {
            ManagedSecurityContext sec = ManagedAccount.GetAdminSecurityContext(Session);
            string email = string.Format("{0}@sncore.com", Guid.NewGuid().ToString());

            // create an account with an unverified e-mail, make sure the e-mail is sent
            ManagedAccount ma = new ManagedAccount(Session);
            int id = ma.Create("TestCleanupStaleAccounts", "password", email, DateTime.UtcNow, sec);

            ManagedAccount mb = new ManagedAccount(Session);
            int id2 = mb.Create("TestCleanupStaleAccounts", "password", email, DateTime.UtcNow, sec);
            mb.VerifyAllEmails();

            bool fDeleted = false;

            try
            {
                Session.Flush();

                // get the account as is, update the login date/time to four months behind
                Account a = Session.Load<Account>(id);
                a.LastLogin = DateTime.UtcNow.AddMonths(-4);
                Session.Save(a);
                Session.Flush();

                // this should delete the stale account, since there's another one verified
                service.RunCleanupStaleAccounts(Session, ManagedAccount.GetAdminSecurityContext(Session));

                a = (Account)Session.CreateCriteria(typeof(Account))
                    .Add(Expression.Eq("Id", id))
                    .UniqueResult();

                Assert.IsNull(a);
                fDeleted = true;
            }
            finally
            {
                if (!fDeleted)
                {
                    ma.Delete(sec);
                }

                mb.Delete(sec);
            }
        }

        [Test]
        public void TestCleanupStaleAccounts_1()
        {
            ManagedSecurityContext sec = ManagedAccount.GetAdminSecurityContext(Session);
            string email = string.Format("{0}@sncore.com", Guid.NewGuid().ToString());
 
            // create an account with an unverified e-mail, make sure the e-mail is sent
            ManagedAccount ma = new ManagedAccount(Session);
            int id = ma.Create("TestCleanupStaleAccounts", "password", email, DateTime.UtcNow, sec);

            bool fDeleted = false;

            try
            {
                Session.Flush();

                // get the account as is, update the login date/time to four months behind
                Account a = Session.Load<Account>(id);
                a.LastLogin = DateTime.UtcNow.AddMonths(-4);
                Session.Save(a);
                Session.Flush();

                // update the e-mail as if it was added 3 months ago
                AccountEmail e = (AccountEmail) a.AccountEmails[0];
                Assert.AreEqual(e.Created, e.Modified);
                e.Created = e.Modified = a.LastLogin;
                Session.Save(e);

                // check that the account has a single confirmation e-mail pending
                Console.WriteLine("Email dates: {0}/{1}", e.Created, e.Modified);

                service.RunCleanupStaleAccounts(Session, ManagedAccount.GetAdminSecurityContext(Session));

                Session.Refresh(e);
                Console.WriteLine("Email dates: {0}/{1}", e.Created, e.Modified);
                Assert.AreNotEqual(e.Created, e.Modified);

                // move e-mail modified date in the past -> account should be deleted
                e.Modified = e.Modified.AddMonths(-3);
                Session.Save(e);
                Session.Flush();

                service.RunCleanupStaleAccounts(Session, ManagedAccount.GetAdminSecurityContext(Session));

                a = (Account)Session.CreateCriteria(typeof(Account))
                    .Add(Expression.Eq("Id", id))
                    .UniqueResult();

                Assert.IsNull(a);
                fDeleted = true;
            }
            finally
            {
                if (!fDeleted)
                {
                    ma.Delete(sec);
                }
            }
        }

        [Test]
        public void TestInvitationReminders()
        {
            service.RunInvitationReminders(Session, ManagedAccount.GetAdminSecurityContext(Session));
        }

        [Test]
        public void TestSystemReminders()
        {
            service.RunSystemReminders(Session, ManagedAccount.GetAdminSecurityContext(Session));
        }

        [Test]
        public void TestUpdateAccountCounters()
        {
            service.RunUpdateAccountCounters(Session, ManagedAccount.GetAdminSecurityContext(Session));
            int daily = Session.CreateQuery("SELECT SUM(CounterAccountDaily.Total) FROM CounterAccountDaily CounterAccountDaily").UniqueResult<int>();
            int weekly = Session.CreateQuery("SELECT SUM(CounterAccountWeekly.Total) FROM CounterAccountWeekly CounterAccountWeekly").UniqueResult<int>();
            int monthly = Session.CreateQuery("SELECT SUM(CounterAccountMonthly.Total) FROM CounterAccountMonthly CounterAccountMonthly").UniqueResult<int>();
            long yearly = Session.CreateQuery("SELECT SUM(CounterAccountYearly.Total) FROM CounterAccountYearly CounterAccountYearly").UniqueResult<long>();
            Assert.AreEqual(daily, weekly);
            Assert.AreEqual(weekly, monthly);
            Assert.AreEqual(monthly, yearly);
        }
    }
}
