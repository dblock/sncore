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
        public AccountLicenseTest()
            : base("AccountLicense")
        {

        }

        public override WebLicenseService.TransitAccountLicense GetTransitInstance()
        {
            WebLicenseService.TransitAccountLicense t_instance = new WebLicenseService.TransitAccountLicense();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.ImageUrl = GetNewUri();
            t_instance.LicenseUrl = GetNewUri();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetUserAccount().Id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetUserAccount().Id, options };
            return args;
        }

        [Test]
        public void GetAccountLicenseByAccountIdTest()
        {
            WebLicenseService.TransitAccountLicense t_instance = EndPoint.GetAccountLicenseByAccountId(
                GetAdminTicket(), GetUserAccount().Id);
            if (t_instance != null) Console.WriteLine(t_instance.Name);
        }
    }
}
