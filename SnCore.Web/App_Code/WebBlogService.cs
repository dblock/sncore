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
        /// <param name="id">account id</param>
        /// <returns>transit account blogs</returns>
        [WebMethod(Description = "Get account blogs.", CacheDuration = 60)]
        public List<TransitAccountBlog> GetAccountBlogs(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expression = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.GetList(
                ticket, options, expression, orders);
        }

        /// <summary>
        /// Get authored blogs.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs</returns>
        [WebMethod(Description = "Get account blogs.", CacheDuration = 60)]
        public List<TransitAccountBlog> GetAuthoredAccountBlogs(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.GetList(
                ticket, options, "SELECT AccountBlogAuthor.AccountBlog FROM AccountBlogAuthor AccountBlogAuthor " +
                    string.Format("WHERE AccountBlogAuthor.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get authored account blogs count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs count</returns>
        [WebMethod(Description = "Get authored account blogs count.", CacheDuration = 60)]
        public int GetAuthoredAccountBlogsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.GetCount(
                ticket, string.Format("WHERE AccountBlogAuthor.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account blogs count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account blogs count</returns>
        [WebMethod(Description = "Get account blogs count.", CacheDuration = 60)]
        public int GetAccountBlogsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.GetCount(
                ticket, string.Format("WHERE AccountBlog.Account.Id = {0}", id));
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
            return WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update a blog.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="Blog">new Blog</param>
        [WebMethod(Description = "Create a blog.")]
        public int CreateOrUpdateAccountBlog(string ticket, TransitAccountBlog blog)
        {
            return WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.CreateOrUpdate(
                ticket, blog);
        }

        /// <summary>
        /// Syndicate a blog.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Syndicate a blog.")]
        public int SyndicateAccountBlog(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountBlog m_blog = new ManagedAccountBlog(session, id);
                int result = m_blog.Syndicate(sec);
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
        public void DeleteAccountBlog(string ticket, int id)
        {
            WebServiceImpl<TransitAccountBlog, ManagedAccountBlog, AccountBlog>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountBlogPost

        /// <summary>
        /// Get account blog post by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">blog post id</param>
        /// <returns>transit account blog post</returns>
        [WebMethod(Description = "Get account blog post by id.")]
        public TransitAccountBlogPost GetAccountBlogPostById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account blog posts count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account blog posts count</returns>
        [WebMethod(Description = "Get account blog posts count.", CacheDuration = 60)]
        public int GetAccountBlogPostsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.GetCount(
                ticket, string.Format("WHERE AccountBlogPost.AccountBlog.Id = {0}", id));
        }

        /// <summary>
        /// Get account blog posts.
        /// </summary>
        /// <returns>transit account blog posts</returns>
        [WebMethod(Description = "Get account blog posts.", CacheDuration = 60)]
        public List<TransitAccountBlogPost> GetAccountBlogPosts(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountBlog.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Create or update a blog post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="post">new blog post</param>
        [WebMethod(Description = "Create a blog post.")]
        public int CreateOrUpdateAccountBlogPost(string ticket, TransitAccountBlogPost post)
        {
            return WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.CreateOrUpdate(
                ticket, post);
        }

        /// <summary>
        /// Delete a blog post.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="postid">post id</param>
        [WebMethod(Description = "Delete a blog post.")]
        public void DeleteAccountBlogPost(string ticket, int id)
        {
            WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountBlogAuthor

        /// <summary>
        /// Get account blog authors count.
        /// </summary>
        /// <param name="id">account feed id</param>
        /// <returns>transit account blog authors count</returns>
        [WebMethod(Description = "Get account blog authors count.", CacheDuration = 60)]
        public int GetAccountBlogAuthorsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.GetCount(
                ticket, string.Format("WHERE AccountBlogAuthor.AccountBlog.Id = {0}", id));
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
            return WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account blog authors.
        /// </summary>
        /// <returns>transit account blog authors</returns>
        [WebMethod(Description = "Get account blog authors.", CacheDuration = 60)]
        public List<TransitAccountBlogAuthor> GetAccountBlogAuthors(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountBlog.Id", id) };
            return WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Create or update a blog author.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="author">new blog author</param>
        [WebMethod(Description = "Create or update a blog author.")]
        public int CreateOrUpdateAccountBlogAuthor(string ticket, TransitAccountBlogAuthor author)
        {
            return WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.CreateOrUpdate(
                ticket, author);
        }

        /// <summary>
        /// Delete a blog author.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="authorid">author id</param>
        [WebMethod(Description = "Delete a blog author.")]
        public void DeleteAccountBlogAuthor(string ticket, int id)
        {
            WebServiceImpl<TransitAccountBlogAuthor, ManagedAccountBlogAuthor, AccountBlogAuthor>.Delete(
                ticket, id);
        }

        #endregion

        #region Search

        /// <summary>
        /// Search blog posts.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search blog posts.", CacheDuration = 60)]
        public List<TransitAccountBlogPost> SearchAccountBlogPosts(string ticket, string s, ServiceQueryOptions options)
        {
            int maxsearchresults = 128;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            }

            StringBuilder query = new StringBuilder();
            query.Append("SELECT {AccountBlogPost.*} FROM AccountBlogPost {AccountBlogPost}");
            query.AppendFormat(" INNER JOIN FREETEXTTABLE(AccountBlogPost, ([Title], [Body]), '{0}', {1}) AS ft",
                Renderer.SqlEncode(s), maxsearchresults);
            query.Append(" ON AccountBlogPost.AccountBlogPost_Id = ft.[KEY] ");
            query.Append(" ORDER BY ft.[RANK] DESC");

            return WebServiceImpl<TransitAccountBlogPost, ManagedAccountBlogPost, AccountBlogPost>.GetList(
                ticket, options, query.ToString(), "AccountBlogPost");
        }

        /// <summary>
        /// Return the number of blog posts matching a query.
        /// </summary>
        /// <returns>number of blog posts</returns>
        [WebMethod(Description = "Return the number of blog posts matching a query.", CacheDuration = 60)]
        public int SearchAccountBlogPostsCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            return SearchAccountBlogPosts(ticket, s, null).Count;
        }

        #endregion
    }
}