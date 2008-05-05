using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AdminTest : WebServiceBaseTest<WebAccountServiceNoCache>
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
        public void ImpersonateAsAdminTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            string ticket = EndPoint.Impersonate(GetAdminTicket(), user.id);
            Assert.IsNotEmpty(ticket);
            EndPoint.DeleteAccount(GetAdminTicket(), user.id);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void ImpersonateAsUserTest()
        {
            string ticket = EndPoint.Impersonate(_user.ticket, GetAdminAccount().Id);
            Assert.IsEmpty(ticket);
        }

        [Test]
        public void PromoteAdministratorTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            WebAccountService.TransitAccount t_user = EndPoint.GetAccountById(GetAdminTicket(), user.id);
            Assert.IsFalse(t_user.IsAdministrator);
            EndPoint.PromoteAdministrator(GetAdminTicket(), t_user.Id);
            WebAccountService.TransitAccount t_user_admin = EndPoint.GetAccountById(GetAdminTicket(), user.id);
            Assert.IsTrue(t_user_admin.IsAdministrator);
            EndPoint.DemoteAdministrator(GetAdminTicket(), t_user.Id);
            WebAccountService.TransitAccount t_user_nonadmin = EndPoint.GetAccountById(GetAdminTicket(), user.id);
            Assert.IsFalse(t_user_nonadmin.IsAdministrator);
            EndPoint.DeleteAccount(GetAdminTicket(), user.id);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void DemoteDemotedAdministratorTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            EndPoint.DemoteAdministrator(GetAdminTicket(), user.id);
            EndPoint.DeleteAccount(GetAdminTicket(), user.id);
        }
    }
}
