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
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;

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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugPriority m_priority = new ManagedBugPriority(session);
                m_priority.CreateOrUpdate(priority);
                SnCore.Data.Hibernate.Session.Flush();
                return m_priority.Id;
            }
        }

        /// <summary>
        /// Get a bug priority.
        /// </summary>
        /// <returns>transit bug priority</returns>
        [WebMethod(Description = "Get a bug priority.")]
        public TransitBugPriority GetBugPriorityById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugPriority result = new ManagedBugPriority(session, id).TransitBugPriority;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bug priorities.
        /// </summary>
        /// <returns>list of transit bug priorities</returns>
        [WebMethod(Description = "Get all bug priorities.")]
        public List<TransitBugPriority> GetBugPriorities()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList priorities = session.CreateCriteria(typeof(BugPriority)).List();
                List<TransitBugPriority> result = new List<TransitBugPriority>(priorities.Count);
                foreach (BugPriority priority in priorities)
                {
                    result.Add(new ManagedBugPriority(session, priority).TransitBugPriority);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug priority
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug priority.")]
        public void DeleteBugPriority(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugPriority m_priority = new ManagedBugPriority(session, id);
                m_priority.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugSeverity m_severity = new ManagedBugSeverity(session);
                m_severity.CreateOrUpdate(severity);
                SnCore.Data.Hibernate.Session.Flush();
                return m_severity.Id;
            }
        }

        /// <summary>
        /// Get a bug severity.
        /// </summary>
        /// <returns>transit bug severity</returns>
        [WebMethod(Description = "Get a bug severity.")]
        public TransitBugSeverity GetBugSeverityById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugSeverity result = new ManagedBugSeverity(session, id).TransitBugSeverity;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bug severities.
        /// </summary>
        /// <returns>list of transit bug severities</returns>
        [WebMethod(Description = "Get all bug severities.")]
        public List<TransitBugSeverity> GetBugSeverities()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList severities = session.CreateCriteria(typeof(BugSeverity)).List();
                List<TransitBugSeverity> result = new List<TransitBugSeverity>(severities.Count);
                foreach (BugSeverity severity in severities)
                {
                    result.Add(new ManagedBugSeverity(session, severity).TransitBugSeverity);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug severity
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug severity.")]
        public void DeleteBugSeverity(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugSeverity m_severity = new ManagedBugSeverity(session, id);
                m_severity.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugResolution m_resolution = new ManagedBugResolution(session);
                m_resolution.CreateOrUpdate(resolution);
                SnCore.Data.Hibernate.Session.Flush();
                return m_resolution.Id;
            }
        }

        /// <summary>
        /// Get a bug resolution.
        /// </summary>
        /// <returns>transit bug resolution</returns>
        [WebMethod(Description = "Get a bug resolution.")]
        public TransitBugResolution GetBugResolutionById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugResolution result = new ManagedBugResolution(session, id).TransitBugResolution;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all bug resolutions.
        /// </summary>
        /// <returns>list of transit bug resolutions</returns>
        [WebMethod(Description = "Get all bug resolutions.")]
        public List<TransitBugResolution> GetBugResolutions()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList resolutions = session.CreateCriteria(typeof(BugResolution)).List();
                List<TransitBugResolution> result = new List<TransitBugResolution>(resolutions.Count);
                foreach (BugResolution resolution in resolutions)
                {
                    result.Add(new ManagedBugResolution(session, resolution).TransitBugResolution);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug resolution
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug resolution.")]
        public void DeleteBugResolution(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugResolution m_resolution = new ManagedBugResolution(session, id);
                m_resolution.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugStatus m_status = new ManagedBugStatus(session);
                m_status.CreateOrUpdate(status);
                SnCore.Data.Hibernate.Session.Flush();
                return m_status.Id;
            }
        }

        /// <summary>
        /// Get a bug status.
        /// </summary>
        /// <returns>transit bug status</returns>
        [WebMethod(Description = "Get a bug status.")]
        public TransitBugStatus GetBugStatusById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugStatus result = new ManagedBugStatus(session, id).TransitBugStatus;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bug statuses.
        /// </summary>
        /// <returns>list of transit bug statuses</returns>
        [WebMethod(Description = "Get all bug statuses.")]
        public List<TransitBugStatus> GetBugStatuses()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList statuses = session.CreateCriteria(typeof(BugStatu)).List();
                List<TransitBugStatus> result = new List<TransitBugStatus>(statuses.Count);
                foreach (BugStatu status in statuses)
                {
                    result.Add(new ManagedBugStatus(session, status).TransitBugStatus);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug status
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug status.")]
        public void DeleteBugStatus(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugStatus m_status = new ManagedBugStatus(session, id);
                m_status.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugType m_type = new ManagedBugType(session);
                m_type.CreateOrUpdate(type);
                SnCore.Data.Hibernate.Session.Flush();
                return m_type.Id;
            }
        }

        /// <summary>
        /// Get a bug type.
        /// </summary>
        /// <returns>transit bug type</returns>
        [WebMethod(Description = "Get a bug type.")]
        public TransitBugType GetBugTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugType result = new ManagedBugType(session, id).TransitBugType;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bug types.
        /// </summary>
        /// <returns>list of transit bug types</returns>
        [WebMethod(Description = "Get all bug types.")]
        public List<TransitBugType> GetBugTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList types = session.CreateCriteria(typeof(BugType)).List();
                List<TransitBugType> result = new List<TransitBugType>(types.Count);
                foreach (BugType type in types)
                {
                    result.Add(new ManagedBugType(session, type).TransitBugType);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug type.")]
        public void DeleteBugType(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugType m_type = new ManagedBugType(session, id);
                m_type.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Project

        /// <summary>
        /// Create or update a bug project.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="project">transit bug project</param>
        [WebMethod(Description = "Create or update a bug project.")]
        public int CreateOrUpdateBugProject(string ticket, TransitBugProject project)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugProject m_project = new ManagedBugProject(session);
                m_project.CreateOrUpdate(project);
                SnCore.Data.Hibernate.Session.Flush();
                return m_project.Id;
            }
        }

        /// <summary>
        /// Get a bug project.
        /// </summary>
        /// <returns>transit bug project</returns>
        [WebMethod(Description = "Get a bug project.")]
        public TransitBugProject GetBugProjectById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugProject result = new ManagedBugProject(session, id).TransitBugProject;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bug projects.
        /// </summary>
        /// <returns>list of transit bug projects</returns>
        [WebMethod(Description = "Get all bug projects.")]
        public List<TransitBugProject> GetBugProjects()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList projects = session.CreateCriteria(typeof(BugProject)).List();
                List<TransitBugProject> result = new List<TransitBugProject>(projects.Count);
                foreach (BugProject project in projects)
                {
                    result.Add(new ManagedBugProject(session, project).TransitBugProject);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug project
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug project.")]
        public void DeleteBugProject(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugProject m_project = new ManagedBugProject(session, id);
                m_project.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Bug

        /// <summary>
        /// Create or update a bug.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="">transit bug </param>
        [WebMethod(Description = "Create or update a bug.")]
        public int CreateOrUpdateBug(string ticket, TransitBug bug)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator() && bug.Id > 0)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBug m_bug = new ManagedBug(session);
                if (bug.Id == 0) bug.AccountId = userid;
                m_bug.CreateOrUpdate(bug);
                SnCore.Data.Hibernate.Session.Flush();
                return m_bug.Id;
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Resolve(resolution);

                if (note.Length > 0)
                {
                    TransitBugNote t_note = new TransitBugNote();
                    t_note.BugId = bug.Id;
                    t_note.AccountId = userid;
                    t_note.Details = note;
                    ManagedBugNote m_note = new ManagedBugNote(session);
                    m_note.CreateOrUpdate(t_note);
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Close();

                ManagedBugNote note = new ManagedBugNote(session);
                TransitBugNote t_note = new TransitBugNote();
                t_note.BugId = bug.Id;
                t_note.AccountId = userid;
                t_note.Details = "Bug closed.";
                note.CreateOrUpdate(t_note);

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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBug bug = new ManagedBug(session, bugid);
                bug.Reopen();

                ManagedBugNote note = new ManagedBugNote(session);
                TransitBugNote t_note = new TransitBugNote();
                t_note.BugId = bug.Id;
                t_note.AccountId = userid;
                t_note.Details = "Bug reopened.";
                note.CreateOrUpdate(t_note);

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a bug.
        /// </summary>
        /// <returns>transit bug </returns>
        [WebMethod(Description = "Get a bug.")]
        public TransitBug GetBugById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBug result = new ManagedBug(session, id).TransitBug;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all bugs count.
        /// </summary>
        [WebMethod(Description = "Get all bugs.")]
        public int GetBugsCount(TransitBugQueryOptions bugoptions)
        {
            return GetBugs(bugoptions, null).Count;
        }

        /// <summary>
        /// Get all bugs.
        /// </summary>
        [WebMethod(Description = "Get all bugs.")]
        public List<TransitBug> GetBugs(TransitBugQueryOptions bugoptions, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery q = bugoptions.GetQuery(session);

                if (options != null)
                {
                    q.SetFirstResult(options.FirstResult);
                    q.SetMaxResults(options.PageSize);
                }

                IList s = q.List();

                List<TransitBug> result = new List<TransitBug>(s.Count);
                foreach (Bug bug in s)
                {
                    result.Add(new ManagedBug(session, bug).TransitBug);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug 
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug.")]
        public void DeleteBug(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBug m_bug = new ManagedBug(session, id);
                m_bug.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Notes

        /// <summary>
        /// Get all bug notes.
        /// </summary>
        /// <param name="projectid">project id</param>
        /// <returns>list of transit bug notes</returns>
        [WebMethod(Description = "Get all bug notes.")]
        public List<TransitBugNote> GetBugNotes(int bugid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList s = session.CreateCriteria(typeof(BugNote))
                    .Add(Expression.Eq("Bug.Id", bugid))
                    .AddOrder(Order.Desc("Modified"))
                    .List();
                List<TransitBugNote> result = new List<TransitBugNote>(s.Count);
                foreach (BugNote note in s)
                {
                    result.Add(new ManagedBugNote(session, note).TransitBugNote);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a bug note.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bug note.")]
        public void DeleteBugNote(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugNote m_bugnote = new ManagedBugNote(session, id);
                m_bugnote.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Create or update a bug note.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="">transit bug note</param>
        [WebMethod(Description = "Create or update a bug note.")]
        public int CreateOrUpdateBugNote(string ticket, TransitBugNote bugnote)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator() && bugnote.Id > 0)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedBugNote m_bugnote = new ManagedBugNote(session);
                bugnote.AccountId = userid;
                m_bugnote.CreateOrUpdate(bugnote);
                SnCore.Data.Hibernate.Session.Flush();
                return m_bugnote.Id;
            }
        }

        /// <summary>
        /// Get a bug note.
        /// </summary>
        /// <returns>transit bug note</returns>
        [WebMethod(Description = "Get a bug note.")]
        public TransitBugNote GetBugNoteById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBugNote result = new ManagedBugNote(session, id).TransitBugNote;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        #endregion
    }
}