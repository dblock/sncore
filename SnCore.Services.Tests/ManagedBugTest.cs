using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using NHibernate;
using SnCore.Data.Tests;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugTest : ManagedCRUDTest<Bug, TransitBug, ManagedBug>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedBugPriorityTest _priority = new ManagedBugPriorityTest();
        private ManagedBugSeverityTest _severity = new ManagedBugSeverityTest();
        private ManagedBugResolutionTest _resolution = new ManagedBugResolutionTest();
        private ManagedBugStatusTest _status = new ManagedBugStatusTest();
        private ManagedBugTypeTest _type = new ManagedBugTypeTest();
        private ManagedBugProjectTest _project = new ManagedBugProjectTest();

        [SetUp]
        public override void SetUp()
        {
            _project.SetUp();
            _account.SetUp();
            _priority.SetUp();
            _severity.SetUp();
            _resolution.SetUp();
            _status.SetUp();
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _type.TearDown();
            _status.TearDown();
            _resolution.TearDown();
            _severity.TearDown();
            _priority.TearDown();
            _account.TearDown();
            _project.TearDown();
        }

        public override TransitBug GetTransitInstance()
        {
            TransitBug t_instance = new TransitBug();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Details = GetNewString();
            t_instance.Priority = _priority.Instance.Instance.Name;
            t_instance.Resolution = _resolution.Instance.Instance.Name;
            t_instance.Severity = _severity.Instance.Instance.Name;
            t_instance.Status = _status.Instance.Instance.Name;
            t_instance.Subject = GetNewString();
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.ProjectId = _project.Instance.Id;
            return t_instance;
        }

        public ManagedBugTest()
        {

        }

        [Test]
        public void CreateBug()
        {
            ManagedBugPriority priority = new ManagedBugPriority(Session);
            ManagedBugProject project = new ManagedBugProject(Session);
            ManagedBugResolution resolution = new ManagedBugResolution(Session);
            ManagedBugSeverity severity = new ManagedBugSeverity(Session);
            ManagedBugStatus status = new ManagedBugStatus(Session);
            ManagedBugType type = new ManagedBugType(Session);
            ManagedBug bug = new ManagedBug(Session);

            try
            {
                TransitBugPriority t_priority = new TransitBugPriority();
                t_priority.Name = GetNewString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = GetNewString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = GetNewString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = GetNewString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = GetNewString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = GetNewString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id;;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = GetNewString();
                t_bug.Type = t_type.Name;
                t_bug.Details = GetNewString();
                t_bug.Id = bug.CreateOrUpdate(t_bug, AdminSecurityContext);

                Session.Flush();

                // foodcandy bug #416 - Bugs: reported by blank in BugProjectBugsManage.aspx
                ManagedBug m_bug1 = new ManagedBug(Session, t_bug.Id);
                TransitBug t_instance1 = m_bug1.GetTransitInstance(AdminSecurityContext);
                Assert.AreEqual(t_instance1.AccountName, AdminSecurityContext.Account.Name);
                Assert.AreEqual(t_instance1.AccountId, AdminSecurityContext.Account.Id);
            }
            finally
            {
                bug.Delete(AdminSecurityContext);
                priority.Delete(AdminSecurityContext);
                resolution.Delete(AdminSecurityContext);
                severity.Delete(AdminSecurityContext);
                status.Delete(AdminSecurityContext);
                type.Delete(AdminSecurityContext);
                project.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void LinkBugs()
        {
            ManagedBugPriority priority = new ManagedBugPriority(Session);
            ManagedBugProject project = new ManagedBugProject(Session);
            ManagedBugResolution resolution = new ManagedBugResolution(Session);
            ManagedBugSeverity severity = new ManagedBugSeverity(Session);
            ManagedBugStatus status = new ManagedBugStatus(Session);
            ManagedBugType type = new ManagedBugType(Session);
            ManagedBug bug = new ManagedBug(Session);
            ManagedBug linkedbug = new ManagedBug(Session);

            try
            {
                TransitBugPriority t_priority = new TransitBugPriority();
                t_priority.Name = GetNewString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = GetNewString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = GetNewString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = GetNewString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = GetNewString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = GetNewString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = GetNewString();
                t_bug.Type = t_type.Name;
                t_bug.Details = GetNewString();
                
                bug.CreateOrUpdate(t_bug, AdminSecurityContext);
                linkedbug.CreateOrUpdate(t_bug, AdminSecurityContext);
                bug.LinkTo(linkedbug, AdminSecurityContext);
            }
            finally
            {
                bug.Delete(AdminSecurityContext);
                linkedbug.Delete(AdminSecurityContext);
                priority.Delete(AdminSecurityContext);
                resolution.Delete(AdminSecurityContext);
                severity.Delete(AdminSecurityContext);
                status.Delete(AdminSecurityContext);
                type.Delete(AdminSecurityContext);
                project.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void SearchBugs()
        {
            ManagedBugPriority priority = new ManagedBugPriority(Session);
            ManagedBugProject project = new ManagedBugProject(Session);
            ManagedBugResolution resolution = new ManagedBugResolution(Session);
            ManagedBugSeverity severity = new ManagedBugSeverity(Session);
            ManagedBugStatus status = new ManagedBugStatus(Session);
            ManagedBugType type = new ManagedBugType(Session);
            ManagedBug bug = new ManagedBug(Session);

            try
            {
                TransitBugPriority t_priority = new TransitBugPriority();
                t_priority.Name = GetNewString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = GetNewString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = GetNewString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = GetNewString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = GetNewString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = GetNewString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = "subject";
                t_bug.Type = t_type.Name;
                t_bug.Details = "details";
                bug.CreateOrUpdate(t_bug, AdminSecurityContext);

                {
                    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                    IQuery query = Session.CreateSQLQuery(qo.GetQuery(Session)).AddEntity("Bug", typeof(Bug));
                    IList list = query.List();
                    Assert.AreEqual(0, list.Count, "Wrong count on query w/o a project.");
                }

                //{
                //    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                //    qo.ProjectId = project.Id;
                //    IQuery query = qo.GetQuery(Session);
                //    IList list = query.List();
                //    Assert.AreEqual(1, list.Count, "Wrong count on query with a project id.");
                //}

                //{
                //    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                //    qo.ProjectId = project.Id;
                //    qo.SearchQuery = GetNewString();
                //    IQuery query = qo.GetQuery(Session);
                //    IList list = query.List();
                //    Assert.AreEqual(0, list.Count, "Wrong count on query with an unmatched query string.");
                //}

                //{
                //    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                //    qo.ProjectId = project.Id;
                //    qo.SearchQuery = "subject";
                //    IQuery query = qo.GetQuery(Session);
                //    IList list = query.List();
                //    Assert.AreEqual(1, list.Count, "Wrong count on query with a matched subject query string.");
                //}

                //{
                //    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                //    qo.ProjectId = project.Id;
                //    qo.SearchQuery = "details";
                //    IQuery query = qo.GetQuery(Session);
                //    IList list = query.List();
                //    Assert.AreEqual(1, list.Count, "Wrong count on query with a matched details query string.");
                //}
            }
            finally
            {
                bug.Delete(AdminSecurityContext);
                priority.Delete(AdminSecurityContext);
                resolution.Delete(AdminSecurityContext);
                severity.Delete(AdminSecurityContext);
                status.Delete(AdminSecurityContext);
                type.Delete(AdminSecurityContext);
                project.Delete(AdminSecurityContext);
            }
        }
    }
}
