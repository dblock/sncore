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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitDiscussionThread : TransitService<DiscussionThread>
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

        public TransitDiscussionThread(DiscussionThread value)
            : base(value)
        {

        }

        public override void SetInstance(DiscussionThread value)
        {
            Created = value.Created;
            Modified = value.Modified;
            DiscussionId = value.Discussion.Id;
            base.SetInstance(value);
        }

        public override DiscussionThread GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DiscussionThread instance = base.GetInstance(session, sec);
            if (Id == 0)
            {
                instance.Discussion = (Discussion)session.Load(typeof(Discussion), this.DiscussionId);
            }
            return instance;
        }
    }

    public class ManagedDiscussionThread : ManagedService<DiscussionThread, TransitDiscussionThread>
    {
        public ManagedDiscussionThread()
        {

        }

        public ManagedDiscussionThread(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussionThread(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedDiscussionThread(ISession session, DiscussionThread value)
            : base(session, value)
        {

        }

        public List<TransitDiscussionPost> GetDiscussionPosts(ManagedSecurityContext sec)
        {
            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>();
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                if (post.DiscussionPostParent == null)
                {
                    ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                    result.Insert(0, m_post.GetTransitInstance(sec));
                    result.InsertRange(1, m_post.GetPosts(sec));
                }
            }
            return result;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            return acl;
        }

        public void Move(ManagedSecurityContext sec, int targetid)
        {
            GetACL().Check(sec, DataOperation.Delete | DataOperation.Create);

            if (mInstance.Discussion.DiscussionThreads != null)
                mInstance.Discussion.DiscussionThreads.Remove(mInstance);

            mInstance.Discussion = (Discussion) Session.Load(
                typeof(Discussion), targetid);

            if (mInstance.Discussion.DiscussionThreads != null)
                mInstance.Discussion.DiscussionThreads.Add(mInstance);

            Save(sec);
        }
    }
}
