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
    public class ManagedBugTest : ManagedServiceTest
    {
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
                t_priority.Name = Guid.NewGuid().ToString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = Guid.NewGuid().ToString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = Guid.NewGuid().ToString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = Guid.NewGuid().ToString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = Guid.NewGuid().ToString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = Guid.NewGuid().ToString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id;;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = Guid.NewGuid().ToString();
                t_bug.Type = t_type.Name;
                t_bug.Details = Guid.NewGuid().ToString();
                bug.CreateOrUpdate(t_bug, AdminSecurityContext);
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
                t_priority.Name = Guid.NewGuid().ToString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = Guid.NewGuid().ToString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = Guid.NewGuid().ToString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = Guid.NewGuid().ToString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = Guid.NewGuid().ToString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = Guid.NewGuid().ToString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = Guid.NewGuid().ToString();
                t_bug.Type = t_type.Name;
                t_bug.Details = Guid.NewGuid().ToString();
                
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
                t_priority.Name = Guid.NewGuid().ToString();
                priority.CreateOrUpdate(t_priority, AdminSecurityContext);

                TransitBugProject t_project = new TransitBugProject();
                t_project.Name = Guid.NewGuid().ToString();
                project.CreateOrUpdate(t_project, AdminSecurityContext);

                TransitBugResolution t_resolution = new TransitBugResolution();
                t_resolution.Name = Guid.NewGuid().ToString();
                resolution.CreateOrUpdate(t_resolution, AdminSecurityContext);

                TransitBugSeverity t_severity = new TransitBugSeverity();
                t_severity.Name = Guid.NewGuid().ToString();
                severity.CreateOrUpdate(t_severity, AdminSecurityContext);

                TransitBugStatus t_status = new TransitBugStatus();
                t_status.Name = Guid.NewGuid().ToString();
                status.CreateOrUpdate(t_status, AdminSecurityContext);

                TransitBugType t_type = new TransitBugType();
                t_type.Name = Guid.NewGuid().ToString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitBug t_bug = new TransitBug();
                t_bug.Priority = t_priority.Name;
                t_bug.ProjectId = project.Id; ;
                t_bug.Resolution = t_resolution.Name;
                t_bug.Severity = t_severity.Name;
                t_bug.Status = t_status.Name;
                t_bug.Subject = "subject";
                t_bug.Type = t_type.Name;
                t_bug.Details = "details";
                bug.CreateOrUpdate(t_bug, AdminSecurityContext);

                {
                    TransitBugQueryOptions qo = new TransitBugQueryOptions();
                    IQuery query = qo.GetQuery(Session);
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
                //    qo.SearchQuery = Guid.NewGuid().ToString();
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
