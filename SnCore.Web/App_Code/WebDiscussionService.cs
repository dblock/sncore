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

        #region Discussion

        /// <summary>
        /// Add or get the current user tag discussion.
        /// </summary>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Add or get the current user tag discussion.")]
        public int GetTagDiscussionId(int accountid)
        {
            return GetDiscussionId(accountid, ManagedDiscussion.AccountTagsDiscussion, 0, true);
        }

        /// <summary>
        /// Add or get the account picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the account picture comment discussion.", CacheDuration = 60)]
        public int GetAccountPictureDiscussionId(int pictureid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture p = new ManagedAccountPicture(session, pictureid);
                accountid = p.AccountId;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountPictureDiscussion, pictureid, true);
        }

        /// <summary>
        /// Add or get the account event picture comment discussion.
        /// </summary>
        /// <param name="eventpictureid">event picture id</param>
        [WebMethod(Description = "Add or get the account event picture comment discussion.", CacheDuration = 60)]
        public int GetAccountEventPictureDiscussionId(int pictureid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEventPicture p = new ManagedAccountEventPicture(session, pictureid);
                accountid = p.AccountId;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountEventPictureDiscussion, pictureid, true);
        }

        /// <summary>
        /// Add or get the place picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the place picture comment discussion.", CacheDuration = 60)]
        public int GetPlacePictureDiscussionId(int pictureid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlacePicture p = new ManagedPlacePicture(session, pictureid);
                accountid = p.Place.Account.Id;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.PlacePictureDiscussion, pictureid, true);
        }


        /// <summary>
        /// Add or get the current story picture comment discussion.
        /// </summary>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Add or get the current story picture comment discussion.", CacheDuration = 60)]
        public int GetAccountStoryPictureDiscussionId(int pictureid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountStoryPicture p = new ManagedAccountStoryPicture(session, pictureid);
                accountid = p.AccountId;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountStoryPictureDiscussion, pictureid, true);
        }

        /// <summary>
        /// Add or get the current account story comment discussion.
        /// </summary>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Add or get the current account story comment discussion.", CacheDuration = 60)]
        public int GetAccountStoryDiscussionId(int storyid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountStory s = new ManagedAccountStory(session, storyid);
                accountid = s.AccountId;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountStoryDiscussion, storyid, true);
        }

        /// <summary>
        /// Add or get the place comments discussion.
        /// </summary>
        /// <param name="placeid">place id</param>
        [WebMethod(Description = "Add or get the place comments discussion.")]
        public int GetPlaceDiscussionId(int id)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                Place p = (Place)session.Load(typeof(Place), id);
                accountid = p.Account.Id;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.PlaceDiscussion, id, true);
        }

        /// <summary>
        /// Add or get the account event comment discussion.
        /// </summary>
        /// <param name="eventid">event id</param>
        [WebMethod(Description = "Add or get the account event comment discussion.", CacheDuration = 60)]
        public int GetAccountEventDiscussionId(int eventid)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEvent evt = new ManagedAccountEvent(session, eventid);
                accountid = evt.Account.Id;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountEventDiscussion, eventid, true);
        }

        /// <summary>
        /// Add or get the feed item discussion.
        /// </summary>
        /// <param name="accountfeeditemid">feed item id</param>
        [WebMethod(Description = "Add or get the feed item discussion.")]
        public int GetAccountFeedItemDiscussionId(int id)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                AccountFeedItem i = (AccountFeedItem)session.Load(typeof(AccountFeedItem), id);
                accountid = i.AccountFeed.Account.Id;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountFeedItemDiscussion, id, true);
        }

        /// <summary>
        /// Add or get the blog post discussion.
        /// </summary>
        /// <param name="accountblogpostid">blog post id</param>
        [WebMethod(Description = "Add or get the blog post discussion.")]
        public int GetAccountBlogPostDiscussionId(int id)
        {
            int accountid = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                AccountBlogPost i = (AccountBlogPost)session.Load(typeof(AccountBlogPost), id);
                accountid = i.AccountBlog.Account.Id;
            }

            return GetDiscussionId(accountid, ManagedDiscussion.AccountBlogPostDiscussion, id, true);
        }

        /// <summary>
        /// Get a discussion id.
        /// </summary>
        /// <param name="accountid">account id</param>
        /// <param name="name">discussion name</param>
        [WebMethod(Description = "Get a discussion id.")]
        public int GetDiscussionId(int accountid, string name)
        {
            return GetDiscussionId(accountid, name, 0, false);
        }

        protected int GetDiscussionId(int accountid, string name, int objectid, bool createonerror)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                int result = ManagedDiscussion.GetDiscussionId(session, accountid, name, objectid, createonerror);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Add a discussion.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="discussion">transit discussion</param>
        [WebMethod(Description = "Add a discussion.")]
        public int AddDiscussion(string ticket, TransitDiscussion discussion)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (discussion.AccountId == 0)
                {
                    discussion.AccountId = id;
                }

                if (discussion.AccountId != a.Id && ! a.IsAdministrator())
                {
                    // the discussion will belong to the current user
                    throw new ManagedAccount.AccessDeniedException();
                }

                discussion.Created = discussion.Modified = DateTime.UtcNow;

                ManagedDiscussion d = new ManagedDiscussion(session);
                d.Create(discussion);
                SnCore.Data.Hibernate.Session.Flush();
                return d.Id;
            }
        }

        /// <summary>
        /// Get all discussions.
        /// </summary>
        /// <returns>list of discussions</returns>
        [WebMethod(Description = "Get all discussions.", CacheDuration = 30)]
        public List<TransitDiscussion> GetDiscussions(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(Discussion))
                    .Add(Expression.Eq("Personal", false))
                    .AddOrder(Order.Desc("Modified"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList discussions = c.List();
                List<TransitDiscussion> result = new List<TransitDiscussion>(discussions.Count);
                foreach (Discussion d in discussions)
                {
                    result.Add(new ManagedDiscussion(session, d).TransitDiscussion);
                }
                session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get discussions count.
        /// </summary>
        /// <returns>number of account discussions</returns>
        [WebMethod(Description = "Get discussions count.")]
        public int GetDiscussionsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(
                    "SELECT COUNT(*) from Discussion d" +
                    " WHERE d.Personal = 0").UniqueResult();
            }
        }

        /// <summary>
        /// Get all discussions.
        /// </summary>
        /// <returns>list of discussions</returns>
        [WebMethod(Description = "Get account discussions.")]
        public List<TransitDiscussion> GetAccountDiscussions(string ticket, ServiceQueryOptions options)
        {
            int user_id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(Discussion))
                    .Add(Expression.Eq("Personal", false))
                    .Add(Expression.Eq("Account.Id", user_id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList discussions = c.List();

                List<TransitDiscussion> result = new List<TransitDiscussion>(discussions.Count);
                foreach (Discussion d in discussions)
                {
                    result.Add(new ManagedDiscussion(session, d).TransitDiscussion);
                }
                session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account discussions count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of account discussions</returns>
        [WebMethod(Description = "Get account discussions count.")]
        public int GetAccountDiscussionsCount(string ticket)
        {
            int user_id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(
                    "SELECT COUNT(*) from Discussion d" +
                    " where d.Account.Id = " + user_id.ToString() +
                    " and d.Personal = 0").UniqueResult();
            }
        }

        /// <summary>
        /// Delete a discussion.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion id</param>
        /// </summary>
        [WebMethod(Description = "Delete a discussion.")]
        public void DeleteDiscussion(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedDiscussion m = new ManagedDiscussion(session, id);

                if (m.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get discussion by id.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion by id.", CacheDuration = 30)]
        public TransitDiscussion GetDiscussionById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedDiscussion(session, id).TransitDiscussion;
            }
        }

        #endregion

        #region Discussion Posts

        /// <summary>
        /// Get discussion thread posts count.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts count.")]
        public int GetDiscussionThreadPostsCount(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(p) FROM DiscussionPost p WHERE p.DiscussionThread.Id = {0}", id))
                    .UniqueResult();                    
            }
        }

        /// <summary>
        /// Get discussion thread posts.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread posts.")]
        public List<TransitDiscussionPost> GetDiscussionThreadPosts(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = (userid > 0) ? new ManagedAccount(session, userid) : null;

                DiscussionThread thread = (DiscussionThread)session.Load(typeof(DiscussionThread), id);
                ManagedDiscussionThread m_thread = new ManagedDiscussionThread(session, thread);
                List<TransitDiscussionPost> result = m_thread.GetPosts();

                if (acct != null)
                {
                    foreach (TransitDiscussionPost post in result)
                    {
                        post.SetPermissions(thread.Discussion, acct);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Add a discussion post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="post">transit discussion post</param>
        [WebMethod(Description = "Add a discussion post.")]
        public int AddDiscussionPost(string ticket, TransitDiscussionPost post)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, userid);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                if (post.AccountId == 0) post.AccountId = userid;

                if (post.AccountId != a.Id)
                {
                    // the discussion post will belong to the current user
                    throw new ManagedAccount.AccessDeniedException();
                }

                int result = 0;
                if (post.Id > 0)
                {
                    DiscussionPost p = (DiscussionPost)session.Load(typeof(DiscussionPost), post.Id);
                    p.Body = post.Body;
                    p.Subject = post.Subject;
                    p.Modified = DateTime.UtcNow;
                    session.Save(p);
                }
                else
                {
                    ManagedDiscussion d = new ManagedDiscussion(session, post.DiscussionId);
                    result = d.CreatePost(post.AccountId, post.DiscussionPostParentId, post.Subject, post.Body);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
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
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = (userid > 0) ? new ManagedAccount(session, userid) : null;

                TransitDiscussionPost post = new ManagedDiscussionPost(session, id).GetTransitDiscussionPost();

                if (acct != null)
                {
                    Discussion d = (Discussion)session.Load(typeof(Discussion), post.DiscussionId);
                    post.SetPermissions(d, acct);
                }

                return post;
            }
        }

        /// <summary>
        /// Get discussion posts.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion posts.")]
        public List<TransitDiscussionPost> GetDiscussionPosts(string ticket, int id, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = (userid > 0) ? new ManagedAccount(session, userid) : null;

                Discussion d = (Discussion)session.Load(typeof(Discussion), id);
                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(
                    d.DiscussionThreads != null ? d.DiscussionThreads.Count : 0);

                if (d.DiscussionThreads != null)
                {
                    foreach (DiscussionThread thread in d.DiscussionThreads)
                    {
                        ManagedDiscussionThread m_thread = new ManagedDiscussionThread(session, thread);
                        result.AddRange(m_thread.GetPosts());
                    }

                    if (options != null)
                    {
                        if (options.FirstResult > 0) result.RemoveRange(0, options.FirstResult);
                        if (options.PageSize > 0 && result.Count > options.PageSize) result.RemoveRange(options.PageSize, result.Count - options.PageSize);
                    }

                    if (acct != null)
                    {
                        foreach (TransitDiscussionPost post in result)
                        {
                            post.SetPermissions(d, acct);
                        }
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
        public List<TransitDiscussionPost> GetLatestDiscussionPosts()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                    "SELECT {Post.*} FROM DiscussionPost {Post}" +
                    " WHERE Post.DiscussionPost_Id IN ( " +
                    "  SELECT MAX(DiscussionPost_Id) FROM DiscussionPost dp, DiscussionThread dt, Discussion d" +
                    "   WHERE dp.DiscussionThread_Id = dt.DiscussionThread_Id" +
                    "   AND dp.Created > DATEADD(\"day\", -7, getdate())" +
                    "   AND d.Discussion_Id = dt.Discussion_Id" +
                    "   AND d.Personal = 0" +
                    "   GROUP BY d.Discussion_Id" +
                    " ) ORDER BY Post.Modified DESC",
                    "Post",
                    typeof(DiscussionPost));

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost post in posts)
                {
                    result.Add(new ManagedDiscussionPost(session, post).GetTransitDiscussionPost());
                }

                return result;
            }
        }

        /// <summary>
        /// Get recent discussion posts.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get recent discussion posts.", CacheDuration = 60)]
        public List<TransitDiscussionPost> GetLatestDiscussionPostsById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(string.Format(
                    "from DiscussionPost post" +
                    " where post.DiscussionThread.Discussion.Id = {0}" +
                    " order by post.Created desc", id));

                if (options != null)
                {
                    if (options.FirstResult > 0) query.SetFirstResult(options.FirstResult);
                    if (options.PageSize > 0) query.SetMaxResults(options.PageSize);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost post in posts)
                {
                    result.Add(new ManagedDiscussionPost(session, post).GetTransitDiscussionPost());
                }

                return result;
            }
        }

        /// <summary>
        /// Delete a discussion post.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">discussion post id</param>
        /// </summary>
        [WebMethod(Description = "Delete a discussion post.")]
        public void DeleteDiscussionPost(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedDiscussionPost m = new ManagedDiscussionPost(session, id);
                Discussion d = (Discussion)session.Load(typeof(Discussion), m.DiscussionId);

                TransitDiscussionPost t_m = m.GetTransitDiscussionPost();
                t_m.SetPermissions(d, user);

                if (!t_m.CanDelete)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m.Delete();
                session.Flush();
            }
        }

        #endregion

        #region DiscussionThreads

        /// <summary>
        /// Get discussion threads count.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.")]
        public int GetDiscussionThreadsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(
                    "SELECT COUNT(*) from DiscussionPost post" +
                    " where post.DiscussionThread.Discussion.Id = " + id.ToString() +
                    " and post.DiscussionPostParent is null").UniqueResult();
            }
        }

        /// <summary>
        /// Get discussion threads count.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.")]
        public int GetDiscussionThreadsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(
                    "SELECT COUNT(t) from DiscussionThread t, Discussion d" +
                        " WHERE t.Discussion.Id = d.Id " +
                        " AND d.Personal = 0").UniqueResult();
            }
        }

        /// <summary>
        /// Get discussion threads.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads, newest first.")]
        public List<TransitDiscussionPost> GetDiscussionThreads(string ticket, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                    "SELECT {Post.*} FROM DiscussionPost {Post}" +
                    " WHERE Post.DiscussionPost_Id IN ( " +
                    " SELECT MAX(DiscussionPost_Id) FROM DiscussionPost dp, DiscussionThread dt, Discussion d" + 
                    "  WHERE dp.DiscussionThread_Id = dt.DiscussionThread_Id" + 
                    "  AND d.Discussion_Id = dt.Discussion_Id" + 
                    "  AND d.Personal = 0" +
                    "  GROUP BY dt.DiscussionThread_Id" + 
                    " ) ORDER BY Post.Modified DESC",
                    "Post",
                    typeof(DiscussionPost));

                if (options != null)
                {
                    query.SetMaxResults(options.PageSize);
                    query.SetFirstResult(options.FirstResult);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost p in posts)
                {
                    TransitDiscussionPost post = new ManagedDiscussionPost(session, p).GetTransitDiscussionPost();
                    result.Add(post);
                }

                return result;
            }
        }

        /// <summary>
        /// Get discussion threads.
        /// </summary>
        /// <param name="id">discussion id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads.")]
        public List<TransitDiscussionPost> GetDiscussionThreadsById(string ticket, int id, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = (userid > 0) ? new ManagedAccount(session, userid) : null;

                Discussion d = (Discussion)session.Load(typeof(Discussion), id);

                IQuery query = session.CreateSQLQuery(
                    "SELECT {Post.*} FROM DiscussionPost {Post}, DiscussionThread Thread, Discussion Discussion" +
                    " WHERE Post.DiscussionThread_Id = Thread.DiscussionThread_Id" +
                    " AND Thread.Discussion_Id = Discussion.Discussion_Id" +
                    " AND Discussion.Discussion_Id = " + id.ToString() +
                    " AND Post.DiscussionPostParent_Id IS NULL" +
                    " ORDER BY Thread.Modified DESC",
                    "Post",
                    typeof(DiscussionPost));

                if (options != null)
                {
                    query.SetMaxResults(options.PageSize);
                    query.SetFirstResult(options.FirstResult);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>();
                if (posts != null)
                {
                    foreach (DiscussionPost p in posts)
                    {
                        TransitDiscussionPost post = new ManagedDiscussionPost(session, p).GetTransitDiscussionPost();

                        if (acct != null)
                        {
                            post.SetPermissions(d, acct);
                        }

                        result.Add(post);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get discussion thread parent post.
        /// </summary>
        /// <param name="id">discussion thread id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion thread parent post.")]
        public TransitDiscussionPost GetDiscussionThreadPost(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = (userid > 0) ? new ManagedAccount(session, userid) : null;

                DiscussionPost post = (DiscussionPost) session.CreateQuery(
                    "from DiscussionPost post" +
                    " where post.DiscussionThread.Id = " + id.ToString() +
                    " and post.DiscussionPostParent is null").UniqueResult();

                if (post == null)
                    return null;

                TransitDiscussionPost result = new ManagedDiscussionPost(session, post).GetTransitDiscussionPost();

                if (acct != null)
                {
                    result.SetPermissions(
                        post.DiscussionThread.Discussion, 
                        acct);
                }

                return result;
            }
        }

        /// <summary>
        /// Get the count of discussion threads that a user participates in.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads count.", CacheDuration = 60)]
        public int GetUserDiscussionThreadsCount(DiscussionQueryOptions queryoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery query = session.CreateQuery(queryoptions.CountQuery);
                return (int)query.UniqueResult();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, userid);

                if (!acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                DiscussionThread thread = (DiscussionThread) session.Load(
                    typeof(DiscussionThread), threadid);

                if (thread.Discussion.DiscussionThreads != null)
                    thread.Discussion.DiscussionThreads.Remove(thread);

                thread.Discussion = (Discussion) session.Load(
                    typeof(Discussion), targetid);

                if (thread.Discussion.DiscussionThreads != null)
                    thread.Discussion.DiscussionThreads.Add(thread);

                session.Save(thread);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }


        /// <summary>
        /// Get discussion threads that a user participates in.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Get discussion threads that a user participates in.", CacheDuration = 60)]
        public List<TransitDiscussionPost> GetUserDiscussionThreads(
            DiscussionQueryOptions queryoptions, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery query = session.CreateQuery(queryoptions.SelectQuery);

                if (options != null)
                {
                    query.SetMaxResults(options.PageSize);
                    query.SetFirstResult(options.FirstResult);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost post in posts)
                {
                    TransitDiscussionPost p = new ManagedDiscussionPost(session, post).GetTransitDiscussionPost();
                    result.Add(p);
                }

                return result;
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
            // int userid = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                DiscussionThread t = (DiscussionThread)session.Load(typeof(DiscussionThread), id);
                return new TransitDiscussionThread(t);
            }
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
        public string GetAccountTestimonialsDiscussionName()
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
        public string GetAcountFeedItemDiscussionName()
        {
            return ManagedDiscussion.AccountFeedItemDiscussion;
        }

        /// <summary>
        /// Get the built-in name for a blog post discussion.
        /// </summary>
        [WebMethod(Description = "Get the built-in name for a blog post discussion.")]
        public string GetAcountBlogPostDiscussionName()
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
        public List<TransitDiscussionPost> SearchDiscussionPosts(string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {DiscussionPost.*} FROM DiscussionPost {DiscussionPost}" +
                        " WHERE FREETEXT ((Body, Subject), '" + Renderer.SqlEncode(s) + "')",
                        "DiscussionPost",
                        typeof(DiscussionPost));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost post in posts)
                {
                    result.Add(new ManagedDiscussionPost(session, post).GetTransitDiscussionPost());
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of discussion posts matching a query
        /// </summary>
        /// <returns>number of discussion posts</returns>
        [WebMethod(Description = "Return the number of discussion posts matching a query.", CacheDuration = 60)]
        public int SearchDiscussionPostsCount(string s)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {DiscussionPost.*} FROM DiscussionPost {DiscussionPost}" +
                        " WHERE FREETEXT ((Body, Subject), '" + Renderer.SqlEncode(s) + "')",
                        "DiscussionPost",
                        typeof(DiscussionPost));

                return query.List().Count;
            }
        }

        /// <summary>
        /// Search all discussion posts in a discussion.
        /// </summary>
        /// <returns>discussion posts that match free text</returns>
        [WebMethod(Description = "Search discussion posts in a discussion.", CacheDuration = 60)]
        public List<TransitDiscussionPost> SearchDiscussionPostsById(int id, string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {p.*} FROM DiscussionPost {p}, DiscussionThread t, Discussion d" +
                        " WHERE t.Discussion_Id = d.Discussion_Id" +
                        " AND t.DiscussionThread_Id = p.DiscussionThread_Id" +
                        " AND d.Discussion_Id = " + id.ToString() +
                        " AND FREETEXT ((p.Body, p.Subject), '" + Renderer.SqlEncode(s) + "')",
                        "p",
                        typeof(DiscussionPost));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList posts = query.List();

                List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(posts.Count);
                foreach (DiscussionPost post in posts)
                {
                    result.Add(new ManagedDiscussionPost(session, post).GetTransitDiscussionPost());
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of discussion posts matching a query in a discussion.
        /// </summary>
        /// <returns>number of discussion posts</returns>
        [WebMethod(Description = "Return the number of discussion posts matching a query in a discussion.", CacheDuration = 60)]
        public int SearchDiscussionPostsByIdCount(int id, string s)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {p.*} FROM DiscussionPost {p}, DiscussionThread t, Discussion d" +
                        " WHERE t.Discussion_Id = d.Discussion_Id" +
                        " AND t.DiscussionThread_Id = p.DiscussionThread_Id" +
                        " AND d.Discussion_Id = " + id.ToString() +
                        " AND FREETEXT ((p.Body, p.Subject), '" + Renderer.SqlEncode(s) + "')",
                        "p",
                        typeof(DiscussionPost));

                return query.List().Count;
            }
        }

        #endregion
    }
}
