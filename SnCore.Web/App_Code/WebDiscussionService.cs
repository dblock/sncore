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
    /// Managed web Discussion services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebDiscussionService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebDiscussionService : WebService
    {
        public WebDiscussionService()
        {

        }

        #region Named Discussions

        /// <summary>
        /// Add or get the current user tag discussion.
        /// </summary>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Add or get the current user tag discussion.")]
        public int GetOrCreateAccountTagsDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                Account instance = session.Load<Account>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Id, ManagedDiscussion.AccountTagsDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the account picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the account picture comment discussion.", CacheDuration = 60)]
        public int GetOrCreateAccountPictureDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountPicture instance = session.Load<AccountPicture>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Account.Id, ManagedDiscussion.AccountPictureDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the account event picture comment discussion.
        /// </summary>
        /// <param name="eventpictureid">event picture id</param>
        [WebMethod(Description = "Add or get the account event picture comment discussion.", CacheDuration = 60)]
        public int GetOrCreateAccountEventPictureDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountEventPicture instance = session.Load<AccountEventPicture>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.AccountEvent.Account.Id, ManagedDiscussion.AccountEventPictureDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the place picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the place picture comment discussion.", CacheDuration = 60)]
        public int GetOrCreatePlacePictureDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                PlacePicture instance = session.Load<PlacePicture>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Place.Account.Id, ManagedDiscussion.PlacePictureDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the current story picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the current story picture comment discussion.", CacheDuration = 60)]
        public int GetOrCreateAccountStoryPictureDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountStoryPicture instance = session.Load<AccountStoryPicture>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.AccountStory.Account.Id, ManagedDiscussion.AccountStoryPictureDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the current account story comment discussion.
        /// </summary>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Add or get the current account story comment discussion.", CacheDuration = 60)]
        public int GetOrCreateAccountStoryDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountStory instance = session.Load<AccountStory>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Account.Id, ManagedDiscussion.AccountStoryDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the place comments discussion.
        /// </summary>
        /// <param name="placeid">place id</param>
        [WebMethod(Description = "Add or get the place comments discussion.")]
        public int GetOrCreatePlaceDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                Place instance = session.Load<Place>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Account.Id, ManagedDiscussion.PlaceDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the account event comment discussion.
        /// </summary>
        /// <param name="eventid">event id</param>
        [WebMethod(Description = "Add or get the account event comment discussion.", CacheDuration = 60)]
        public int GetOrCreateAccountEventDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountEvent instance = session.Load<AccountEvent>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.Account.Id, ManagedDiscussion.AccountEventDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the feed item discussion.
        /// </summary>
        /// <param name="accountfeeditemid">feed item id</param>
        [WebMethod(Description = "Add or get the feed item discussion.")]
        public int GetOrCreateAccountFeedItemDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountFeedItem instance = session.Load<AccountFeedItem>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.AccountFeed.Account.Id, ManagedDiscussion.AccountFeedItemDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Add or get the blog post discussion.
        /// </summary>
        /// <param name="accountblogpostid">blog post id</param>
        [WebMethod(Description = "Add or get the blog post discussion.")]
        public int GetOrCreateAccountBlogPostDiscussionId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                AccountBlogPost instance = session.Load<AccountBlogPost>(id);
                return ManagedDiscussion.GetOrCreateDiscussionId(
                    session, instance.AccountBlog.Account.Id, ManagedDiscussion.AccountBlogPostDiscussion, id, sec);
            }
        }

        /// <summary>
        /// Get a discussion id.
        /// </summary>
        /// <param name="accountid">account id</param>
        /// <param name="name">discussion name</param>
        [WebMethod(Description = "Get a discussion id.")]
        public int GetDiscussionId(string ticket, int accountid, string name)
        {
            return GetDiscussionId(ticket, accountid, name, 0);
        }

        protected int GetDiscussionId(string ticket, int accountid, string name, int objectid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                int result = ManagedDiscussion.GetDiscussionId(session, accountid, name, objectid, sec);
                return result;
            }
        }

        #endregion

        #region Discussion

        /// <summary>
        /// Add a discussion.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="discussion">transit discussion</param>
        [WebMethod(Description = "Add a discussion.")]
        public int CreateOrUpdateDiscussion(string ticket, TransitDiscussion discussion)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.CreateOrUpdate(
                ticket, discussion);
        }

        /// <summary>
        /// Get all discussions.
        /// </summary>
        /// <returns>list of discussions</returns>
        [WebMethod(Description = "Get all discussions.", CacheDuration = 30)]
        public List<TransitDiscussion> GetDiscussions(string ticket, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Personal", false) };
            Order[] orders = { Order.Desc("Modified") };
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get discussions count.
        /// </summary>
        /// <returns>number of account discussions</returns>
        [WebMethod(Description = "Get discussions count.")]
        public int GetDiscussionsCount(string ticket)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetCount(
                ticket, "WHERE Discussion.Personal = 0");
        }

        /// <summary>
        /// Get all discussions.
        /// </summary>
        /// <returns>list of discussions</returns>
        [WebMethod(Description = "Get account discussions.")]
        public List<TransitDiscussion> GetAccountDiscussions(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = 
            {
                Expression.Eq("Personal", false),
                Expression.Eq("Account.Id", id)
            };

            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get account discussions count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of account discussions</returns>
        [WebMethod(Description = "Get account discussions count.")]
        public int GetAccountDiscussionsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetCount(
                ticket, string.Format("WHERE Discussion.Account.Id = {0} AND Discussion.Personal = 0", id));
        }

        /// <summary>
        /// Delete a discussion.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion id</param>
        /// </summary>
        [WebMethod(Description = "Delete a discussion.")]
        public void DeleteDiscussion(string ticket, int id)
        {
            WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get discussion by id.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion by id.", CacheDuration = 30)]
        public TransitDiscussion GetDiscussionById(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetById(
                ticket, id);
        }

        #endregion

        #region DiscussionPost

        /// <summary>
        /// Get discussion thread posts count.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts count.")]
        public int GetDiscussionThreadPostsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetCount(
                ticket, string.Format("WHERE DiscussionPost.DiscussionThread.Id = {0}", id));
        }

        /// <summary>
        /// Get discussion thread posts.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts.")]
        public List<TransitDiscussionPost> GetDiscussionThreadPosts(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ManagedDiscussionThread m_thread = new ManagedDiscussionThread(session, id);
                return m_thread.GetDiscussionPosts(sec);
            }
        }

        /// <summary>
        /// Add a discussion post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="post">transit discussion post</param>
        [WebMethod(Description = "Add a discussion post.")]
        public int CreateOrUpdateDiscussionPost(string ticket, TransitDiscussionPost post)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.CreateOrUpdate(
                ticket, post);
        }

        /// <summary>
        /// Get discussion post by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion post id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion post by id.")]
        public TransitDiscussionPost GetDiscussionPostById(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get discussion posts count.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion posts count.")]
        public int GetDiscussionPostsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetCount(
                ticket, string.Format("WHERE DiscussionPost.DiscussionThread.Discussion.Id = {0}", id));
        }

        /// <summary>
        /// Get discussion posts.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion posts.")]
        public List<TransitDiscussionPost> GetDiscussionPosts(string ticket, int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedDiscussion m_discussion = new ManagedDiscussion(session, id);
                List<TransitDiscussionPost> result = m_discussion.GetDiscussionPosts(sec);
                if (options != null)
                {
                    if (options.FirstResult > 0) result.RemoveRange(0, options.FirstResult);
                    if (options.PageSize > 0 && result.Count > options.PageSize)
                    {
                        result.RemoveRange(options.PageSize, result.Count - options.PageSize);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get latest discussion posts.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get latest discussion posts.", CacheDuration = 60)]
        public List<TransitDiscussionPost> GetLatestDiscussionPosts(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options,
                    "SELECT {Post.*} FROM DiscussionPost {Post}" +
                    " WHERE Post.DiscussionPost_Id IN ( " +
                    "  SELECT MAX(DiscussionPost_Id) FROM DiscussionPost dp, DiscussionThread dt, Discussion d" +
                    "   WHERE dp.DiscussionThread_Id = dt.DiscussionThread_Id" +
                    "   AND dp.Created > DATEADD(\"day\", -7, getdate())" +
                    "   AND d.Discussion_Id = dt.Discussion_Id" +
                    "   AND d.Personal = 0" +
                    "   GROUP BY d.Discussion_Id" +
                    " ) ORDER BY Post.Modified DESC",
                    "Post");
        }

        /// <summary>
        /// Get recent discussion posts.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get recent discussion posts.", CacheDuration = 60)]
        public List<TransitDiscussionPost> GetLatestDiscussionPostsById(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, string.Format(
                    "SELECT DiscussionPost FROM DiscussionPost DiscussionPost WHERE DiscussionPost.DiscussionThread.Discussion.Id = {0}" +
                    " ORDER BY DiscussionPost.Created DESC", id));
        }

        /// <summary>
        /// Delete a discussion post.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion post id</param>
        /// </summary>
        [WebMethod(Description = "Delete a discussion post.")]
        public void DeleteDiscussionPost(string ticket, int id)
        {
            WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.Delete(
                ticket, id);
        }

        #endregion

        #region DiscussionThread

        /// <summary>
        /// Get discussion threads count.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.")]
        public int GetDiscussionThreadsCount(string ticket)
        {
            return WebServiceImpl<TransitDiscussionThread, ManagedDiscussionThread, DiscussionThread>.GetCount(
                ticket, ", Discussion Discussion " +
                    " WHERE DiscussionThread.Discussion.Id = Discussion.Id " +
                    " AND Discussion.Personal = 0" +
                    " AND EXISTS ELEMENTS ( DiscussionThread.DiscussionPosts )");
        }

        /// <summary>
        /// Get discussion threads.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads, newest first.")]
        public List<TransitDiscussionPost> GetDiscussionThreads(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options,
                    "SELECT {Post.*} FROM DiscussionPost {Post}" +
                        " WHERE Post.DiscussionPost_Id IN ( " +
                        " SELECT MAX(dp.DiscussionPost_Id) FROM DiscussionPost dp, DiscussionThread dt, Discussion d" +
                        "  WHERE dp.DiscussionThread_Id = dt.DiscussionThread_Id" +
                        "  AND d.Discussion_Id = dt.Discussion_Id" +
                        "  AND d.Personal = 0" +
                        "  GROUP BY dt.DiscussionThread_Id" +
                        " ) ORDER BY Post.Modified DESC",
                    "Post");
        }

        /// <summary>
        /// Get top of discussion threads count.
        /// </summary>
        [WebMethod(Description = "Get top of discussion threads count.")]
        public int GetDiscussionTopOfThreadsCount(string ticket)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetCount(
                ticket, ", DiscussionThread DiscussionThread, Discussion Discussion" +
                    " WHERE DiscussionPost.DiscussionThread.Id = DiscussionThread.Id" +
                    " AND DiscussionThread.Discussion.Id = Discussion.Id" +
                    " AND DiscussionPost.DiscussionPostParent IS NULL" +
                    " AND Discussion.Personal = 0");
        }

        /// <summary>
        /// Get top of discussion threads.
        /// </summary>
        [WebMethod(Description = "Get top of discussion threads.")]
        public List<TransitDiscussionPost> GetDiscussionTopOfThreads(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options,
                    "SELECT {Post.*} FROM DiscussionPost {Post}, DiscussionThread Thread, Discussion Discussion" +
                        " WHERE Post.DiscussionThread_Id = Thread.DiscussionThread_Id" +
                        " AND Thread.Discussion_Id = Discussion.Discussion_Id" +
                        " AND Post.DiscussionPostParent_Id IS NULL" +
                        " AND Discussion.Personal = 0" +
                        " ORDER BY Thread.Modified DESC",
                    "Post");
        }

        /// <summary>
        /// Get discussion threads count.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.")]
        public int GetDiscussionThreadsCountByDiscussionId(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionThread, ManagedDiscussionThread, DiscussionThread>.GetCount(
                ticket, string.Format("WHERE DiscussionThread.Discussion.Id = {0}", id));
        }

        /// <summary>
        /// Get discussion threads.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads.")]
        public List<TransitDiscussionPost> GetDiscussionThreadsByDiscussionId(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options,
                    "SELECT {Post.*} FROM DiscussionPost {Post}, DiscussionThread Thread, Discussion Discussion" +
                        " WHERE Post.DiscussionThread_Id = Thread.DiscussionThread_Id" +
                        " AND Thread.Discussion_Id = Discussion.Discussion_Id" +
                        " AND Discussion.Discussion_Id = " + id.ToString() +
                        " AND Post.DiscussionPostParent_Id IS NULL" +
                        " ORDER BY Thread.Modified DESC",
                        "Post");
        }

        /// <summary>
        /// Get discussion thread parent post.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread parent post.")]
        public TransitDiscussionPost GetDiscussionThreadPost(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetByQuery(
                ticket, "FROM DiscussionPost DiscussionPost" +
                    " WHERE DiscussionPost.DiscussionThread.Id = " + id.ToString() +
                    " AND DiscussionPost.DiscussionPostParent IS NULL");
        }

        /// <summary>
        /// Get the count of discussion threads that a user participates in.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.", CacheDuration = 60)]
        public int GetUserDiscussionThreadsCount(string ticket, DiscussionQueryOptions qopt)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetCount(
                ticket, qopt.CountQuery);
        }

        /// <summary>
        /// Get discussion threads that a user participates in.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads that a user participates in.", CacheDuration = 60)]
        public List<TransitDiscussionPost> GetUserDiscussionThreads(
            string ticket, DiscussionQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, qopt.SelectQuery);
        }

        /// <summary>
        /// Move a discussion thread.
        /// </summary>
        /// <param name="targetid">target discussion id</param>
        /// <param name="threadid">thread id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns></returns>
        [WebMethod(Description = "Move a discussion thread.", CacheDuration = 60)]
        public void MoveDiscussionThread(string ticket, int threadid, int targetid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedDiscussionThread thread = new ManagedDiscussionThread(session, threadid);
                thread.Move(sec, targetid);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get discussion thread by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread by id.")]
        public TransitDiscussionThread GetDiscussionThreadById(string ticket, int id)
        {
            return WebServiceImpl<TransitDiscussionThread, ManagedDiscussionThread, DiscussionThread>.GetById(
                ticket, id);
        }

        #endregion

        #region Specialized Discussions

        /// <summary>
        /// Get the built-in name for an account event discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for an account event discussion.")]
        public string GetAccountEventDiscussionName()
        {
            return ManagedDiscussion.AccountEventDiscussion;
        }

        /// <summary>
        /// Get the built-in name for an account event picture discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for an account event picture discussion.")]
        public string GetAccountEventPictureDiscussionName()
        {
            return ManagedDiscussion.AccountEventPictureDiscussion;
        }

        /// <summary>
        /// Get the built-in name for an account picture discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for an account picture discussion.")]
        public string GetAccountPictureDiscussionName()
        {
            return ManagedDiscussion.AccountPictureDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a place picture discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a place picture discussion.")]
        public string GetPlacePictureDiscussionName()
        {
            return ManagedDiscussion.PlacePictureDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a story discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a story discussion.")]
        public string GetAccountStoryDiscussionName()
        {
            return ManagedDiscussion.AccountStoryDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a story picture discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a story picture discussion.")]
        public string GetAccountStoryPictureDiscussionName()
        {
            return ManagedDiscussion.AccountStoryPictureDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a testimonials discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a testimonials discussion.")]
        public string GetAccountTagsDiscussionName()
        {
            return ManagedDiscussion.AccountTagsDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a place comments discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a place comments discussion.")]
        public string GetPlaceDiscussionName()
        {
            return ManagedDiscussion.PlaceDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a feed item discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a feed item discussion.")]
        public string GetAccountFeedItemDiscussionName()
        {
            return ManagedDiscussion.AccountFeedItemDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a blog post discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a blog post discussion.")]
        public string GetAccountBlogPostDiscussionName()
        {
            return ManagedDiscussion.AccountBlogPostDiscussion;
        }

        #endregion

        #region Search

        /// <summary>
        /// Search all discussion posts.
        /// </summary>
        /// <returns>discussion posts that match free text</returns>
        [WebMethod(Description = "Search discussion posts.", CacheDuration = 60)]
        public List<TransitDiscussionPost> SearchDiscussionPosts(string ticket, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitDiscussionPost>();

            int maxsearchresults = 128;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, "SELECT {DiscussionPost.*} FROM DiscussionPost {DiscussionPost}" +
                    " INNER JOIN FREETEXTTABLE(DiscussionPost, ([Subject], [Body]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft " +
                    " ON DiscussionPost.DiscussionPost_Id = ft.[KEY] " +
                    " ORDER BY ft.[RANK] DESC",
                    "DiscussionPost");
        }

        /// <summary>
        /// Return the number of discussion posts matching a query
        /// </summary>
        /// <returns>number of discussion posts</returns>
        [WebMethod(Description = "Return the number of discussion posts matching a query.", CacheDuration = 60)]
        public int SearchDiscussionPostsCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            return SearchDiscussionPosts(ticket, s, null).Count;
        }

        /// <summary>
        /// Search all discussion posts in a discussion.
        /// </summary>
        /// <returns>discussion posts that match free text</returns>
        [WebMethod(Description = "Search discussion posts in a discussion.", CacheDuration = 60)]
        public List<TransitDiscussionPost> SearchDiscussionPostsById(string ticket, int id, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitDiscussionPost>();

            int maxsearchresults = 128;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, "SELECT {DiscussionPost.*} FROM DiscussionThread t, Discussion d, DiscussionPost {DiscussionPost}" +
                    " INNER JOIN FREETEXTTABLE(DiscussionPost, ([Subject], [Body]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft " +
                    " ON DiscussionPost.DiscussionPost_Id = ft.[KEY] " +
                    " WHERE t.Discussion_Id = d.Discussion_Id" +
                    " AND t.DiscussionThread_Id = DiscussionPost.DiscussionThread_Id" +
                    " AND d.Discussion_Id = " + id.ToString() +
                    " ORDER BY ft.[RANK] DESC",
                    "DiscussionPost");
        }

        /// <summary>
        /// Return the number of discussion posts matching a query in a discussion.
        /// </summary>
        /// <returns>number of discussion posts</returns>
        [WebMethod(Description = "Return the number of discussion posts matching a query in a discussion.", CacheDuration = 60)]
        public int SearchDiscussionPostsByIdCount(string ticket, int id, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            return SearchDiscussionPostsById(ticket, id, s, null).Count;
        }

        #endregion
    }
}
