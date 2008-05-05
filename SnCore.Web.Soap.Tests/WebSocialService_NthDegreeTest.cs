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

        [Test, ExpectedException(typeof(SoapException))]
        public void GetNDegreeCountByIdZeroTest()
        {
            EndPoint.GetNDegreeCountById(_user.ticket, _user.id, 0);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void GetNDegreeCountByIdMinusOneTest()
        {
            EndPoint.GetNDegreeCountById(_user.ticket, _user.id, -1);
        }

        [Test]
        public void GetNDegreeCountByIdTest()
        {
            Nullable<int> previous = new Nullable<int>();
            for (int i = 1; i < 5; i++)
            {
                int count = EndPoint.GetNDegreeCountById(_user.ticket, GetAdminAccount().Id, i);
                Console.WriteLine("{0} degree count: {1}", i, count);

                if (previous.HasValue)
                    Assert.IsTrue(previous.Value <= count);

                previous = count;
            }
        }

        [Test]
        public void GetFirstDegreeCountByIdTest()
        {
            int degree_count = EndPoint.GetNDegreeCountById(_user.ticket, GetAdminAccount().Id, 1);
            int direct_count = EndPoint.GetFirstDegreeCountById(_user.ticket, GetAdminAccount().Id);
            Assert.AreEqual(degree_count, direct_count);
        }
    }
}
