using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using NHibernate;

namespace SnCore.Services
{
    public abstract class ManagedDiscussionMapEntry
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

        private string mPublicUriFormat;

        public string PublicUriFormat
        {
            get
            {
                return mPublicUriFormat;
            }
            set
            {
                mPublicUriFormat = value;
            }
        }

        public abstract Type Type { get; }
        public abstract int GetAccountId(ISession session, int id);
        public abstract string GetObjectName(ISession session, int id);

        public ManagedDiscussionMapEntry(string name, string uriformat)
        {
            mName = name;
            mPublicUriFormat = uriformat;
        }
    }

    public class ManagedDiscussionMapEntry<InstanceType> : ManagedDiscussionMapEntry
        where InstanceType : IDbObject
    {
        public override Type Type
        {
            get
            {
                return typeof(InstanceType);
            }
        }

        public override int GetAccountId(ISession session, int id)
        {
            InstanceType instance = session.Load<InstanceType>(id);
            return mGetOwnerIdDelegate(instance, id);
        }

        public override string GetObjectName(ISession session, int id)
        {
            InstanceType instance = session.Load<InstanceType>(id);
            return mGetObjectNameDelegate(instance, id);
        }

        public delegate int GetOwnerIdDelegate(InstanceType instance, int id);
        private GetOwnerIdDelegate mGetOwnerIdDelegate;

        public delegate string GetObjectNameDelegate(InstanceType instance, int id);
        private GetObjectNameDelegate mGetObjectNameDelegate;

        public ManagedDiscussionMapEntry(GetOwnerIdDelegate ownerdelegate, GetObjectNameDelegate namedelegate, string name, string uriformat)
            : base(name, uriformat)
        {
            mGetOwnerIdDelegate = ownerdelegate;
            mGetObjectNameDelegate = namedelegate;
        }
    };

    public abstract class ManagedDiscussionMap
    {
        public static int GetOwnerId(AccountPicture instance, int id) { return instance.Account.Id; }
        public static int GetOwnerId(AccountStory instance, int id) { return instance.Account.Id; }
        public static int GetOwnerId(Account instance, int id) { return instance.Id; }
        public static int GetOwnerId(Place instance, int id) { return instance.Account.Id; }
        public static int GetOwnerId(PlacePicture instance, int id) { return instance.Place.Account.Id; }
        public static int GetOwnerId(AccountFeedItem instance, int id) { return instance.AccountFeed.Account.Id; }
        public static int GetOwnerId(AccountBlogPost instance, int id) { return instance.AccountBlog.Account.Id; }
        public static int GetOwnerId(AccountStoryPicture instance, int id) { return instance.AccountStory.Account.Id; }
        public static int GetOwnerId(AccountEvent instance, int id) { return instance.Account.Id; }
        public static int GetOwnerId(AccountEventPicture instance, int id) { return instance.AccountEvent.Account.Id; }
        public static int GetOwnerId(AccountGroupPicture instance, int id) { return instance.Account.Id; }

        public static string GetObjectName(AccountPicture instance, int id) { return string.Format("Profile Pictures: {0}", instance.Name); }
        public static string GetObjectName(AccountStory instance, int id) { return instance.Name; }
        public static string GetObjectName(Account instance, int id) { return "Testimonials"; }
        public static string GetObjectName(Place instance, int id) { return instance.Name; }
        public static string GetObjectName(PlacePicture instance, int id) { return string.Format("{0} Pictures: {1}", instance.Place.Name, instance.Name); }
        public static string GetObjectName(AccountFeedItem instance, int id) { return string.Format("{0}: {1}", instance.AccountFeed.Name, instance.Title); }
        public static string GetObjectName(AccountBlogPost instance, int id) { return string.Format("{0}: {1}", instance.AccountBlog.Name, instance.Title); }
        public static string GetObjectName(AccountStoryPicture instance, int id) { return string.Format("{0} Pictures: {1}", instance.AccountStory.Name, instance.Name); }
        public static string GetObjectName(AccountEvent instance, int id) { return instance.Name; }
        public static string GetObjectName(AccountEventPicture instance, int id) { return string.Format("{0} Pictures: {1}", instance.AccountEvent.Name, instance.Name); }
        public static string GetObjectName(AccountGroupPicture instance, int id) { return string.Format("{0} Pictures: {1}", instance.AccountGroup.Name, instance.Name); }

        public static ManagedDiscussionMapEntry[] GlobalMap =
        {
            new ManagedDiscussionMapEntry<AccountPicture>(GetOwnerId, GetObjectName, "Picture Comments", "AccountPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountStory>(GetOwnerId, GetObjectName, "Story Comments", "AccountStoryView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<Account>(GetOwnerId, GetObjectName, "Testimonials", "AccountView.aspx?id={0}&#testimonials"),
            new ManagedDiscussionMapEntry<Place>(GetOwnerId, GetObjectName, "Place Comments", "PlaceView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<PlacePicture>(GetOwnerId, GetObjectName, "Place Picture Comments", "PlacePictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountFeedItem>(GetOwnerId, GetObjectName, "Feed Entry Comments", "AccountFeedItemView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountBlogPost>(GetOwnerId, GetObjectName, "Blog Post Comments", "AccountBlogPostView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountStoryPicture>(GetOwnerId, GetObjectName, "Story Picture Comments", "AccountStoryPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountEvent>(GetOwnerId, GetObjectName, "Event Comments", "AccountEventView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountEventPicture>(GetOwnerId, GetObjectName, "Event Picture Comments", "AccountEventPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountGroupPicture>(GetOwnerId, GetObjectName, "Group Picture Comments", "AccountGroupPictureView.aspx?id={0}&#comments")
        };

        public static ManagedDiscussionMapEntry Find(string name)
        {
            ManagedDiscussionMapEntry entry;
            if (TryFind(name, out entry))
                return entry;

            throw new Exception(string.Format("Missing Discussion Map for {0}", name));
        }

        public static bool TryFind(string name, out ManagedDiscussionMapEntry result)
        {
            result = null;

            foreach (ManagedDiscussionMapEntry entry in GlobalMap)
            {
                if (entry.Name == name)
                {
                    result = entry;
                    return true;
                }
            }

            return false;
        }

        public static ManagedDiscussionMapEntry Find(Type type)
        {
            ManagedDiscussionMapEntry entry;
            if (TryFind(type, out entry))
                return entry;

            throw new Exception(string.Format("Missing Discussion Map for {0}", type.Name));
        }

        public static bool TryFind(Type type, out ManagedDiscussionMapEntry result)
        {
            result = null;

            foreach (ManagedDiscussionMapEntry entry in GlobalMap)
            {
                if (entry.Type == type)
                {
                    result = entry;
                    return true;
                }
            }

            return false;
        }
    }
}
