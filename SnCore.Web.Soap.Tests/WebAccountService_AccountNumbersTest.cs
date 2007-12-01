using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebLocationServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountNumbersTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        public AccountNumbersTest()
        {

        }

        [Test]
        public void GetAccountNumbersTest()
        {
            WebAccountService.TransitAccountNumbers t_instance = EndPoint.GetAccountNumbersByAccountId(
                GetUserTicket(), GetUserAccount().Id);
            Console.WriteLine("{0}/{1}/{2}", t_instance.FirstDegreeCount, t_instance.SecondDegreeCount, t_instance.AllCount);
            Assert.IsTrue(t_instance.FirstDegreeCount <= t_instance.SecondDegreeCount);
            Assert.IsTrue(t_instance.SecondDegreeCount <= t_instance.AllCount);
        }
    }
}
