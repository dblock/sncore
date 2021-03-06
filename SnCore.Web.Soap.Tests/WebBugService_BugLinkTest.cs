using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugLinkTest : WebServiceTest<WebBugService.TransitBugLink, WebBugServiceNoCache>
    {
        private BugTest _bug1 = new BugTest();
        private int _bug1_id = 0;

        private BugTest _bug2 = new BugTest();
        private int _bug2_id = 0;

        public BugLinkTest()
            : base("BugLink")
        {
        }

        [SetUp]
        public override void SetUp()
        {
            _bug1.SetUp();
            _bug1_id = _bug1.Create(GetAdminTicket());

            _bug2.SetUp();
            _bug2_id = _bug2.Create(GetAdminTicket());

        }

        [TearDown]
        public override void TearDown()
        {
            _bug2.Delete(GetAdminTicket(), _bug2_id);
            _bug2.TearDown();

            _bug1.Delete(GetAdminTicket(), _bug1_id);
            _bug1.TearDown();
        }

        public override WebBugService.TransitBugLink GetTransitInstance()
        {
            WebBugService.TransitBugLink t_instance = new WebBugService.TransitBugLink();
            t_instance.BugId = _bug1_id;
            t_instance.RelatedBugId = _bug2_id;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _bug1_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _bug1_id, options };
            return args;
        }
    }
}
