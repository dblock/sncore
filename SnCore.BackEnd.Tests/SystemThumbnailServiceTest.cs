using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;
using SnCore.Services;
using SnCore.Tools.Web;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemThumbnailServicesTest : NHibernateTest
    {
        private SystemThumbnailService service = null;

        [SetUp]
        public override void SetUp()
        {
            service = new SystemThumbnailService();
            base.SetUp();
        }

        [Test]
        public void TestThumbnail()
        {
            service.RunThumbnail(Session, ManagedAccount.GetAdminSecurityContext(Session));
        }
    }
}
