using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web bug services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebBugService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebBugService : WebService
    {
        public WebBugService()
        {

        }

        #region Priority

        /// <summary>
        /// Create or update a bug priority.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="priority">transit bug priority</param>
        [WebMethod(Description = "Create or update a bug priority.")]
        public int CreateOrUpdateBugPriority(string ticket, TransitBugPriority priority)
        {
            return WebServiceImpl<TransitBugPriority, ManagedBugPriority, BugPriority>.CreateOrUpdate(
                ticket, priority);
        }

        /// <summary>
        /// Get a bug priority.
        /// </summary>
        /// <returns>transit bug priority</returns>
        [WebMethod(Description = "Get a bug priority.")]
        public TransitBugPriority GetBugPriorityById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugPriority, ManagedBugPriority, BugPriority>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug priorities.
        /// </summary>
        /// <returns>list of transit bug priorities</returns>
        [WebMethod(Description = "Get all bug priorities.")]
        public List<TransitBugPriority> GetBugPriorities(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugPriority, ManagedBugPriority, BugPriority>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all bug priorities count.
        /// </summary>
        /// <returns>number of bug priorities</returns>
        [WebMethod(Description = "Get all bug priorities count.")]
        public int GetBugPrioritiesCount(string ticket)
        {
            return WebServiceImpl<TransitBugPriority, ManagedBugPriority, BugPriority>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bug priority.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">priority id</param>
        [WebMethod(Description = "Delete a bug priority.")]
        public void DeleteBugPriority(string ticket, int id)
        {
            WebServiceImpl<TransitBugPriority, ManagedBugPriority, BugPriority>.Delete(
                ticket, id);
        }

        #endregion

        #region Severity

        /// <summary>
        /// Create or update a bug severity.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="severity">transit bug severity</param>
        [WebMethod(Description = "Create or update a bug severity.")]
        public int CreateOrUpdateBugSeverity(string ticket, TransitBugSeverity severity)
        {
            return WebServiceImpl<TransitBugSeverity, ManagedBugSeverity, BugSeverity>.CreateOrUpdate(
                ticket, severity);
        }

        /// <summary>
        /// Get a bug severity.
        /// </summary>
        /// <returns>transit bug severity</returns>
        [WebMethod(Description = "Get a bug severity.")]
        public TransitBugSeverity GetBugSeverityById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugSeverity, ManagedBugSeverity, BugSeverity>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug severities.
        /// </summary>
        /// <returns>list of transit bug severities</returns>
        [WebMethod(Description = "Get all bug severities.")]
        public List<TransitBugSeverity> GetBugSeverities(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugSeverity, ManagedBugSeverity, BugSeverity>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all bug severities count.
        /// </summary>
        /// <returns>number of bug severities</returns>
        [WebMethod(Description = "Get all bug severities count.")]
        public int GetBugSeveritiesCount(string ticket)
        {
            return WebServiceImpl<TransitBugSeverity, ManagedBugSeverity, BugSeverity>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bug severity.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">severity id</param>
        [WebMethod(Description = "Delete a bug severity.")]
        public void DeleteBugSeverity(string ticket, int id)
        {
            WebServiceImpl<TransitBugSeverity, ManagedBugSeverity, BugSeverity>.Delete(
                ticket, id);
        }

        #endregion

        #region Resolution

        /// <summary>
        /// Create or update a bug resolution.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="resolution">transit bug resolution</param>
        [WebMethod(Description = "Create or update a bug resolution.")]
        public int CreateOrUpdateBugResolution(string ticket, TransitBugResolution resolution)
        {
            return WebServiceImpl<TransitBugResolution, ManagedBugResolution, BugResolution>.CreateOrUpdate(
                ticket, resolution);
        }

        /// <summary>
        /// Get a bug resolution.
        /// </summary>
        /// <returns>transit bug resolution</returns>
        [WebMethod(Description = "Get a bug resolution.")]
        public TransitBugResolution GetBugResolutionById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugResolution, ManagedBugResolution, BugResolution>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug resolutions.
        /// </summary>
        /// <returns>list of transit bug resolutions</returns>
        [WebMethod(Description = "Get all bug resolutions.")]
        public List<TransitBugResolution> GetBugResolutions(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugResolution, ManagedBugResolution, BugResolution>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all bug resolutions count.
        /// </summary>
        /// <returns>number of bug resolutions</returns>
        [WebMethod(Description = "Get all bug resolutions count.")]
        public int GetBugResolutionsCount(string ticket)
        {
            return WebServiceImpl<TransitBugResolution, ManagedBugResolution, BugResolution>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bug resolution.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">resolution id</param>
        [WebMethod(Description = "Delete a bug resolution.")]
        public void DeleteBugResolution(string ticket, int id)
        {
            WebServiceImpl<TransitBugResolution, ManagedBugResolution, BugResolution>.Delete(
                ticket, id);
        }

        #endregion        

        #region Status

        /// <summary>
        /// Create or update a bug status.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="status">transit bug status</param>
        [WebMethod(Description = "Create or update a bug status.")]
        public int CreateOrUpdateBugStatus(string ticket, TransitBugStatus status)
        {
            return WebServiceImpl<TransitBugStatus, ManagedBugStatus, BugStatu>.CreateOrUpdate(
                ticket, status);
        }

        /// <summary>
        /// Get a bug status.
        /// </summary>
        /// <returns>transit bug status</returns>
        [WebMethod(Description = "Get a bug status.")]
        public TransitBugStatus GetBugStatusById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugStatus, ManagedBugStatus, BugStatu>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug statuses.
        /// </summary>
        /// <returns>list of transit bug statuses</returns>
        [WebMethod(Description = "Get all bug statuses.")]
        public List<TransitBugStatus> GetBugStatuses(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugStatus, ManagedBugStatus, BugStatu>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all bug statuses count.
        /// </summary>
        /// <returns>number of bug statuses</returns>
        [WebMethod(Description = "Get all bug statuses count.")]
        public int GetBugStatusesCount(string ticket)
        {
            return WebServiceImpl<TransitBugStatus, ManagedBugStatus, BugStatu>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bug status.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">status id</param>
        [WebMethod(Description = "Delete a bug status.")]
        public void DeleteBugStatus(string ticket, int id)
        {
            WebServiceImpl<TransitBugStatus, ManagedBugStatus, BugStatu>.Delete(
                ticket, id);
        }

        #endregion

        #region Type

        /// <summary>
        /// Create or update a bug type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit bug type</param>
        [WebMethod(Description = "Create or update a bug type.")]
        public int CreateOrUpdateBugType(string ticket, TransitBugType type)
        {
            return WebServiceImpl<TransitBugType, ManagedBugType, BugType>.CreateOrUpdate(
                ticket, type);
        }

        /// <summary>
        /// Get a bug type.
        /// </summary>
        /// <returns>transit bug type</returns>
        [WebMethod(Description = "Get a bug type.")]
        public TransitBugType GetBugTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugType, ManagedBugType, BugType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug types count.
        /// </summary>
        /// <returns>number of bug types</returns>
        [WebMethod(Description = "Get all bug types count.")]
        public int GetBugTypesCount(string ticket)
        {
            return WebServiceImpl<TransitBugType, ManagedBugType, BugType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all bug types.
        /// </summary>
        /// <returns>list of transit bug types</returns>
        [WebMethod(Description = "Get all bug types.")]
        public List<TransitBugType> GetBugTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugType, ManagedBugType, BugType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Delete a bug type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">type id</param>
        [WebMethod(Description = "Delete a bug type.")]
        public void DeleteBugType(string ticket, int id)
        {
            WebServiceImpl<TransitBugType, ManagedBugType, BugType>.Delete(
                ticket, id);
        }

        #endregion

        #region Bug Project

        /// <summary>
        /// Create or update a bug project.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="project">transit bug project</param>
        [WebMethod(Description = "Create or update a bug project.")]
        public int CreateOrUpdateBugProject(string ticket, TransitBugProject project)
        {
            return WebServiceImpl<TransitBugProject, ManagedBugProject, BugProject>.CreateOrUpdate(
                ticket, project);
        }

        /// <summary>
        /// Get a bug project.
        /// </summary>
        /// <returns>transit bug project</returns>
        [WebMethod(Description = "Get a bug project.")]
        public TransitBugProject GetBugProjectById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugProject, ManagedBugProject, BugProject>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bug projects.
        /// </summary>
        /// <returns>list of transit bug projects</returns>
        [WebMethod(Description = "Get all bug projects.")]
        public List<TransitBugProject> GetBugProjects(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBugProject, ManagedBugProject, BugProject>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all bug projects count.
        /// </summary>
        /// <returns>number of bug projects</returns>
        [WebMethod(Description = "Get all bug projects count.")]
        public int GetBugProjectsCount(string ticket)
        {
            return WebServiceImpl<TransitBugProject, ManagedBugProject, BugProject>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bug project.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">project id</param>
        [WebMethod(Description = "Delete a bug project.")]
        public void DeleteBugProject(string ticket, int id)
        {
            WebServiceImpl<TransitBugProject, ManagedBugProject, BugProject>.Delete(
                ticket, id);
        }

        #endregion

        #region Bug

        /// <summary>
        /// Get all bugs count.
        /// </summary>
        [WebMethod(Description = "Get all bugs.")]
        public int GetBugsWithOptionsCount(string ticket, TransitBugQueryOptions qopt)
        {
            return GetBugsWithOptions(ticket, qopt, null).Count;
        }

        /// <summary>
        /// Get all bugs.
        /// </summary>
        [WebMethod(Description = "Get all bugs.")]
        public List<TransitBug> GetBugsWithOptions(string ticket, TransitBugQueryOptions qopt, ServiceQueryOptions options)
        {
            string query = null;
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                query = qopt.GetQuery(session);
            }

            return WebServiceImpl<TransitBug, ManagedBug, Bug>.GetList(
                ticket, options, query, "Bug");
        }

        /// <summary>
        /// Create or update a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="project">transit bug</param>
        [WebMethod(Description = "Create or update a bug.")]
        public int CreateOrUpdateBug(string ticket, TransitBug bug)
        {
            return WebServiceImpl<TransitBug, ManagedBug, Bug>.CreateOrUpdate(
                ticket, bug);
        }

        /// <summary>
        /// Get a bug.
        /// </summary>
        /// <returns>transit bug</returns>
        [WebMethod(Description = "Get a bug.")]
        public TransitBug GetBugById(string ticket, int id)
        {
            return WebServiceImpl<TransitBug, ManagedBug, Bug>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all bugs count.
        /// </summary>
        /// <returns>number of bugs</returns>
        [WebMethod(Description = "Get all bugs statuses count.")]
        public int GetBugsCount(string ticket, int project_id)
        {
            ICriterion[] expressions = { Expression.Eq("Project.Id", project_id) };
            return WebServiceImpl<TransitBug, ManagedBug, Bug>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all bugs.
        /// </summary>
        /// <returns>list of transit bugs</returns>
        [WebMethod(Description = "Get all bugs.")]
        public List<TransitBug> GetBugs(string ticket, int project_id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Project.Id", project_id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitBug, ManagedBug, Bug>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Delete a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">bug id</param>
        [WebMethod(Description = "Delete a bug.")]
        public void DeleteBug(string ticket, int id)
        {
            WebServiceImpl<TransitBug, ManagedBug, Bug>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Resolve a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="note">note</param>
        /// <param name="bugid">bug id</param>
        [WebMethod(Description = "Resolve a bug.")]
        public void ResolveBug(string ticket, int bugid, string resolution, string note)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Resolve(resolution, sec);

                if (note.Length > 0)
                {
                    TransitBugNote t_note = new TransitBugNote();
                    t_note.BugId = bug.Id;
                    t_note.Details = note;
                    ManagedBugNote m_note = new ManagedBugNote(session);
                    m_note.CreateOrUpdate(t_note, sec);
                }

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Close a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="bugid">bug id</param>
        [WebMethod(Description = "Close a bug.")]
        public void CloseBug(string ticket, int bugid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Close(sec);

                ManagedBugNote note = new ManagedBugNote(session);
                TransitBugNote t_note = new TransitBugNote();
                t_note.BugId = bug.Id;
                t_note.Details = "Bug closed.";
                note.CreateOrUpdate(t_note, sec);

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Reopen a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="bugid">bug id</param>
        [WebMethod(Description = "Reopen a bug.")]
        public void ReopenBug(string ticket, int bugid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Reopen(sec);

                ManagedBugNote note = new ManagedBugNote(session);
                TransitBugNote t_note = new TransitBugNote();
                t_note.BugId = bug.Id;
                t_note.Details = "Bug reopened.";
                note.CreateOrUpdate(t_note, sec);

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Bug Note

        /// <summary>
        /// Get all bug notes.
        /// </summary>
        /// <param name="bug_id">bug id</param>
        /// <returns>list of transit bug notes</returns>
        [WebMethod(Description = "Get all bug notes.")]
        public List<TransitBugNote> GetBugNotes(string ticket, int bug_id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Bug.Id", bug_id) };
            Order[] orders = { Order.Desc("Modified") };
            return WebServiceImpl<TransitBugNote, ManagedBugNote, BugNote>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get all bug notes count.
        /// </summary>
        /// <param name="bug_id">bug id</param>
        /// <returns>number of bug notes</returns>
        [WebMethod(Description = "Get all bug notes count.")]
        public int GetBugNotesCount(string ticket, int bug_id)
        {
            ICriterion[] expressions = { Expression.Eq("Bug.Id", bug_id) };
            return WebServiceImpl<TransitBugNote, ManagedBugNote, BugNote>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete a bug note.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">note id</param>
        [WebMethod(Description = "Delete a bug note.")]
        public void DeleteBugNote(string ticket, int id)
        {
            WebServiceImpl<TransitBugNote, ManagedBugNote, BugNote>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Create or update a bug note.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="">transit bug note</param>
        [WebMethod(Description = "Create or update a bug note.")]
        public int CreateOrUpdateBugNote(string ticket, TransitBugNote bugnote)
        {
            return WebServiceImpl<TransitBugNote, ManagedBugNote, BugNote>.CreateOrUpdate(
                ticket, bugnote);
        }

        /// <summary>
        /// Get a bug note.
        /// </summary>
        /// <returns>transit bug note</returns>
        [WebMethod(Description = "Get a bug note.")]
        public TransitBugNote GetBugNoteById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugNote, ManagedBugNote, BugNote>.GetById(
                ticket, id);
        }

        #endregion

        #region Bug Link

        /// <summary>
        /// Get all bug links.
        /// </summary>
        /// <param name="bug_id">bug id</param>
        /// <returns>list of transit bug links</returns>
        [WebMethod(Description = "Get all bug links.")]
        public List<TransitBugLink> GetBugLinks(string ticket, int bug_id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Bug.Id", bug_id) };
            return WebServiceImpl<TransitBugLink, ManagedBugLink, BugLink>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all bug links count.
        /// </summary>
        /// <param name="bug_id">bug id</param>
        /// <returns>number of bug links</returns>
        [WebMethod(Description = "Get all bug links count.")]
        public int GetBugLinksCount(string ticket, int bug_id)
        {
            ICriterion[] expressions = { Expression.Eq("Bug.Id", bug_id) };
            return WebServiceImpl<TransitBugLink, ManagedBugLink, BugLink>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete a bug link.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">link id</param>
        [WebMethod(Description = "Delete a bug link.")]
        public void DeleteBugLink(string ticket, int id)
        {
            WebServiceImpl<TransitBugLink, ManagedBugLink, BugLink>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Create or update a bug link.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="">transit bug link</param>
        [WebMethod(Description = "Create or update a bug link.")]
        public int CreateOrUpdateBugLink(string ticket, TransitBugLink buglink)
        {
            return WebServiceImpl<TransitBugLink, ManagedBugLink, BugLink>.CreateOrUpdate(
                ticket, buglink);
        }

        /// <summary>
        /// Get a bug link.
        /// </summary>
        /// <returns>transit bug link</returns>
        [WebMethod(Description = "Get a bug link.")]
        public TransitBugLink GetBugLinkById(string ticket, int id)
        {
            return WebServiceImpl<TransitBugLink, ManagedBugLink, BugLink>.GetById(
                ticket, id);
        }

        #endregion
    }
}