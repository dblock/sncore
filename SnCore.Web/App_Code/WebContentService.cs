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
        /// Get account content groups count.
        /// </summary>
        [WebMethod(Description = "Get account content groups count.", CacheDuration = 60)]
        public int GetAccountContentGroupsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.GetCount(
                ticket, string.Format("WHERE AccountContentGroup.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account content groups.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account content groups</returns>
        [WebMethod(Description = "Get account content groups.", CacheDuration = 60)]
        public List<TransitAccountContentGroup> GetAccountContentGroups(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account content groups count.
        /// </summary>
        [WebMethod(Description = "Get account content groups count.", CacheDuration = 60)]
        public int GetAllAccountContentGroupsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get account content groups.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account content groups</returns>
        [WebMethod(Description = "Get account content groups.", CacheDuration = 60)]
        public List<TransitAccountContentGroup> GetAllAccountContentGroups(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Create or update a content group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="group">content group</param>
        [WebMethod(Description = "Create a content group.")]
        public int CreateOrUpdateAccountContentGroup(string ticket, TransitAccountContentGroup group)
        {
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.CreateOrUpdate(
                ticket, group);
        }

        /// <summary>
        /// Delete a content group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">content group id</param>
        [WebMethod(Description = "Delete a content group.")]
        public void DeleteAccountContentGroup(string ticket, int id)
        {
            WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.Delete(
                ticket, id);
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
            return WebServiceImpl<TransitAccountContentGroup, ManagedAccountContentGroup, AccountContentGroup>.GetById(
                ticket, id);
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
            return WebServiceImpl<TransitAccountContent, ManagedAccountContent, AccountContent>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account content count.
        /// </summary>
        /// <param name="id">account group id</param>
        /// <returns>transit account contents count</returns>
        [WebMethod(Description = "Get account contents count by group id.", CacheDuration = 60)]
        public int GetAccountContentsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountContent, ManagedAccountContent, AccountContent>.GetCount(
                ticket, string.Format("WHERE AccountContent.AccountContentGroup.Id = {0}", id));
        }

        /// <summary>
        /// Get account contents in a group.
        /// </summary>
        /// <returns>transit account contents</returns>
        [WebMethod(Description = "Get account contents in a group.", CacheDuration = 60)]
        public List<TransitAccountContent> GetAccountContents(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountContentGroup.Id", id) };
            Order[] orders = { Order.Desc("Timestamp") };
            return WebServiceImpl<TransitAccountContent, ManagedAccountContent, AccountContent>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Create or update an account content.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="content">content</param>
        [WebMethod(Description = "Create an account content.")]
        public int CreateOrUpdateAccountContent(string ticket, TransitAccountContent content)
        {
            return WebServiceImpl<TransitAccountContent, ManagedAccountContent, AccountContent>.CreateOrUpdate(
                ticket, content);
        }

        /// <summary>
        /// Delete an account content.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="contentid">id</param>
        [WebMethod(Description = "Delete an account content.")]
        public void DeleteAccountContent(string ticket, int id)
        {
            WebServiceImpl<TransitAccountContent, ManagedAccountContent, AccountContent>.Delete(
                ticket, id);
        }

        #endregion
    }
}