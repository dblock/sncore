using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class DiscussionQueryOptions
    {
        public int AccountId;
        public bool TopOfThreadOnly = true;

        public string CountQuery
        {
            get
            {
                if (TopOfThreadOnly)
                {
                    return "SELECT COUNT(tp) FROM DiscussionPost tp, DiscussionThread t, Discussion d" +
                           " WHERE tp.AccountId = " + AccountId.ToString() +
                           " AND t.Discussion.Id = d.Id" +
                           " AND d.Personal = 0" +
                           " AND t.Id = tp.DiscussionThread.Id" +
                           " AND tp.DiscussionPostParent IS NULL";
                }
                else
                {
                    return "SELECT COUNT(tp) FROM DiscussionThread t, DiscussionPost tp, Discussion d" +
                           " WHERE EXISTS(" +
                           "  SELECT p FROM DiscussionPost p WHERE p.DiscussionThread.Id = t.Id" +
                           "  AND p.AccountId = " + AccountId.ToString() + ")" +
                           " AND t.Discussion.Id = d.Id" +
                           " AND d.Personal = 0" +
                           " AND t.Id = tp.DiscussionThread.Id" +
                           " AND tp.DiscussionPostParent IS NULL";
                }
            }
        }

        public string SelectQuery
        {
            get
            {
                if (TopOfThreadOnly)
                {
                    return "SELECT tp FROM DiscussionPost tp, DiscussionThread t, Discussion d" +
                           " WHERE tp.AccountId = " + AccountId.ToString() +
                           " AND tp.DiscussionPostParent IS NULL" +
                           " AND t.Discussion.Id = d.Id" +
                           " AND d.Personal = 0" +
                           " AND t.Id = tp.DiscussionThread.Id" +
                           " AND tp.DiscussionPostParent IS NULL" +
                           " ORDER BY t.Created DESC";
                }
                else
                {
                    return "SELECT tp FROM DiscussionThread t, DiscussionPost tp, Discussion d" +
                         " WHERE EXISTS(" +
                         "  SELECT p FROM DiscussionPost p WHERE p.DiscussionThread.Id = t.Id" +
                         "  AND p.AccountId = " + AccountId.ToString() + ")" +
                         " AND t.Discussion.Id = d.Id" +
                         " AND d.Personal = 0" +
                         " AND t.Id = tp.DiscussionThread.Id" +
                         " AND tp.DiscussionPostParent IS NULL" +
                         " ORDER BY t.Created DESC";
                }
            }
        }
    }

    public class TransitDiscussion : TransitService
    {
        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        private int mAccountId;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private int mObjectId;

        public int ObjectId
        {
            get
            {

                return mObjectId;
            }
            set
            {
                mObjectId = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        private bool mPersonal;

        public bool Personal
        {
            get
            {

                return mPersonal;
            }
            set
            {
                mPersonal = value;
            }
        }

        private int mPostCount;

        public int PostCount
        {
            get
            {

                return mPostCount;
            }
            set
            {
                mPostCount = value;
            }
        }

        private int mThreadCount;

        public int ThreadCount
        {
            get
            {

                return mThreadCount;
            }
            set
            {
                mThreadCount = value;
            }
        }

        public TransitDiscussion()
        {

        }

        public TransitDiscussion(ISession session, Discussion d)
            : base(d.Id)
        {
            Name = d.Name;
            Description = d.Description;
            AccountId = d.Account.Id;
            Created = d.Created;
            Modified = d.Modified;
            Personal = d.Personal;
            ObjectId = d.ObjectId;
            PostCount = ManagedDiscussion.GetDiscussionPostCount(session, d.Id);
            ThreadCount = ManagedDiscussion.GetDiscussionThreadCount(session, d.Id);
        }

        public Discussion GetDiscussion(ISession session)
        {
            Discussion d = (Id != 0) ? (Discussion)session.Load(typeof(Discussion), Id) : new Discussion();

            if (Id == 0)
            {
                if (AccountId > 0) d.Account = (Account)session.Load(typeof(Account), this.AccountId);
                d.Personal = this.Personal;
                d.ObjectId = this.ObjectId;
            }

            d.Name = this.Name;
            d.Description = this.Description;
            return d;
        }
    }

    /// <summary>
    /// Managed discussion.
    /// </summary>
    public class ManagedDiscussion : ManagedService
    {
        public const string AccountPictureDiscussion = "Picture Comments";
        public const string AccountStoryDiscussion = "Story Comments";
        public const string AccountStoryPictureDiscussion = "Story Picture Comments";
        public const string AccountTagsDiscussion = "Testimonials";
        public const string AccountFeedItemDiscussion = "Feed Entry Comments";
        public const string PlaceDiscussion = "Place Comments";
        public const string PlacePictureDiscussion = "Place Picture Comments";
        public const string AccountBlogPostDiscussion = "Blog Post Comments";
        public const string AccountEventDiscussion = "Event Comments";
        public const string AccountEventPictureDiscussion = "Event Picture Comments";

        public class DiscussionNotFoundException : SoapException
        {
            public DiscussionNotFoundException()
                : base("Discussion not found", SoapException.ClientFaultCode)
            {

            }
        }

        private Discussion mDiscussion = null;

        public ManagedDiscussion(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussion(ISession session, int id)
            : base(session)
        {
            mDiscussion = (Discussion)session.Load(typeof(Discussion), id);
        }

        public ManagedDiscussion(ISession session, Discussion value)
            : base(session)
        {
            mDiscussion = value;
        }

        public int Id
        {
            get
            {
                return mDiscussion.Id;
            }
        }

        public string Name
        {
            get
            {
                return mDiscussion.Name;
            }
        }

        public string Description
        {
            get
            {
                return mDiscussion.Description;
            }
        }

        public int AccountId
        {
            get
            {
                return mDiscussion.Account.Id;
            }
        }

        public bool Personal
        {
            get
            {
                return mDiscussion.Personal;
            }
        }

        public TransitDiscussion TransitDiscussion
        {
            get
            {
                return new TransitDiscussion(Session, mDiscussion);
            }
        }

        public void Create(
            string name,
            string description,
            int accountid,
            bool personal)
        {
            TransitDiscussion t = new TransitDiscussion();
            t.Name = name;
            t.Description = description;
            t.AccountId = accountid;
            t.Personal = personal;
            t.Created = t.Modified = DateTime.UtcNow;
            Create(t);
        }

        public void Create(TransitDiscussion c)
        {
            mDiscussion = c.GetDiscussion(Session);
            mDiscussion.Modified = DateTime.UtcNow;
            if (mDiscussion.Id == 0) mDiscussion.Created = mDiscussion.Modified;
            Session.Save(mDiscussion);
        }

        public void Delete()
        {
            Session.Delete(mDiscussion);
        }

        public int CreatePost(
            int accountid,
            int parentid,
            string subject,
            string body)
        {
            DiscussionThread thread = null;
            DiscussionPost parent = null;

            if (parentid != 0)
            {
                parent = (DiscussionPost)Session.Load(typeof(DiscussionPost), parentid);
                if (parent == null)
                {
                    throw new ArgumentException();
                }

                thread = parent.DiscussionThread;
                if (thread.Discussion.Id != Id)
                {
                    throw new ArgumentException();
                }

                thread.Modified = DateTime.UtcNow;
                Session.Save(thread);
            }
            else
            {
                thread = new DiscussionThread();
                thread.Created = thread.Modified = DateTime.UtcNow;
                thread.Discussion = mDiscussion;
                Session.Save(thread);

                if (mDiscussion.DiscussionThreads == null) mDiscussion.DiscussionThreads = new ArrayList();
                mDiscussion.DiscussionThreads.Add(thread);
            }

            DiscussionPost result = new DiscussionPost();
            result.AccountId = accountid;
            result.Created = result.Modified = thread.Modified;
            result.DiscussionPostParent = parent;
            result.DiscussionThread = thread;
            result.Subject = subject;
            result.Body = body;
            Session.Save(result);
            Session.Flush();

            if (thread.DiscussionPosts == null) thread.DiscussionPosts = new ArrayList();
            thread.DiscussionPosts.Add(result);

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, accountid);
                ManagedAccount ma = new ManagedAccount(Session, parent != null ? parent.AccountId : thread.Discussion.Account.Id);

                if (ra.Id != ma.Id)
                {
                    string replyTo = ma.ActiveEmailAddress;
                    if (replyTo != null)
                    {
                        string partialurl = string.Format(
                            "DiscussionThreadView.aspx?id={0}",
                            thread.Id);

                        string url = string.Format(
                            "{0}/{1}",
                            ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                            partialurl);

                        string replyurl = string.Format(
                            "{0}/DiscussionPost.aspx?did={1}&pid={2}&ReturnUrl={3}&#edit",
                            ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                            Id,
                            result.Id,
                            partialurl);

                        string messagebody =
                            "<html>" +
                            "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                            "<body>" +
                            "<b>from:</b> " + Renderer.Render(ra.Name) +
                            "<br><b>subject:</b> " + Renderer.Render(result.Subject) +
                            "<br><b>posted:</b> " + result.Created.ToString() +
                            "<br><br>" +
                            Renderer.RenderEx(result.Body) +
                            "<blockquote>" +
                            "<a href=\"" + url + "\">Full Thread</a> or <a href=\"" + replyurl + "\">Reply</a>" +
                            "</body>" +
                            "</html>";

                        ra.SendAccountMailMessage(
                        string.Format("{0} <noreply@{1}>",
                            Renderer.Render(ra.Name),
                            ManagedConfiguration.GetValue(Session, "SnCore.Domain", "vestris.com")),
                            replyTo,
                            string.Format("{0}: {1} has posted a new message for you.",
                                ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                                ra.Name),
                            messagebody,
                            true);
                    }
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }

            return result.Id;
        }

        public static int GetDiscussionThreadCount(ISession session, int accountid, string name, int objectid)
        {
            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", (objectid == 0) ? null : (object)objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion == null)
                return 0;

            return GetDiscussionThreadCount(session, existingtagdiscussion.Id);
        }

        public static int GetDiscussionPostCount(ISession session, int accountid, string name, int objectid)
        {
            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", (objectid == 0) ? null : (object)objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion == null)
                return 0;

            return GetDiscussionPostCount(session, existingtagdiscussion.Id);
        }

        public static int GetDiscussionPostCount(ISession session, int discussionid)
        {
            return (int)session.CreateQuery(
                string.Format(
                    "SELECT COUNT(p)" +
                    " FROM DiscussionPost p, DiscussionThread t" +
                    " WHERE p.DiscussionThread.Id = t.Id" +
                    " AND t.Discussion.Id = {0}",
                    discussionid.ToString())).UniqueResult();
        }

        public static int GetDiscussionThreadCount(ISession session, int discussionid)
        {
            return (int)session.CreateQuery(
                string.Format(
                    "SELECT COUNT(t)" +
                    " FROM DiscussionThread t " +
                    "WHERE t.Discussion.Id = {0}",
                    discussionid)).UniqueResult();
        }

        public static int GetDiscussionId(ISession session, int accountid, string name, int objectid, bool createonerror)
        {
            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion != null)
            {
                return existingtagdiscussion.Id;
            }
            else if (createonerror)
            {
                TransitDiscussion td = new TransitDiscussion();
                td.AccountId = accountid;
                td.Name = name;
                td.Personal = true;
                td.Description = string.Empty;
                td.Created = td.Modified = DateTime.UtcNow;
                td.ObjectId = objectid;
                ManagedDiscussion d = new ManagedDiscussion(session);
                d.Create(td);
                return d.Id;
            }
            else
            {
                throw new ManagedDiscussion.DiscussionNotFoundException();
            }
        }
    }
}
