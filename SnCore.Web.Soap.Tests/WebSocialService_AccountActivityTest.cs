using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebSocialServiceTests
{
    [TestFixture]
    public class AccountActivityTest : WebServiceBaseTest<WebSocialServiceNoCache>
    {
        private UserInfo _user = null;

        [SetUp]
        public void SetUp()
        {
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteUser(_user.id);
        }

        [Test]
        public void GetNewAccountsTest()
        {
            WebSocialService.TransitAccount[] accounts = EndPoint.GetNewAccounts(
                _user.ticket, (WebSocialService.ServiceQueryOptions)GetServiceQueryOptions(0, 10));
            Console.WriteLine("New instances: {0}", accounts.Length);
            Nullable<DateTime> previous = new Nullable<DateTime>();
            foreach (WebSocialService.TransitAccount account in accounts)
            {
                Console.WriteLine(account.Created);
                if (previous.HasValue)
                    Assert.IsTrue(previous.Value >= account.Created);
                previous = account.Created;
            }
        }

        [Test]
        public void GetActiveAccountsTest()
        {
            WebSocialService.TransitAccount[] accounts = EndPoint.GetActiveAccounts(
                _user.ticket, (WebSocialService.ServiceQueryOptions)GetServiceQueryOptions(0, 10));
            Console.WriteLine("Active instances: {0}", accounts.Length);
            Nullable<DateTime> previous = new Nullable<DateTime>();
            foreach (WebSocialService.TransitAccount account in accounts)
            {
                Console.WriteLine(account.LastLogin);
                if (previous.HasValue)
                    Assert.IsTrue(previous.Value >= account.LastLogin);
                previous = account.LastLogin;
            }
        }

        [Test]
        public void GetAccountActivityTest()
        {
            WebSocialService.AccountActivityQueryOptions options = new WebSocialService.AccountActivityQueryOptions();
            options.BloggersOnly = false;
            options.PicturesOnly = false;
            int count = EndPoint.GetAccountActivityCount(_user.ticket, options);
            Console.WriteLine("Activity count: {0}", count);
            WebSocialService.TransitAccountActivity[] activity = EndPoint.GetAccountActivity(
                _user.ticket, options, (WebSocialService.ServiceQueryOptions) GetServiceQueryOptions(0, 10));
            Console.WriteLine("Activity instances: {0}", activity.Length);
        }

        [Test]
        public void GetFriendsAccountActivityTest()
        {
            int count = EndPoint.GetFriendsAccountActivityCount(_user.ticket, GetAdminAccount().Id);
            Console.WriteLine("Friends activity count: {0}", count);
            WebSocialService.TransitAccountActivity[] activity = EndPoint.GetFriendsAccountActivity(
                _user.ticket, GetAdminAccount().Id, (WebSocialService.ServiceQueryOptions)GetServiceQueryOptions(0, 10));
            Console.WriteLine("Activity instances: {0}", activity.Length);
        }
    }
}
