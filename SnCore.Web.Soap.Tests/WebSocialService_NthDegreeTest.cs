using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSocialServiceTests
{
    [TestFixture]
    public class NthDegreeTest : WebServiceBaseTest<WebSocialServiceNoCache>
    {
        [Test, ExpectedException(typeof(SoapException))]
        public void GetNDegreeCountByIdZeroTest()
        {
            EndPoint.GetNDegreeCountById(GetUserTicket(), GetUserAccount().Id, 0);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void GetNDegreeCountByIdMinusOneTest()
        {
            EndPoint.GetNDegreeCountById(GetUserTicket(), GetUserAccount().Id, -1);
        }

        [Test]
        public void GetNDegreeCountByIdTest()
        {
            Nullable<int> previous = new Nullable<int>();
            for (int i = 1; i < 5; i++)
            {
                int count = EndPoint.GetNDegreeCountById(GetUserTicket(), GetAdminAccount().Id, i);
                Console.WriteLine("{0} degree count: {1}", i, count);

                if (previous.HasValue)
                    Assert.IsTrue(previous.Value <= count);

                previous = count;
            }
        }

        [Test]
        public void GetFirstDegreeCountByIdTest()
        {
            int degree_count = EndPoint.GetNDegreeCountById(GetUserTicket(), GetAdminAccount().Id, 1);
            int direct_count = EndPoint.GetFirstDegreeCountById(GetUserTicket(), GetAdminAccount().Id);
            Assert.AreEqual(degree_count, direct_count);
        }
    }
}
