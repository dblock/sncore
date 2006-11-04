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
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web content services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebContentService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebContentService : WebService
    {
        public WebContentService()
        {

        }

        #region Account Content Group

        /// <summary>
        /// Get account contents.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account contents</returns>
        [WebMethod(Description = "Get account content groups.")]
        public List<TransitAccountContentGroup> GetAccountContentGroups(string ticket, ServiceQueryOptions options)
        {
            return GetAccountContentGroupsById(ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account content groups count.
        /// </summary>
        [WebMethod(Description = "Get account content groups count.", CacheDuration = 60)]
        public int GetAccountContentGroupsCount(string ticket)
        {
            return GetAccountContentGroupsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account content groups count.
        /// </summary>
        [WebMethod(Description = "Get account content groups count.", CacheDuration = 60)]
        public int GetAccountContentGroupsCountById(int accountid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(g) FROM AccountContentGroup g WHERE g.Account.Id = {0}",
                        accountid)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all account content groups count.
        /// </summary>
        [WebMethod(Description = "Get all account content groups count.", CacheDuration = 60)]
        public int GetAllAccountContentGroupsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(g) FROM AccountContentGroup g").UniqueResult();
            }
        }

        /// <summary>
        /// Get all account content groups.
        /// </summary>
        /// <returns>transit account content groups</returns>
        [WebMethod(Description = "Get all account content groups.", CacheDuration = 60)]
        public List<TransitAccountContentGroup> GetAllAccountContentGroups(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountContentGroup))
                    .AddOrder(Order.Desc("Created"))
                    .List();

                List<TransitAccountContentGroup> result = new List<TransitAccountContentGroup>(list.Count);
                foreach (AccountContentGroup group in list)
                {
                    result.Add(new TransitAccountContentGroup(group));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account content groups.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account content groups</returns>
        [WebMethod(Description = "Get account content groups.", CacheDuration = 60)]
        public List<TransitAccountContentGroup> GetAccountContentGroupsById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountContentGroup))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"))
                    .List();

                List<TransitAccountContentGroup> result = new List<TransitAccountContentGroup>(list.Count);
                foreach (AccountContentGroup group in list)
                {
                    result.Add(new TransitAccountContentGroup(group));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Create or update a content group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="content">content group</param>
        [WebMethod(Description = "Create a content group.")]
        public int CreateOrUpdateAccountContentGroup(string ticket, TransitAccountContentGroup group)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(group);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a content group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="groupid">content group id</param>
        [WebMethod(Description = "Delete a content group.")]
        public void DeleteAccountContentGroup(string ticket, int groupid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountContentGroup g = new ManagedAccountContentGroup(session, groupid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != g.AccountId && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account content group by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">content group id</param>
        /// <returns>transit account content group</returns>
        [WebMethod(Description = "Get account content group by id.")]
        public TransitAccountContentGroup GetAccountContentGroupById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountContentGroup(session, id).TransitAccountContentGroup;
            }
        }

        #endregion

        #region Account Content

        /// <summary>
        /// Get account content by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">content id</param>
        /// <returns>transit account content</returns>
        [WebMethod(Description = "Get account content by id.")]
        public TransitAccountContent GetAccountContentById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountContent(session, id).TransitAccountContent;
            }
        }

        /// <summary>
        /// Get account contents count.
        /// </summary>
        /// <param name="id">account group id</param>
        /// <returns>transit account contents count</returns>
        [WebMethod(Description = "Get account contents count by group id.", CacheDuration = 60)]
        public int GetAccountContentsCountById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountContentGroup m = new ManagedAccountContentGroup(session, id);
                m.CheckPermissions(ticket);

                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(c) FROM AccountContent c WHERE c.AccountContentGroup.Id = {0}",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account contents in a group.
        /// </summary>
        /// <returns>transit account contents</returns>
        [WebMethod(Description = "Get account contents in a group.", CacheDuration = 60)]
        public List<TransitAccountContent> GetAccountContentsById(string ticket, int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountContentGroup m = new ManagedAccountContentGroup(session, id);
                m.CheckPermissions(ticket);

                ICriteria c = session.CreateCriteria(typeof(AccountContent))
                    .Add(Expression.Eq("AccountContentGroup.Id", id))
                    .AddOrder(Order.Desc("Timestamp"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList list = c.List();

                List<TransitAccountContent> result = new List<TransitAccountContent>(list.Count);

                foreach (AccountContent item in list)
                {
                    result.Add(new ManagedAccountContent(session, item).TransitAccountContent);
                }

                return result;
            }
        }

        /// <summary>
        /// Create or update an account content.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="content">new content</param>
        [WebMethod(Description = "Create an account content.")]
        public int CreateOrUpdateAccountContent(string ticket, TransitAccountContent content)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(content);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an account content.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="contentid">id</param>
        [WebMethod(Description = "Delete an account content.")]
        public void DeleteAccountContent(string ticket, int contentid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountContent content = new ManagedAccountContent(session, contentid);
                ManagedAccount acct = new ManagedAccount(session, id);

                if (content.AccountId != acct.Id && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                content.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion
    }
}