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
using System.Collections.Generic;

namespace SnCore.Services
{
    public class TransitDiscussionThread : TransitService
    {
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

        private int mDiscussionId;

        public int DiscussionId
        {
            get
            {

                return mDiscussionId;
            }
            set
            {
                mDiscussionId = value;
            }
        }

        public TransitDiscussionThread()
        {

        }

        public TransitDiscussionThread(DiscussionThread d)
            : base(d.Id)
        {
            Created = d.Created;
            Modified = d.Modified;
            DiscussionId = d.Discussion.Id;
        }

        public DiscussionThread GetDiscussionThread(ISession session)
        {
            DiscussionThread d = (Id != 0) ? (DiscussionThread)session.Load(typeof(DiscussionThread), Id) : new DiscussionThread();
            if (Id == 0)
            {
                d.Discussion = (Discussion)session.Load(typeof(Discussion), this.DiscussionId);
            }
            return d;
        }
    }

    /// <summary>
    /// Managed discussion thread.
    /// </summary>
    public class ManagedDiscussionThread : ManagedService<DiscussionThread>
    {
        private DiscussionThread mDiscussionThread = null;

        public ManagedDiscussionThread(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussionThread(ISession session, int id)
            : base(session)
        {
            mDiscussionThread = (DiscussionThread)session.Load(typeof(DiscussionThread), id);
        }

        public ManagedDiscussionThread(ISession session, DiscussionThread value)
            : base(session)
        {
            mDiscussionThread = value;
        }

        public int Id
        {
            get
            {
                return mDiscussionThread.Id;
            }
        }

        public List<TransitDiscussionPost> GetPosts()
        {
            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>();
            foreach (DiscussionPost post in mDiscussionThread.DiscussionPosts)
            {
                if (post.DiscussionPostParent == null)
                {
                    ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                    result.Insert(0, m_post.GetTransitDiscussionPost());
                    result.InsertRange(1, m_post.GetPosts());
                }
            }
            return result;
        }

        public void Create(TransitDiscussionThread c)
        {
            mDiscussionThread = c.GetDiscussionThread(Session);
            mDiscussionThread.Modified = DateTime.UtcNow;
            if (mDiscussionThread.Id == 0) mDiscussionThread.Created = mDiscussionThread.Modified;
            Session.Save(mDiscussionThread);
        }

        public void Delete()
        {
            Session.Delete(mDiscussionThread);
        }
    }
}
