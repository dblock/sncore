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
    /// Managed web syndication services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebSyndicationService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebSyndicationService : WebService
    {

        public WebSyndicationService()
        {

        }

        #region FeedType
        /// <summary>
        /// Create or update an account feed type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account feed type</param>
        [WebMethod(Description = "Create or update an account feed type.")]
        public int CreateOrUpdateFeedType(string ticket, TransitFeedType type)
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

                ManagedFeedType m_type = new ManagedFeedType(session);
                m_type.CreateOrUpdate(type);
                SnCore.Data.Hibernate.Session.Flush();
                return m_type.Id;
            }
        }

        /// <summary>
        /// Get an account feed type.
        /// </summary>
        /// <returns>transit account feed type</returns>
        [WebMethod(Description = "Get an account feed type.")]
        public TransitFeedType GetFeedTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitFeedType result = new ManagedFeedType(session, id).TransitFeedType;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get an account feed type by name.
        /// </summary>
        /// <returns>transit account feed type</returns>
        [WebMethod(Description = "Get an account feed type by name.")]
        public TransitFeedType GetFeedTypeByName(string name)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitFeedType result = new ManagedFeedType(session, 
                    ManagedFeedType.Find(session, name)).TransitFeedType;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all AccountFeed types.
        /// </summary>
        /// <returns>list of transit account feed types</returns>
        [WebMethod(Description = "Get all account feed types.")]
        public List<TransitFeedType> GetFeedTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList types = session.CreateCriteria(typeof(FeedType)).List();
                List<TransitFeedType> result = new List<TransitFeedType>(types.Count);
                foreach (FeedType type in types)
                {
                    result.Add(new ManagedFeedType(session, type).TransitFeedType);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an account feed type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed type.")]
        public void DeleteFeedType(string ticket, int id)
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

                ManagedFeedType m_type = new ManagedFeedType(session, id);
                m_type.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }
        #endregion

        #region AccountFeed
        /// <summary>
        /// Create or update an account feed.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account feed</param>
        [WebMethod(Description = "Create or update an account feed.")]
        public int CreateOrUpdateAccountFeed(string ticket, TransitAccountFeed feed)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if ((feed.AccountId != 0) && (feed.AccountId != user.Id) && (! user.IsAdministrator()))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (feed.AccountId == 0) feed.AccountId = user.Id;
                ManagedAccount account = new ManagedAccount(session, feed.AccountId);
                int result = account.CreateOrUpdate(feed);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Update account feed items.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feedid">feed id</param>
        [WebMethod(Description = "Create or update an account feed.")]
        public int UpdateAccountFeedItems(string ticket, int feedid)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feedid);

                if ((m_feed.AccountId != 0) && (m_feed.AccountId != user.Id) && (!user.IsAdministrator()))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                int result = m_feed.Update();
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Update account feed item images.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feedid">feed id</param>
        [WebMethod(Description = "Create or update account feed item images.")]
        public int UpdateAccountFeedItemImgs(string ticket, int feedid)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feedid);

                if ((m_feed.AccountId != 0) && (m_feed.AccountId != user.Id) && (!user.IsAdministrator()))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                int result = m_feed.UpdateImages();
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get an account feed.
        /// </summary>
        /// <returns>transit account feed</returns>
        [WebMethod(Description = "Get an account feed.")]
        public TransitAccountFeed GetAccountFeedById(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountFeed result = new ManagedAccountFeed(session, id).TransitAccountFeed;
                ManagedAccount user = (userid > 0) ? new ManagedAccount(session, userid) : null;

                if ((user == null) || ((user.Id != result.AccountId) && !user.IsAdministrator()))
                {
                    // clear potentially private fields
                    result.Username = string.Empty;
                    result.Password = string.Empty;
                    result.FeedUrl = string.Empty;
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get account feeds.
        /// </summary>
        /// <returns>list of account feeds</returns>
        [WebMethod(Description = "Get account feeds.")]
        public List<TransitAccountFeed> GetAccountFeeds(string ticket, ServiceQueryOptions options)
        {
            return GetAccountFeedsById(ticket, ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account feeds count.
        /// </summary>
        /// <returns>number of account feeds</returns>
        [WebMethod(Description = "Get account feeds count.")]
        public int GetAccountFeedsCount(string ticket)
        {
            return GetAccountFeedsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account feeds count.
        /// </summary>
        /// <returns>number of account feeds</returns>
        [WebMethod(Description = "Get account feeds count.", CacheDuration = 60)]
        public int GetAccountFeedsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(i) FROM AccountFeed i WHERE i.Account.Id = {0}",
                        id)).UniqueResult();
            }
        }


        /// <summary>
        /// Get account feeds.
        /// </summary>
        /// <returns>list of account feeds</returns>
        [WebMethod(Description = "Get account feeds.", CacheDuration = 60)]
        public List<TransitAccountFeed> GetAccountFeedsById(string ticket, int id, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = (userid > 0) ? new ManagedAccount(session, userid) : null;
                ICriteria c = session.CreateCriteria(typeof(AccountFeed))
                    .Add(Expression.Eq("Account.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList feeds = c.List();
                List<TransitAccountFeed> result = new List<TransitAccountFeed>(feeds.Count);
                foreach (AccountFeed feed in feeds)
                {
                    TransitAccountFeed f = new ManagedAccountFeed(session, feed).TransitAccountFeed;
                    if ((user == null) || ((user.Id != f.AccountId) && !user.IsAdministrator()))
                    {
                        // clear potentially private fields
                        f.Username = string.Empty;
                        f.Password = string.Empty;
                        f.FeedUrl = string.Empty;
                    }
                    result.Add(f);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get updated account feeds count.
        /// </summary>
        /// <returns>updated account feeds count</returns>
        [WebMethod(Description = "Get updated account feeds count.", CacheDuration = 60)]
        public int GetUpdatedAccountFeedsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(f) " +
                    "FROM AccountFeed f").UniqueResult();
            }
        }
 
        /// <summary>
        /// Get updated account feeds.
        /// </summary>
        /// <returns>list of updated account feeds</returns>
        [WebMethod(Description = "Get updated account feeds.", CacheDuration = 60)]
        public List<TransitAccountFeed> GetUpdatedAccountFeeds(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery query = session.CreateQuery(
                    "SELECT feed.Id FROM AccountFeed feed LEFT JOIN feed.AccountFeedItems item " +
                    "GROUP BY feed.Id ORDER BY MAX(item.Created) DESC");

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList feeds = query.List();
                List<TransitAccountFeed> result = new List<TransitAccountFeed>(feeds.Count);
                foreach (int feed_id in feeds)
                {
                    TransitAccountFeed f = new ManagedAccountFeed(session, feed_id).TransitAccountFeed;
                    // clear potentially private fields
                    f.Username = string.Empty;
                    f.Password = string.Empty;
                    f.FeedUrl = string.Empty;
                    result.Add(f);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an account feed.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed.")]
        public void DeleteAccountFeed(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountFeed m_feed = new ManagedAccountFeed(session, id);

                if (m_feed.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_feed.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }
        #endregion

        #region AccountFeedItem

        /// <summary>
        /// Get account feed items count.
        /// </summary>
        /// <returns>transit account feed items count</returns>
        [WebMethod(Description = "Get account feed items count.", CacheDuration = 60)]
        public int GetAccountFeedItemsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(
                    "SELECT COUNT(i) FROM AccountFeedItem i" +
                    " WHERE i.AccountFeed.Publish = 1").UniqueResult();
            }
        }

        /// <summary>
        /// Get account feed items.
        /// </summary>
        /// <returns>transit account feed items</returns>
        [WebMethod(Description = "Get account feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> GetAccountFeedItems(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery q = session.CreateQuery(
                    "SELECT i FROM AccountFeedItem i" +
                    " WHERE i.AccountFeed.Publish = 1" + 
                    " ORDER BY Created DESC");

                if (options != null)
                {
                    q.SetMaxResults(options.PageSize);
                    q.SetFirstResult(options.FirstResult);
                }

                IList list = q.List();

                List<TransitAccountFeedItem> result = new List<TransitAccountFeedItem>(list.Count);

                foreach (AccountFeedItem item in list)
                {
                    result.Add(new ManagedAccountFeedItem(session, item).TransitAccountFeedItem);
                }

                return result;
            }
        }


        /// <summary>
        /// Get account feed item by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">feed item id</param>
        /// <returns>transit account feed item</returns>
        [WebMethod(Description = "Get account feed item by id.")]
        public TransitAccountFeedItem GetAccountFeedItemById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for story
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountFeedItem(session, id).TransitAccountFeedItem;
            }
        }

        /// <summary>
        /// Get account feed items count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account feed items count</returns>
        [WebMethod(Description = "Get account feed items count.", CacheDuration = 60)]
        public int GetAccountFeedItemsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(i) FROM AccountFeedItem i WHERE i.AccountFeed.Id = {0}",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account feed items.
        /// </summary>
        /// <returns>transit account feed items</returns>
        [WebMethod(Description = "Get account feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> GetAccountFeedItemsById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountFeedItem))
                    .Add(Expression.Eq("AccountFeed.Id", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList list = c.List();

                List<TransitAccountFeedItem> result = new List<TransitAccountFeedItem>(list.Count);

                foreach (AccountFeedItem item in list)
                {
                    result.Add(new ManagedAccountFeedItem(session, item).TransitAccountFeedItem);
                }

                return result;
            }
        }
        #endregion

        #region Search

        /// <summary>
        /// Search feed items.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> SearchAccountFeedItems(string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {AccountFeedItem.*} FROM AccountFeedItem {AccountFeedItem}" +
                        " WHERE FREETEXT ((Title, Description), '" + Renderer.SqlEncode(s) + "')",
                        "AccountFeedItem",
                        typeof(AccountFeedItem));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList FeedItems = query.List();

                List<TransitAccountFeedItem> result = new List<TransitAccountFeedItem>(FeedItems.Count);
                foreach (AccountFeedItem FeedItem in FeedItems)
                {
                    result.Add(new ManagedAccountFeedItem(session, FeedItem).TransitAccountFeedItem);
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of feed items matching a query.
        /// </summary>
        /// <returns>number of feed items</returns>
        [WebMethod(Description = "Return the number of feed items matching a query.", CacheDuration = 60)]
        public int SearchAccountFeedItemsCount(string s)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {AccountFeedItem.*} FROM AccountFeedItem {AccountFeedItem}" +
                        " WHERE FREETEXT ((Title, Description), '" + Renderer.SqlEncode(s) + "')",
                        "AccountFeedItem",
                        typeof(AccountFeedItem));

                return query.List().Count;
            }
        }

        #endregion

        #region AccountFeedItemImg

        /// <summary>
        /// Get account feed item images count.
        /// </summary>
        /// <returns>transit account feed item images count</returns>
        [WebMethod(Description = "Get account feed item images count.", CacheDuration = 60)]
        public int GetAccountFeedItemImgsCount(TransitAccountFeedItemImgQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) options.CreateCountQuery(session).UniqueResult();
            }
        }

        /// <summary>
        /// Get account feed item images.
        /// </summary>
        /// <returns>transit account feed item images</returns>
        [WebMethod(Description = "Get account feed item images.", CacheDuration = 60)]
        public List<TransitAccountFeedItemImg> GetAccountFeedItemImgs(TransitAccountFeedItemImgQueryOptions qopt, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery q = qopt.CreateQuery(session);

                if (options != null)
                {
                    q.SetMaxResults(options.PageSize);
                    q.SetFirstResult(options.FirstResult);
                }

                IList list = q.List();

                List<TransitAccountFeedItemImg> result = new List<TransitAccountFeedItemImg>(list.Count);

                foreach (AccountFeedItemImg item in list)
                {
                    TransitAccountFeedItemImg img = new ManagedAccountFeedItemImg(session, item).TransitAccountFeedItemImg;
                    img.Thumbnail = null;
                    result.Add(img);
                }

                return result;
            }
        }

        /// <summary>
        /// Get account feed item image by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">feed item image id</param>
        /// <returns>transit account feed item image</returns>
        [WebMethod(Description = "Get account feed item image by id.")]
        public TransitAccountFeedItemImg GetAccountFeedItemImgById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountFeedItemImg(session, id).TransitAccountFeedItemImg;
            }
        }

        /// <summary>
        /// Create or update an account feed item image.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account feed item image</param>
        [WebMethod(Description = "Create or update an account feed item image.")]
        public int CreateOrUpdateAccountFeedItemImg(string ticket, TransitAccountFeedItemImg img)
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

                ManagedAccountFeedItemImg m_img = new ManagedAccountFeedItemImg(session);
                m_img.CreateOrUpdate(img);
                SnCore.Data.Hibernate.Session.Flush();
                return img.Id;
            }
        }


        #endregion
    }
}