using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLicenseServiceTests
{
    [TestFixture]
    public class AccountLicenseTest : WebServiceTest<WebLicenseService.TransitAccountLicense, WebLicenseServiceNoCache>
    {
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
        }

        public AccountLicenseTest()
            : base("AccountLicense")
        {

        }

        public override WebLicenseService.TransitAccountLicense GetTransitInstance()
        {
            WebLicenseService.TransitAccountLicense t_instance = new WebLicenseService.TransitAccountLicense();
            t_instance.AccountId = _user.id;
            t_instance.ImageUrl = GetNewUri();
            t_instance.LicenseUrl = GetNewUri();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _user.id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _user.id, options };
            return args;
        }

        [Test]
        public void GetAccountLicenseByAccountIdTest()
        {
            WebLicenseService.TransitAccountLicense t_instance = EndPoint.GetAccountLicenseByAccountId(
                GetAdminTicket(), _user.id);
            if (t_instance != null) Console.WriteLine(t_instance.Name);
        }
    }
}
