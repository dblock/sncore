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
using System.Text;

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
            return WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.CreateOrUpdate(
                ticket, type);
        }

        /// <summary>
        /// Get an account feed type.
        /// </summary>
        /// <returns>transit account feed type</returns>
        [WebMethod(Description = "Get an account feed type.")]
        public TransitFeedType GetFeedTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get an account feed type by name.
        /// </summary>
        /// <returns>transit account feed type</returns>
        [WebMethod(Description = "Get an account feed type by name.")]
        public TransitFeedType GetFeedTypeByName(string ticket, string name)
        {
            ICriterion[] expression = { Expression.Eq("Name", name) };
            return WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.GetByCriterion(
                ticket, expression);
        }

        /// <summary>
        /// Get all AccountFeed types.
        /// </summary>
        /// <returns>list of transit account feed types</returns>
        [WebMethod(Description = "Get all account feed types.")]
        public List<TransitFeedType> GetFeedTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all AccountFeed types count.
        /// </summary>
        /// <returns>number of transit account feed types</returns>
        [WebMethod(Description = "Get all account feed types count.")]
        public int GetFeedTypesCount(string ticket)
        {
            return WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete an account feed type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed type.")]
        public void DeleteFeedType(string ticket, int id)
        {
            WebServiceImpl<TransitFeedType, ManagedFeedType, FeedType>.Delete(
                ticket, id);
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
            int id = WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.CreateOrUpdate(
                ticket, feed);

            try
            {
                UpdateAccountFeedItems(ticket, id);
            }
            catch
            {

            }

            return id;
        }

        /// <summary>
        /// Update account feed items.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feedid">feed id</param>
        [WebMethod(Description = "Create or update an account feed.")]
        public int UpdateAccountFeedItems(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountFeed feed = new ManagedAccountFeed(session, id);
                int count = feed.Update(sec);
                SnCore.Data.Hibernate.Session.Flush();
                return count;
            }
        }

        /// <summary>
        /// Update account feed item images.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feedid">feed id</param>
        [WebMethod(Description = "Create or update account feed item images.")]
        public int UpdateAccountFeedItemImgs(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountFeed feed = new ManagedAccountFeed(session, id);
                int count = feed.UpdateImages(sec);
                SnCore.Data.Hibernate.Session.Flush();
                return count;
            }
        }

        /// <summary>
        /// Update account feed item medias.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feedid">feed id</param>
        [WebMethod(Description = "Create or update account feed item medias.")]
        public int UpdateAccountFeedItemMedias(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountFeed feed = new ManagedAccountFeed(session, id);
                int count = feed.UpdateMedias(sec);
                SnCore.Data.Hibernate.Session.Flush();
                return count;
            }
        }

        /// <summary>
        /// Get an account feed.
        /// </summary>
        /// <returns>transit account feed</returns>
        [WebMethod(Description = "Get an account feed.")]
        public TransitAccountFeed GetAccountFeedById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account feeds count.
        /// </summary>
        /// <returns>number of account feeds</returns>
        [WebMethod(Description = "Get account feeds count.", CacheDuration = 60)]
        public int GetAccountFeedsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.GetCount(
                ticket, string.Format("WHERE AccountFeed.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account feeds.
        /// </summary>
        /// <returns>list of account feeds</returns>
        [WebMethod(Description = "Get account feeds.", CacheDuration = 60)]
        public List<TransitAccountFeed> GetAccountFeeds(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account feeds count.
        /// </summary>
        /// <returns>all account feeds count</returns>
        [WebMethod(Description = "Get all account feeds count.", CacheDuration = 60)]
        public int GetAllAccountFeedsCount(string ticket, TransitAccountFeedQueryOptions qopt)
        {
            return WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.GetCount(
                ticket, qopt.CreateCountQuery());
        }

        /// <summary>
        /// Get updated account feeds.
        /// </summary>
        /// <returns>list of updated account feeds</returns>
        [WebMethod(Description = "Get updated account feeds.", CacheDuration = 60)]
        public List<TransitAccountFeed> GetAllAccountFeeds(string ticket, TransitAccountFeedQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.GetList(
                ticket, options, qopt.CreateQuery());
        }

        /// <summary>
        /// Delete an account feed.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed.")]
        public void DeleteAccountFeed(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFeed, ManagedAccountFeed, AccountFeed>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountFeedItem

        /// <summary>
        /// Get account feed items count.
        /// </summary>
        /// <returns>transit account feed items count</returns>
        [WebMethod(Description = "Get all account feed items count.", CacheDuration = 60)]
        public int GetAllAccountFeedItemsCount(string ticket, TransitAccountFeedItemQueryOptions qopt)
        {
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetCount(
                ticket, qopt.CreateCountQuery());
        }

        /// <summary>
        /// Get account feed items.
        /// </summary>
        /// <returns>transit account feed items</returns>
        [WebMethod(Description = "Get account feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> GetAllAccountFeedItems(string ticket, TransitAccountFeedItemQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetList(
                ticket, options, qopt.CreateQuery());
        }

        /// <summary>
        /// Delete an account feed item.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed item.")]
        public void DeleteAccountFeedItem(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.Delete(
                ticket, id);
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
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account feed items count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account feed items count</returns>
        [WebMethod(Description = "Get account feed items count.", CacheDuration = 60)]
        public int GetAccountFeedItemsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetCount(
                ticket, string.Format("WHERE AccountFeedItem.AccountFeed.Id = {0}", id));
        }

        /// <summary>
        /// Get account feed items.
        /// </summary>
        /// <returns>transit account feed items</returns>
        [WebMethod(Description = "Get account feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> GetAccountFeedItems(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountFeed.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Create or update an  account feed item.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update an account feed item.")]
        public int CreateOrUpdateAccountFeedItem(string ticket, TransitAccountFeedItem item)
        {
            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.CreateOrUpdate(
                ticket, item);
        }

        #endregion

        #region Search

        /// <summary>
        /// Search feed items.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search feed items.", CacheDuration = 60)]
        public List<TransitAccountFeedItem> SearchAccountFeedItems(string ticket, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new List<TransitAccountFeedItem>();
            }

            int maxsearchresults = 128;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            StringBuilder query = new StringBuilder();
            query.Append("SELECT {AccountFeedItem.*} FROM AccountFeedItem {AccountFeedItem}");
            query.AppendFormat(" INNER JOIN FREETEXTTABLE(AccountFeedItem, ([Title], [Description]), '{0}', {1}) as ft ", Renderer.SqlEncode(s), maxsearchresults);
            query.Append(" ON AccountFeedItem.AccountFeedItem_Id = ft.[KEY] ");
            query.Append(" ORDER BY ft.[RANK] DESC");

            return WebServiceImpl<TransitAccountFeedItem, ManagedAccountFeedItem, AccountFeedItem>.GetList(
                ticket, options, query.ToString(), "AccountFeedItem");
        }

        /// <summary>
        /// Return the number of feed items matching a query.
        /// </summary>
        /// <returns>number of feed items</returns>
        [WebMethod(Description = "Return the number of feed items matching a query.", CacheDuration = 60)]
        public int SearchAccountFeedItemsCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            return SearchAccountFeedItems(ticket, s, null).Count;
        }

        #endregion

        #region AccountFeedItemImg

        /// <summary>
        /// Get account feed item images count.
        /// </summary>
        /// <returns>transit account feed item images count</returns>
        [WebMethod(Description = "Get account feed item images count.", CacheDuration = 60)]
        public int GetAccountFeedItemImgsCount(string ticket, TransitAccountFeedItemImgQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeedItemImg, ManagedAccountFeedItemImg, AccountFeedItemImg>.GetCount(
                ticket, options.CreateCountQuery());
        }

        /// <summary>
        /// Get account feed item images.
        /// </summary>
        /// <returns>transit account feed item images</returns>
        [WebMethod(Description = "Get account feed item images.", CacheDuration = 60)]
        public List<TransitAccountFeedItemImg> GetAccountFeedItemImgs(string ticket, TransitAccountFeedItemImgQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeedItemImg, ManagedAccountFeedItemImg, AccountFeedItemImg>.GetList(
                ticket, options, qopt.CreateQuery());
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
            return WebServiceImpl<TransitAccountFeedItemImg, ManagedAccountFeedItemImg, AccountFeedItemImg>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update an account feed item image.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account feed item image</param>
        [WebMethod(Description = "Create or update an account feed item image.")]
        public int CreateOrUpdateAccountFeedItemImg(string ticket, TransitAccountFeedItemImg img)
        {
            return WebServiceImpl<TransitAccountFeedItemImg, ManagedAccountFeedItemImg, AccountFeedItemImg>.CreateOrUpdate(
                ticket, img);
        }

        /// <summary>
        /// Delete an account feed item image.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed item.")]
        public void DeleteAccountFeedItemImg(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFeedItemImg, ManagedAccountFeedItemImg, AccountFeedItemImg>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountFeedItemMedia

        /// <summary>
        /// Get account feed item medias count.
        /// </summary>
        /// <returns>transit account feed item medias count</returns>
        [WebMethod(Description = "Get account feed item medias count.", CacheDuration = 60)]
        public int GetAccountFeedItemMediasCount(string ticket, TransitAccountFeedItemMediaQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia, AccountFeedItemMedia>.GetCount(
                ticket, options.CreateCountQuery());
        }

        /// <summary>
        /// Get account feed item medias.
        /// </summary>
        /// <returns>transit account feed item medias</returns>
        [WebMethod(Description = "Get account feed item medias.", CacheDuration = 60)]
        public List<TransitAccountFeedItemMedia> GetAccountFeedItemMedias(string ticket, TransitAccountFeedItemMediaQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia, AccountFeedItemMedia>.GetList(
                ticket, options, qopt.CreateQuery());
        }

        /// <summary>
        /// Get account feed item media by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">feed item media id</param>
        /// <returns>transit account feed item media</returns>
        [WebMethod(Description = "Get account feed item media by id.")]
        public TransitAccountFeedItemMedia GetAccountFeedItemMediaById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia, AccountFeedItemMedia>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update an account feed item media.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account feed item media</param>
        [WebMethod(Description = "Create or update an account feed item media.")]
        public int CreateOrUpdateAccountFeedItemMedia(string ticket, TransitAccountFeedItemMedia img)
        {
            return WebServiceImpl<TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia, AccountFeedItemMedia>.CreateOrUpdate(
                ticket, img);
        }

        /// <summary>
        /// Delete an account feed item media.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account feed item.")]
        public void DeleteAccountFeedItemMedia(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia, AccountFeedItemMedia>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountRssWatch
        /// <summary>
        /// Create or update an rss watch.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit rss watch</param>
        [WebMethod(Description = "Create or update an rss watch.")]
        public int CreateOrUpdateAccountRssWatch(string ticket, TransitAccountRssWatch rsswatch)
        {
            return WebServiceImpl<TransitAccountRssWatch, ManagedAccountRssWatch, AccountRssWatch>.CreateOrUpdate(
                ticket, rsswatch);
        }

        /// <summary>
        /// Get an rss watch.
        /// </summary>
        /// <returns>transit rss watch</returns>
        [WebMethod(Description = "Get an rss watch.")]
        public TransitAccountRssWatch GetAccountRssWatchById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountRssWatch, ManagedAccountRssWatch, AccountRssWatch>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get rss watchs count.
        /// </summary>
        /// <returns>number of rss watchs</returns>
        [WebMethod(Description = "Get rss watchs count.", CacheDuration = 60)]
        public int GetAccountRssWatchsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountRssWatch, ManagedAccountRssWatch, AccountRssWatch>.GetCount(
                ticket, string.Format("WHERE AccountRssWatch.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get rss watchs.
        /// </summary>
        /// <returns>list of rss watchs</returns>
        [WebMethod(Description = "Get rss watchs.", CacheDuration = 60)]
        public List<TransitAccountRssWatch> GetAccountRssWatchs(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountRssWatch, ManagedAccountRssWatch, AccountRssWatch>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get rss items count.
        /// </summary>
        /// <returns>number of rss items</returns>
        [WebMethod(Description = "Get rss items count.", CacheDuration = 60)]
        public int GetAccountRssWatchItemsCount(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountRssWatch rsswatch = new ManagedAccountRssWatch(session, id);
                return rsswatch.GetSubscriptionUpdatesCount(sec);
            }
        }

        /// <summary>
        /// Get rss items.
        /// </summary>
        /// <returns>list of rss items</returns>
        [WebMethod(Description = "Get rss items.", CacheDuration = 60)]
        public TransitRssChannelItems GetAccountRssWatchItems(string ticket, int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountRssWatch rsswatch = new ManagedAccountRssWatch(session, id);
                TransitRssChannelItems result = rsswatch.GetSubscriptionUpdates(sec);
                result.Items = WebServiceQueryOptions<TransitRssItem>.Apply(options, result.Items);
                return result;
            }
        }

        /// <summary>
        /// Delete an rss watch.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an rss watch.")]
        public void DeleteAccountRssWatch(string ticket, int id)
        {
            WebServiceImpl<TransitAccountRssWatch, ManagedAccountRssWatch, AccountRssWatch>.Delete(
                ticket, id);
        }

        #endregion
    }
}
