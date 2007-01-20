using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

namespace SnCore.Web.Soap.Tests.WebBugServiceTests
{
    [TestFixture]
    public class BugTest : WebServiceTest<WebBugService.TransitBug, WebBugServiceNoCache>
    {
        int _priority_id = 0;
        int _severity_id = 0;
        int _type_id = 0;
        int _status_id = 0;
        int _resolution_id = 0;
        int _resolution2_id = 0;
        int _project_id = 0;

        public BugTest()
            : base("Bug")
        {
        }

        [SetUp]
        public override void SetUp()
        {
            _priority_id = new BugPriorityTest().Create(GetAdminTicket());
            _severity_id = new BugSeverityTest().Create(GetAdminTicket());
            _type_id = new BugTypeTest().Create(GetAdminTicket());
            _status_id = new BugStatusTest().Create(GetAdminTicket());
            _resolution_id = new BugResolutionTest().Create(GetAdminTicket());
            _resolution2_id = new BugResolutionTest().Create(GetAdminTicket());
            _project_id = new BugProjectTest().Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            new BugProjectTest().Delete(GetAdminTicket(), _project_id);
            new BugPriorityTest().Delete(GetAdminTicket(), _priority_id);
            new BugSeverityTest().Delete(GetAdminTicket(), _severity_id);
            new BugTypeTest().Delete(GetAdminTicket(), _type_id);
            new BugStatusTest().Delete(GetAdminTicket(), _status_id);
            new BugResolutionTest().Delete(GetAdminTicket(), _resolution_id);
            new BugResolutionTest().Delete(GetAdminTicket(), _resolution2_id);
        }

        public override WebBugService.TransitBug GetTransitInstance()
        {
            WebBugService.TransitBug t_instance = new WebBugService.TransitBug();
            t_instance.Subject = Guid.NewGuid().ToString();
            t_instance.Details = Guid.NewGuid().ToString();
            t_instance.Priority = (string) new BugPriorityTest().GetInstancePropertyById(GetAdminTicket(), _priority_id, "Name");
            t_instance.Severity = (string) new BugSeverityTest().GetInstancePropertyById(GetAdminTicket(), _severity_id, "Name");
            t_instance.Type = (string) new BugTypeTest().GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.Status = (string) new BugStatusTest().GetInstancePropertyById(GetAdminTicket(), _status_id, "Name");
            t_instance.Resolution = (string) new BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution_id, "Name");
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
            string target_resolution = (string) new BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution2_id, "Name");
            EndPoint.ResolveBug(GetAdminTicket(), id, target_resolution, Guid.NewGuid().ToString());
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
            string target_resolution = (string)new BugResolutionTest().GetInstancePropertyById(GetAdminTicket(), _resolution2_id, "Name");
            EndPoint.ResolveBug(GetAdminTicket(), id, target_resolution, Guid.NewGuid().ToString());
            EndPoint.CloseBug(GetAdminTicket(), id);
            string new_status = (string) GetInstancePropertyById(GetAdminTicket(), id, "Status");
            Console.WriteLine("New status: {0}", new_status);
            Assert.AreNotEqual(current_status, new_status);
            Assert.AreEqual(new_status, "Closed");
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void TestAsUser()
        {
            string ticket = GetUserTicket();
            int count1 = GetCount(ticket);
            int id1 = Create(ticket);
            int count2 = GetCount(ticket);
            Assert.IsTrue(count2 >= 0 && count1 + 1 == count2);
            int id2 = (int)GetInstancePropertyById(ticket, id1, "Id");
            Assert.AreEqual(id1, id2);
            int count3 = GetMany(ticket, null);
            Assert.IsTrue(count2 == count3);
            int count4 = GetMany(ticket, GetServiceQueryOptions(0, 0));
            Assert.IsTrue(count2 == count4);
            const int page_size = 10;
            int count5 = GetMany(ticket, GetServiceQueryOptions(0, page_size));
            Assert.IsTrue(count5 >= 1 && count5 <= page_size);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestDeleteAsUser()
        {
            string ticket = GetUserTicket();
            int id = Create(ticket);
            Delete(ticket, id);
        }

        [Test]
        public void TestAddNoteAsUser()
        {
            int id = Create(GetAdminTicket());

            WebBugService.TransitBugNote t_instance = new WebBugService.TransitBugNote();
            t_instance.BugId = id;
            t_instance.Details = Guid.NewGuid().ToString();
            int note_id = EndPoint.CreateOrUpdateBugNote(GetUserTicket(), t_instance);
            Console.WriteLine("Created BugNote: {0}", note_id);
            Assert.IsTrue(note_id > 0);

            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetBugsWithOptionsTest()
        {
            WebBugService.TransitBug t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            Thread.Sleep(2000); // give time to reindex
            WebBugService.TransitBugQueryOptions options = new WebBugService.TransitBugQueryOptions();
            options.Closed = options.Open = options.Resolved = true;
            options.ProjectId = _project_id;
            options.SearchQuery = t_instance.Subject;
            int count = EndPoint.GetBugsWithOptionsCount(GetAdminTicket(), options);
            Console.WriteLine("Count: {0}", count);
            WebBugService.TransitBug[] bugs = EndPoint.GetBugsWithOptions(GetAdminTicket(), options, null);
            Console.WriteLine("Length: {0}", bugs.Length);
            Assert.AreEqual(count, bugs.Length);
            Assert.IsTrue(new TransitServiceCollection<WebBugService.TransitBug>(bugs).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
        }
    }
}
