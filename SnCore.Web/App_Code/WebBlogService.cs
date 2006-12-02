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
    /// Managed web blog services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebBlogService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebBlogService : WebService
    {

        public WebBlogService()
        {

        }

        #region Blog

        /// <summary>
        /// Get account blogs.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account blogs</returns>
        [WebMethod(Description = "Get account blogs.")]
        public List<TransitAccountBlog> GetAccountBlogs(string ticket)
        {
            return GetAccountBlogsById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account blogs.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs</returns>
        [WebMethod(Description = "Get account blogs.", CacheDuration = 60)]
        public List<TransitAccountBlog> GetAccountBlogsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountBlog))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"))
                    .List();

                List<TransitAccountBlog> result = new List<TransitAccountBlog>(list.Count);
                foreach (AccountBlog e in list)
                {
                    result.Add(new TransitAccountBlog(e));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get authored blogs.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs</returns>
        [WebMethod(Description = "Get account blogs.", CacheDuration = 60)]
        public List<TransitAccountBlog> GetAuthoredAccountBlogsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountBlogAuthor))
                    .Add(Expression.Eq("Account.Id", id))
                    .List();

                List<TransitAccountBlog> result = new List<TransitAccountBlog>(list.Count);
                foreach (AccountBlogAuthor e in list)
                {
                    result.Add(new TransitAccountBlog(e.AccountBlog));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get authored account blogs count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account blogs</returns>
        [WebMethod(Description = "Get authored account blogs count.")]
        public int GetAuthoredAccountBlogsCount(string ticket)
        {
            return GetAuthoredAccountBlogsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get authored account blogs count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs count</returns>
        [WebMethod(Description = "Get authored account blogs count.", CacheDuration = 60)]
        public int GetAuthoredAccountBlogsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountBlogAuthor s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account blogs count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account blogs</returns>
        [WebMethod(Description = "Get account blogs count.")]
        public int GetAccountBlogsCount(string ticket)
        {
            return GetAccountBlogsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account blogs count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs count</returns>
        [WebMethod(Description = "Get account blogs count.", CacheDuration = 60)]
        public int GetAccountBlogsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountBlog s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account blog by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">blog id</param>
        /// <returns>transit account blog</returns>
        [WebMethod(Description = "Get account blog by id.")]
        public TransitAccountBlog GetAccountBlogById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for blog
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountBlog(session, id).TransitAccountBlog;
            }
        }

        /// <summary>
        /// Create or update a blog.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="Blog">new Blog</param>
        [WebMethod(Description = "Create a blog.")]
        public int CreateOrUpdateAccountBlog(string ticket, TransitAccountBlog blog)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(blog);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Syndicate a blog.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Syndicate a blog.")]
        public int SyndicateAccountBlog(string ticket, int id)
        {
            int user_id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, user_id);
                ManagedAccountBlog mb = new ManagedAccountBlog(session, id);
                if (!user.IsAdministrator() && !mb.CanEdit(user_id))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                int result = mb.Syndicate();
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a blog.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="blogid">blog id</param>
        [WebMethod(Description = "Delete a blog.")]
        public void DeleteAccountBlog(string ticket, int blogid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountBlog s = new ManagedAccountBlog(session, blogid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region AccountBlogPosts

        /// <summary>
        /// Get account blog post by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">blog post id</param>
        /// <returns>transit account blog post</returns>
        [WebMethod(Description = "Get account blog post by id.")]
        public TransitAccountBlogPost GetAccountBlogPostById(string ticket, int id)
        {
            int user_id = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountBlogPost post = new ManagedAccountBlogPost(session, id).TransitAccountBlogPost;
                ManagedAccountBlog blog = new ManagedAccountBlog(session, post.AccountBlogId);
                blog.SetPermissions(user_id, post);
                return post;
            }
        }

        /// <summary>
        /// Get account blog posts count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account blog posts count</returns>
        [WebMethod(Description = "Get account blog posts count.", CacheDuration = 60)]
        public int GetAccountBlogPostsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(i) FROM AccountBlogPost i WHERE i.AccountBlog.Id = {0}",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account blog posts.
        /// </summary>
        /// <returns>transit account blog posts</returns>
        [WebMethod(Description = "Get account blog posts.", CacheDuration = 60)]
        public List<TransitAccountBlogPost> GetAccountBlogPostsById(string ticket, int id, ServiceQueryOptions options)
        {
            int user_id = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountBlogPost))
                    .Add(Expression.Eq("AccountBlog.Id", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList list = c.List();

                List<TransitAccountBlogPost> result = new List<TransitAccountBlogPost>(list.Count);

                ManagedAccountBlog blog = new ManagedAccountBlog(session, id);
                foreach (AccountBlogPost item in list)
                {
                    TransitAccountBlogPost post = new ManagedAccountBlogPost(session, item).TransitAccountBlogPost;
                    blog.SetPermissions(user_id, post);
                    result.Add(post);
                }

                return result;
            }
        }

        /// <summary>
        /// Create or update a blog post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="post">new blog post</param>
        [WebMethod(Description = "Create a blog post.")]
        public int CreateOrUpdateAccountBlogPost(string ticket, TransitAccountBlogPost post)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(post);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a blog post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="postid">post id</param>
        [WebMethod(Description = "Delete a blog post.")]
        public void DeleteAccountBlogPost(string ticket, int postid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountBlogPost post = new ManagedAccountBlogPost(session, postid);
                ManagedAccountBlog blog = new ManagedAccountBlog(session, post.AccountBlogId);
                ManagedAccount acct = new ManagedAccount(session, id);

                if (post.AccountId != acct.Id && ! acct.IsAdministrator() && ! blog.CanDelete(acct.Id))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                post.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region AccountBlogAuthors

        /// <summary>
        /// Get account blog authors count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account blog authors count</returns>
        [WebMethod(Description = "Get account blog authors count.", CacheDuration = 60)]
        public int GetAccountBlogAuthorsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(i) FROM AccountBlogAuthor i WHERE i.AccountBlog.Id = {0}",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account blog author by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">blog author id</param>
        /// <returns>transit account blog author</returns>
        [WebMethod(Description = "Get account blog author by id.")]
        public TransitAccountBlogAuthor GetAccountBlogAuthorById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for blog author
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountBlogAuthor(session, id).TransitAccountBlogAuthor;
            }
        }

        /// <summary>
        /// Get account blogs authored by user.
        /// </summary>
        /// <returns>transit account blog authors</returns>
        [WebMethod(Description = "Get account blogs authored by user.")]
        public List<TransitAccountBlogAuthor> GetAccountBlogAuthored(string ticket)
        {
            return GetAccountBlogAuthoredById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account blogs authored by user id.
        /// </summary>
        /// <returns>transit account blog authors</returns>
        [WebMethod(Description = "Get account blogs authored by user id.", CacheDuration = 60)]
        public List<TransitAccountBlogAuthor> GetAccountBlogAuthoredById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountBlogAuthor))
                    .Add(Expression.Eq("Account.Id", id));

                IList list = c.List();

                List<TransitAccountBlogAuthor> result = new List<TransitAccountBlogAuthor>(list.Count);

                foreach (AccountBlogAuthor item in list)
                {
                    result.Add(new ManagedAccountBlogAuthor(session, item).TransitAccountBlogAuthor);
                }

                return result;
            }
        }

        /// <summary>
        /// Get account blog authors.
        /// </summary>
        /// <returns>transit account blog authors</returns>
        [WebMethod(Description = "Get account blog authors.", CacheDuration = 60)]
        public List<TransitAccountBlogAuthor> GetAccountBlogAuthorsById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountBlogAuthor))
                    .Add(Expression.Eq("AccountBlog.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountBlogAuthor> result = new List<TransitAccountBlogAuthor>(list.Count);

                foreach (AccountBlogAuthor item in list)
                {
                    result.Add(new ManagedAccountBlogAuthor(session, item).TransitAccountBlogAuthor);
                }

                return result;
            }
        }

        /// <summary>
        /// Create or update a blog author.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="author">new blog author</param>
        [WebMethod(Description = "Create or update a blog author.")]
        public int CreateOrUpdateAccountBlogAuthor(string ticket, TransitAccountBlogAuthor author)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(author);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a blog author.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="authorid">author id</param>
        [WebMethod(Description = "Delete a blog author.")]
        public void DeleteAccountBlogAuthor(string ticket, int authorid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountBlogAuthor author = new ManagedAccountBlogAuthor(session, authorid);
                ManagedAccount acct = new ManagedAccount(session, id);

                if (author.BlogAccountId != acct.Id && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                author.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Search

        protected IList InternalSearchAccountBlogPosts(ISession session, string s, ServiceQueryOptions options)
        {
            int maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            IQuery query = session.CreateSQLQuery(
                    "SELECT {AccountBlogPost.*} FROM AccountBlogPost {AccountBlogPost}" +
                    " INNER JOIN FREETEXTTABLE(AccountBlogPost, ([Title], [Body]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft " +
                    " ON AccountBlogPost.AccountBlogPost_Id = ft.[KEY] " +
                    " ORDER BY ft.[RANK] DESC",
                    "AccountBlogPost",
                    typeof(AccountBlogPost));

            if (options != null)
            {
                query.SetFirstResult(options.FirstResult);
                query.SetMaxResults(options.PageSize);
            }

            return query.List();
        }

        /// <summary>
        /// Search blog posts.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search blog posts.", CacheDuration = 60)]
        public List<TransitAccountBlogPost> SearchAccountBlogPosts(string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitAccountBlogPost>();

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList BlogPosts = InternalSearchAccountBlogPosts(session, s, options);

                List<TransitAccountBlogPost> result = new List<TransitAccountBlogPost>(BlogPosts.Count);
                foreach (AccountBlogPost BlogPost in BlogPosts)
                {
                    result.Add(new ManagedAccountBlogPost(session, BlogPost).TransitAccountBlogPost);
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of blog posts matching a query.
        /// </summary>
        /// <returns>number of blog posts</returns>
        [WebMethod(Description = "Return the number of blog posts matching a query.", CacheDuration = 60)]
        public int SearchAccountBlogPostsCount(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return InternalSearchAccountBlogPosts(session, s, null).Count;
            }
       }

       #endregion    
    }
}