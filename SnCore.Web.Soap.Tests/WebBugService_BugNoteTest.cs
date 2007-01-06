using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugService_BugNoteTest : WebServiceTest<WebBugService.TransitBugNote>
    {
        private WebBugService_BugTest _bug = new WebBugService_BugTest();
        private int _bug_id = 0;

        public WebBugService_BugNoteTest()
            : base("BugNote", new WebBugService.WebBugService())
        {
        }

        [SetUp]
        public void SetUp()
        {
            _bug.SetUp();
            _bug_id = _bug.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _bug.Delete(GetAdminTicket(), _bug_id);
            _bug.TearDown();
        }

        public override WebBugService.TransitBugNote GetTransitInstance()
        {
            WebBugService.TransitBugNote t_instance = new WebBugService.TransitBugNote();
            t_instance.BugId = _bug_id;
            t_instance.Details = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _bug_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _bug_id, options };
            return args;
        }
    }
}
