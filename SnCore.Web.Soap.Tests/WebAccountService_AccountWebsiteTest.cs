using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountWebsiteTest : AccountBaseTest<WebAccountService.TransitAccountWebsite>
    {
        public AccountWebsiteTest()
            : base("AccountWebsite")
        {

        }

        public override WebAccountService.TransitAccountWebsite GetTransitInstance()
        {
            WebAccountService.TransitAccountWebsite t_instance = new WebAccountService.TransitAccountWebsite();
            t_instance.AccountId = _account_id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://www.{0}.com", Guid.NewGuid());
            return t_instance;
        }
    }
}
