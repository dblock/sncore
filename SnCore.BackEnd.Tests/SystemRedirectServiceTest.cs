using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemRedirectServicesTest : NHibernateTest
    {
        private SystemRedirectService service = null;

        [SetUp]
        public override void SetUp()
        {
            service = new SystemRedirectService();
            base.SetUp();
        }

        [Test]
        public void TestGenerateRedirectIni()
        {
            service.RunGenerateRedirectIni(Session);
        }
    }
}
