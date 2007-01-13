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
        [Test]
        public void ImpersonateAsAdminTest()
        {
            string ticket = EndPoint.Impersonate(GetAdminTicket(), GetUserAccount().Id);
            Assert.IsNotEmpty(ticket);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void ImpersonateAsUserTest()
        {
            string ticket = EndPoint.Impersonate(GetUserTicket(), GetAdminAccount().Id);
            Assert.IsEmpty(ticket);
        }

        [Test]
        public void PromoteAdministratorTest()
        {
            WebAccountService.TransitAccount t_user = EndPoint.GetAccountById(GetAdminTicket(), GetUserAccount().Id);
            Assert.IsFalse(t_user.IsAdministrator);
            EndPoint.PromoteAdministrator(GetAdminTicket(), t_user.Id);
            WebAccountService.TransitAccount t_user_admin = EndPoint.GetAccountById(GetAdminTicket(), GetUserAccount().Id);
            Assert.IsTrue(t_user_admin.IsAdministrator);
            EndPoint.DemoteAdministrator(GetAdminTicket(), t_user.Id);
            WebAccountService.TransitAccount t_user_nonadmin = EndPoint.GetAccountById(GetAdminTicket(), GetUserAccount().Id);
            Assert.IsFalse(t_user_nonadmin.IsAdministrator);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void DemoteDemotedAdministratorTest()
        {
            EndPoint.DemoteAdministrator(GetAdminTicket(), GetUserAccount().Id);
        }
    }
}
