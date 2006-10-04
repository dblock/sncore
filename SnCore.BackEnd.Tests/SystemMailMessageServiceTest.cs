using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemMailMessageServicesTest : NHibernateTest
    {
        private SystemMailMessageService service = null;

        [SetUp]
        public override void SetUp()
        {
            service = new SystemMailMessageService();
            base.SetUp();
        }

        [Test]
        public void TestEmailQueue()
        {
            service.RunEmailQueue(Session);
        }

        [Test]
        public void TestMarketingCampaign()
        {
            service.RunMarketingCampaign(Session);
        }
    }
}
