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
using System.Reflection;

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
        /// Add or get a named discussion.
        /// </summary>
        /// <param name="id">object id</param>
        /// <param name="typename">object type</param>
        [WebMethod(Description = "Add or get a named discussion.")]
        public int GetOrCreateDiscussionId(string ticket, string typename, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                return ManagedDiscussion.GetOrCreateDiscussionId(session, typename, id, sec);
            }
        }

        /// <summary>
        /// Get named discussions count.
        /// </summary>
        /// <param name="id">object id</param>
        /// <param name="typename">object type</param>
        [WebMethod(Description = "Get named discussions count.")]
        public int GetDiscussionsByObjectIdCount(string ticket, string typename, int id)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetCount(
                ticket, string.Format("WHERE Discussion.ObjectId = {0} AND Discussion.DataObject.Name = '{1}'", 
                id, Renderer.SqlEncode(typename)));
        }

        /// <summary>
        /// Get named discussions.
        /// </summary>
        /// <param name="id">object id</param>
        /// <param name="typename">object type</param>
        [WebMethod(Description = "Get named discussion.")]
        public List<TransitDiscussion> GetDiscussionsByObjectId(string ticket, string typename, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDiscussion, ManagedDiscussion, Discussion>.GetList(
                ticket, options, string.Format("SELECT Discussion FROM Discussion Discussion" +
                    " WHERE Discussion.ObjectId = {0} AND Discussion.DataObject.Name = '{1}'" +
                    " ORDER BY Discussion.Name ASC",
                id, Renderer.SqlEncode(typename)));
        }

        /// <summary>
        /// Get a personal discussion redirect url.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a personal discussion redirect url.")]
        public string GetDiscussionRedirectUri(string ticket, int id)
        {
            TransitDiscussion td = GetDiscussionById(ticket, id);
            if (!td.Personal) throw new Exception(string.Format("Discussion {0} is not Personal", td.Name));
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(session, td.DataObjectId);
                return string.Format(mapentry.DiscussionUriFormat, td.ObjectId);
            }
        }

        /// <summary>
        /// Get a personal discussion thread redirect url.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a personal discussion redirect url.")]
        public string GetThreadRedirectUri(string ticket, int id)
        {
            TransitDiscussion td = GetDiscussionById(ticket, id);
            if (!td.Personal) throw new Exception(string.Format("Discussion {0} is not Personal", td.Name));
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(session, td.DataObjectId);
                return string.Format(mapentry.ThreadUriFormat, td.ObjectId);
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
        /// Get discussion thread posts in order of posting.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts in order of posting.")]
        public List<TransitDiscussionPost> GetDiscussionThreadPostsByOrder(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("DiscussionThread.Id", id) };
            Order[] orders = { Order.Desc("Sticky"), Order.Desc("Created") };
            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get discussion thread posts.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts.")]
        public List<TransitDiscussionPost> GetDiscussionThreadPosts(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
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
            using (SnCore.Data.Hibernate.Session.OpenConnection())
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
                    " ) ORDER BY Post.Sticky DESC, Post.Modified DESC",
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
                    " ORDER BY DiscussionPost.Sticky DESC, DiscussionPost.Created DESC", id));
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
                        " ) ORDER BY Post.Sticky DESC, Post.Modified DESC",
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
                        " ORDER BY Post.Sticky DESC, Thread.Modified DESC",
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
                        " ORDER BY Post.Sticky DESC, Thread.Modified DESC",
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
        [WebMethod(Description = "Move a discussion thread to another discussion.")]
        public void MoveDiscussionThread(string ticket, int threadid, int targetid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedDiscussionThread thread = new ManagedDiscussionThread(session, threadid);
                thread.Move(sec, targetid);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Move a discussion post.
        /// </summary>
        /// <param name="targetid">target discussion id</param>
        /// <param name="postid">post id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns></returns>
        [WebMethod(Description = "Move a discussion post to another discussion.")]
        public int MoveDiscussionPost(string ticket, int postid, int targetid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedDiscussionPost post = new ManagedDiscussionPost(session, postid);
                int id = post.Move(sec, targetid);
                SnCore.Data.Hibernate.Session.Flush();
                return id;
            }
        }

        /// <summary>
        /// Move a blog post.
        /// </summary>
        [WebMethod(Description = "Move a blog post to a discussion.")]
        public int MoveAccountBlogPost(string ticket, int postid, int targetid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountBlogPost post = new ManagedAccountBlogPost(session, postid);
                int id = post.MoveToDiscussion(sec, targetid);
                SnCore.Data.Hibernate.Session.Flush();
                return id;
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
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            return WebServiceImpl<TransitDiscussionPost, ManagedDiscussionPost, DiscussionPost>.GetList(
                ticket, options, "SELECT {DiscussionPost.*} FROM DiscussionPost {DiscussionPost}" +
                    " INNER JOIN DiscussionThread AS dt ON DiscussionPost.DiscussionThread_Id = dt.DiscussionThread_Id" +
                    " INNER JOIN Discussion AS d ON dt.Discussion_Id = d.Discussion_Id" +
                    " INNER JOIN FREETEXTTABLE(DiscussionPost, ([Subject], [Body]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft " +
                    " ON DiscussionPost.DiscussionPost_Id = ft.[KEY] " +
                    " WHERE d.Personal = 0 AND d.Object_Id = 0" +
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
            using (SnCore.Data.Hibernate.Session.OpenConnection())
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
