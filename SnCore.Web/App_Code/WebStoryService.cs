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
    /// Managed web story services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebStoryService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebStoryService : WebService
    {

        public WebStoryService()
        {

        }

        /// <summary>
        /// Get account stories.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account stories</returns>
        [WebMethod(Description = "Get account stories.")]
        public List<TransitAccountStory> GetAccountStories(string ticket, ServiceQueryOptions options)
        {
            return GetAccountStoriesById(ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account stories.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account stories</returns>
        [WebMethod(Description = "Get account stories.", CacheDuration = 60)]
        public List<TransitAccountStory> GetAccountStoriesById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria crit = session.CreateCriteria(typeof(AccountStory))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    crit.SetFirstResult(options.FirstResult);
                    crit.SetMaxResults(options.PageSize);
                }

                IList list = crit.List();

                List<TransitAccountStory> result = new List<TransitAccountStory>(list.Count);
                foreach (AccountStory e in list)
                {
                    result.Add(new TransitAccountStory(session, e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account stories count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account stories</returns>
        [WebMethod(Description = "Get account stories count.")]
        public int GetAccountStoriesCount(string ticket)
        {
            return GetAccountStoriesCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account stories count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account stories count</returns>
        [WebMethod(Description = "Get account stories count.", CacheDuration = 60)]
        public int GetAccountStoriesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountStory s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get latest account stories count.
        /// </summary>
        /// <returns>number of account stories</returns>
        [WebMethod(Description = "Get latest account stories count.", CacheDuration = 60)]
        public int GetLatestAccountStoriesCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(s) FROM AccountStory s").UniqueResult();
            }
        }

        /// <summary>
        /// Get latest account stories.
        /// </summary>
        /// <param name="options">query options</param>
        /// <returns>transit account stories</returns>
        [WebMethod(Description = "Get latest account stories.", CacheDuration = 60)]
        public List<TransitAccountStory> GetLatestAccountStories(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountStory))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList list = c.List();

                List<TransitAccountStory> result = new List<TransitAccountStory>(list.Count);
                foreach (AccountStory e in list)
                {
                    result.Add(new TransitAccountStory(session, e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for story
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountStory(session, id).TransitAccountStory;
            }
        }

        /// <summary>
        /// Get account story picture by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">story picture id</param>
        /// <returns>transit account story picture</returns>
        [WebMethod(Description = "Get account story picture by id.")]
        public TransitAccountStoryPicture GetAccountStoryPictureById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for story picture
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountStoryPicture(session, id).TransitAccountStoryPicture;
            }
        }

        /// <summary>
        /// Add a story.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="story">new story</param>
        [WebMethod(Description = "Add a story.")]
        public int AddAccountStory(string ticket, TransitAccountStory story)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                int result = a.CreateOrUpdate(story);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Add a story with pictures.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="story">new story</param>
        [WebMethod(Description = "Add a story with pictures.")]
        public int AddAccountStoryWithPicturesWithPictures(string ticket, TransitAccountStory story, TransitAccountStoryPictureWithPicture[] pictures)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                int result = a.CreateOrUpdateWithPictures(story, pictures);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Add a story with pictures.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="story">new story</param>
        [WebMethod(Description = "Add a story with pictures.")]
        public int AddAccountStoryWithPictures(string ticket, TransitAccountStory story, TransitAccountStoryPicture[] pictures)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                int result = a.CreateOrUpdateWithPictures(story, pictures);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a story.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="storyid">story id</param>
        [WebMethod(Description = "Delete a story.")]
        public void DeleteAccountStory(string ticket, int storyid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountStory s = new ManagedAccountStory(session, storyid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete a story picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="storypictureid">story picture id</param>
        [WebMethod(Description = "Delete a story picture.")]
        public void DeleteAccountStoryPicture(string ticket, int storypictureid)
        {
            int id = ManagedAccount.GetAccountId(ticket);            
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountStoryPicture s = new ManagedAccountStoryPicture(session, storypictureid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Move a story picture up.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="storypictureid">story picture id</param>
        /// <param name="disp">displacement</param>
        [WebMethod(Description = "Move a story picture.")]
        public void MoveAccountStoryPicture(string ticket, int storypictureid, int disp)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountStoryPicture s = new ManagedAccountStoryPicture(session, storypictureid);

                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                s.Move(disp);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get story pictures.
        /// </summary>
        /// <param name="story">story id</param>
        /// <returns>transit story pictures</returns>
        [WebMethod(Description = "Get story pictures.")]
        public List<TransitAccountStoryPicture> GetAccountStoryPictures(string ticket, int storyid)
        {
            return GetAccountStoryPicturesById(storyid);
        }

        /// <summary>
        /// Get story pictures by account id.
        /// </summary>
        /// <param name="storyid">story id</param>
        /// <returns>transit story pictures</returns>
        [WebMethod(Description = "Get story pictures.", CacheDuration = 60)]
        public List<TransitAccountStoryPicture> GetAccountStoryPicturesById(int storyid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountStoryPicture))
                    .Add(Expression.Eq("AccountStory.Id", storyid))
                    .AddOrder(Order.Asc("Location"))
                    .List();

                List<TransitAccountStoryPicture> result = new List<TransitAccountStoryPicture>(list.Count);
                foreach (AccountStoryPicture p in list)
                {
                    result.Add(new ManagedAccountStoryPicture(session, p).TransitAccountStoryPicture);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get story picture picture data.
        /// </summary>
        /// <param name="id">story picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get story picture picture data.", BufferResponse = true)]
        public TransitAccountStoryPictureWithPicture GetAccountStoryPictureWithPictureById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountStoryPicture(session, id).TransitAccountStoryPictureWithPicture;
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
        public TransitAccountStoryPictureWithPicture GetAccountStoryPictureWithPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountStoryPictureWithPicture p = new ManagedAccountStoryPicture(session, id).TransitAccountStoryPictureWithPicture;

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p;
            }
        }

        /// <summary>
        /// Get story picture thumbnail data.
        /// </summary>
        /// <param name="id">story picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit story picture with thumbnail</returns>
        [WebMethod(Description = "Get story picture Thumbnail data.", BufferResponse = true)]
        public TransitAccountStoryPictureWithThumbnail GetAccountStoryPictureWithThumbnailById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountStoryPicture(session, id).TransitAccountStoryPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get story picture thumbnail data if modified since.
        /// </summary>
        /// <param name="id">story picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit story picture with thumbnail</returns>
        [WebMethod(Description = "Get story picture thumbnail data if modified since.", BufferResponse = true)]
        public TransitAccountStoryPictureWithThumbnail GetAccountStoryPictureWithThumbnailIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountStoryPictureWithThumbnail p = new ManagedAccountStoryPicture(session, id).TransitAccountStoryPictureWithThumbnail;

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p;
            }
        }

        #region Search

        /// <summary>
        /// Search stories.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search stories.", CacheDuration = 60)]
        public List<TransitAccountStory> SearchAccountStories(string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {AccountStory.*} FROM AccountStory {AccountStory}" +
                        " WHERE FREETEXT ((Name, Summary), '" + Renderer.SqlEncode(s) + "')",
                        "AccountStory",
                        typeof(AccountStory));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList stories = query.List();

                List<TransitAccountStory> result = new List<TransitAccountStory>(stories.Count);
                foreach (AccountStory story in stories)
                {
                    result.Add(new ManagedAccountStory(session, story).TransitAccountStory);
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of stories matching a query.
        /// </summary>
        /// <returns>number of stories</returns>
        [WebMethod(Description = "Return the number of stories matching a query.", CacheDuration = 60)]
        public int SearchAccountStoriesCount(string s)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                        "SELECT {AccountStory.*} FROM AccountStory {AccountStory}" +
                        " WHERE FREETEXT ((Name, Summary), '" + Renderer.SqlEncode(s) + "')",
                        "AccountStory",
                        typeof(AccountStory));

                return query.List().Count;
            }
        }

        #endregion
    }
}