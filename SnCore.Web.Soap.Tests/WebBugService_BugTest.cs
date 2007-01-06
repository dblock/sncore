using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugService_BugTest : WebServiceTest<WebBugService.TransitBug>
    {
        int _priority_id = 0;
        int _severity_id = 0;
        int _type_id = 0;
        int _status_id = 0;
        int _resolution_id = 0;
        int _resolution2_id = 0;
        int _project_id = 0;

        public WebBugService_BugTest()
            : base("Bug", new WebBugService.WebBugService())
        {
        }

        [SetUp]
        public void SetUp()
        {
            _priority_id = new WebBugService_BugPriorityTest().Create(GetAdminTicket());
            _severity_id = new WebBugService_BugSeverityTest().Create(GetAdminTicket());
            _type_id = new WebBugService_BugTypeTest().Create(GetAdminTicket());
            _status_id = new WebBugService_BugStatusTest().Create(GetAdminTicket());
            _resolution_id = new WebBugService_BugResolutionTest().Create(GetAdminTicket());
            _resolution2_id = new WebBugService_BugResolutionTest().Create(GetAdminTicket());
            _project_id = new WebBugService_BugProjectTest().Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            new WebBugService_BugProjectTest().Delete(GetAdminTicket(), _project_id);
            new WebBugService_BugPriorityTest().Delete(GetAdminTicket(), _priority_id);
            new WebBugService_BugSeverityTest().Delete(GetAdminTicket(), _severity_id);
            new WebBugService_BugTypeTest().Delete(GetAdminTicket(), _type_id);
            new WebBugService_BugStatusTest().Delete(GetAdminTicket(), _status_id);
            new WebBugService_BugResolutionTest().Delete(GetAdminTicket(), _resolution_id);
            new WebBugService_BugResolutionTest().Delete(GetAdminTicket(), _resolution2_id);
        }

        public override WebBugService.TransitBug GetTransitInstance()
        {
            WebBugService.TransitBug t_instance = new WebBugService.TransitBug();
            t_instance.Subject = Guid.NewGuid().ToString();
            t_instance.Details = Guid.NewGuid().ToString();
            t_instance.Priority = (string) new WebBugService_BugPriorityTest().GetInstancePropertyById(GetAdminTicket(), _priority_id, "Name");
            t_instance.Severity = (string) new WebBugService_BugSeverityTest().GetInstancePropertyById(GetAdminTicket(), _severity_id, "Name");
            t_instance.Type = (string) new WebBugService_BugTypeTest().GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.Status = (string) new WebBugService_BugStatusTest().GetInstancePropertyById(GetAdminTicket(), _status_id, "Name");
            t_instance.Resolution = (string) new WebBugService_BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution_id, "Name");
            t_instance.ProjectId = _project_id;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _project_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _project_id, options };
            return args;
        }

        [Test]
        public void TestResolve()
        {
            int id = Create(GetAdminTicket());
            string current_resolution = (string) GetInstancePropertyById(GetAdminTicket(), id, "Resolution");
            Console.WriteLine("Current resolution: {0}", current_resolution);
            string target_resolution = (string) new WebBugService_BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution2_id, "Name");
            WebBugService.WebBugService endpoint = (WebBugService.WebBugService) EndPoint;
            endpoint.ResolveBug(GetAdminTicket(), id, target_resolution, Guid.NewGuid().ToString());
            string new_resolution = (string) GetInstancePropertyById(GetAdminTicket(), id, "Resolution");
            Console.WriteLine("New resolution: {0}", new_resolution);
            Assert.AreNotEqual(current_resolution, new_resolution);
            Assert.AreEqual(new_resolution, target_resolution);
        }

        [Test]
        public void TestClose()
        {
            int id = Create(GetAdminTicket());
            string current_status = (string) GetInstancePropertyById(GetAdminTicket(), id, "Status");
            Console.WriteLine("Current status: {0}", current_status);
            WebBugService.WebBugService endpoint = (WebBugService.WebBugService)EndPoint;
            string target_resolution = (string)new WebBugService_BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution2_id, "Name");
            endpoint.ResolveBug(GetAdminTicket(), id, target_resolution, Guid.NewGuid().ToString());
            endpoint.CloseBug(GetAdminTicket(), id);
            string new_status = (string) GetInstancePropertyById(GetAdminTicket(), id, "Status");
            Console.WriteLine("New status: {0}", new_status);
            Assert.AreNotEqual(current_status, new_status);
            Assert.AreEqual(new_status, "Closed");
            Delete(GetAdminTicket(), id);
        }
    }
}
