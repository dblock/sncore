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
using SnCore.Tools.Web;
using System.Text;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web story services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebStoryService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebStoryService : WebService
    {

        public WebStoryService()
        {

        }

        #region AccountStory

        /// <summary>
        /// Get account stories.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account stories</returns>
        [WebMethod(Description = "Get account stories.", CacheDuration = 60)]
        public List<TransitAccountStory> GetAccountStories(string ticket, int id, AccountStoryQueryOptions queryoptions, ServiceQueryOptions options)
        {
            List<ICriterion> expressions = new List<ICriterion>();
            expressions.Add(Expression.Eq("Account.Id", id));
            if (queryoptions != null && queryoptions.PublishedOnly) expressions.Add(Expression.Eq("Publish", true));

            Order[] orders = { Order.Desc("Created") };

            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetList(
                ticket, options, expressions.ToArray(), orders);
        }

        /// <summary>
        /// Get account stories count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account stories count</returns>
        [WebMethod(Description = "Get account stories count.", CacheDuration = 60)]
        public int GetAccountStoriesCount(string ticket, int id, AccountStoryQueryOptions queryoptions)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat("WHERE AccountStory.Account.Id = {0}", id);
            if (queryoptions != null && queryoptions.PublishedOnly)
                query.Append(" AND AccountStory.Publish = 1");

            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetCount(
                ticket, query.ToString());
        }

        /// <summary>
        /// Get all published account stories count.
        /// </summary>
        /// <returns>number of account stories</returns>
        [WebMethod(Description = "Get all published account stories count.", CacheDuration = 60)]
        public int GetAllAccountStoriesCount(string ticket)
        {
            ICriterion[] expressions = { Expression.Eq("Publish", true) };
            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account stories.
        /// </summary>
        /// <param name="options">query options</param>
        /// <returns>transit account stories</returns>
        [WebMethod(Description = "Get all published account stories.", CacheDuration = 60)]
        public List<TransitAccountStory> GetAllAccountStories(string ticket, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Publish", true) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account story by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">story id</param>
        /// <returns>transit account story</returns>
        [WebMethod(Description = "Get account story by id.")]
        public TransitAccountStory GetAccountStoryById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update an account story.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="story">new story</param>
        [WebMethod(Description = "Create or update an account story.")]
        public int CreateOrUpdateAccountStory(string ticket, TransitAccountStory story)
        {
            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.CreateOrUpdate(
                ticket, story);
        }

        /// <summary>
        /// Delete a story.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">story id</param>
        [WebMethod(Description = "Delete a story.")]
        public void DeleteAccountStory(string ticket, int id)
        {
            WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.Delete(
                ticket, id);
        }

        #endregion

        #region Search

        /// <summary>
        /// Return stories matching a query.
        /// </summary>
        /// <param name="options">service query options</param>
        /// <param name="s">search query</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit stories</returns>
        [WebMethod(Description = "Return stories matching a query.", CacheDuration = 60)]
        public List<TransitAccountStory> SearchAccountStories(string ticket, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitAccountStory>();

            int maxsearchresults = 128;
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            StringBuilder query = new StringBuilder();
            query.Append("SELECT {AccountStory.*} FROM AccountStory {AccountStory}");
            query.Append(" INNER JOIN FREETEXTTABLE(AccountStory, ([Name], [Summary]), '");
            query.Append(Renderer.SqlEncode(s));
            query.AppendFormat("', {0}) AS ft ", maxsearchresults);
            query.Append(" ON AccountStory.AccountStory_Id = ft.[KEY] ");
            query.Append(" WHERE AccountStory.Publish = 1");
            query.Append(" ORDER BY ft.[RANK] DESC");

            return WebServiceImpl<TransitAccountStory, ManagedAccountStory, AccountStory>.GetList(
                ticket, options, query.ToString(), "AccountStory");
        }

        /// <summary>
        /// Return the number of stories matching a query.
        /// </summary>
        /// <returns>number of stories</returns>
        [WebMethod(Description = "Return the number of stories matching a query.", CacheDuration = 60)]
        public int SearchAccountStoriesCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            return SearchAccountStories(ticket, s, null).Count;
        }

        #endregion

        #region AccountStoryPicture

        /// <summary>
        /// Get account story picture by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">story picture id</param>
        /// <returns>transit account story picture</returns>
        [WebMethod(Description = "Get account story picture by id.")]
        public TransitAccountStoryPicture GetAccountStoryPictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete an account story picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="storypictureid">story picture id</param>
        [WebMethod(Description = "Delete an account story picture.")]
        public void DeleteAccountStoryPicture(string ticket, int id)
        {
            WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get story pictures count.
        /// </summary>
        /// <param name="story">story id</param>
        [WebMethod(Description = "Get story pictures count.")]
        public int GetAccountStoryPicturesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("AccountStory.Id", id) };
            return WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get story pictures by account id.
        /// </summary>
        /// <param name="storyid">story id</param>
        /// <returns>transit story pictures</returns>
        [WebMethod(Description = "Get story pictures.", CacheDuration = 60)]
        public List<TransitAccountStoryPicture> GetAccountStoryPictures(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountStory.Id", id) };
            Order[] orders = { Order.Asc("Position"), Order.Desc("Created") };
            return WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Move a story picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="disp">disstory by positions</param>
        /// <param name="id">picture id</param>
        [WebMethod(Description = "Move a story picture.")]
        public void MoveAccountStoryPicture(string ticket, int id, int disp)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountStoryPicture m_instance = new ManagedAccountStoryPicture(session, id);
                m_instance.Move(sec, disp);
            }
        }

        /// <summary>
        /// Get story picture picture data if modified since.
        /// </summary>
        /// <param name="id">story picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get story picture picture data if modified since.", BufferResponse = true)]
        public TransitAccountStoryPicture GetAccountStoryPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAccountStoryPicture t_instance = WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Create or update an account story picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="story">new story picture</param>
        [WebMethod(Description = "Create or update an account story picture.")]
        public int CreateOrUpdateAccountStoryPicture(string ticket, TransitAccountStoryPicture picture)
        {
            return WebServiceImpl<TransitAccountStoryPicture, ManagedAccountStoryPicture, AccountStoryPicture>.CreateOrUpdate(
                ticket, picture);
        }

        #endregion
    }
}