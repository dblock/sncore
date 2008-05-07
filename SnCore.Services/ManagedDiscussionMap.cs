using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using NHibernate;
using System.Reflection;

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

        private string mThreadUriFormat;

        public string ThreadUriFormat
        {
            get
            {
                return mThreadUriFormat;
            }
            set
            {
                mThreadUriFormat = value;
            }
        }

        private string mDiscussionUriFormat;

        public string DiscussionUriFormat
        {
            get
            {
                return mDiscussionUriFormat;
            }
            set
            {
                mDiscussionUriFormat = value;
            }
        }

        public abstract Type Type { get; }
        public abstract int GetAccountId(ISession session, int id);
        public abstract string GetObjectName(ISession session, int id);
        public abstract ACL GetACL(ISession session, int id);
        public abstract ACL GetACL(ISession session, int id, Type type);

        public ManagedDiscussionMapEntry(string name, string uriformat)
        {
            mName = name;
            mThreadUriFormat = mDiscussionUriFormat = uriformat;
        }

        public ManagedDiscussionMapEntry(string name, string threaduriformat, string discussionuriformat)
        {
            mName = name;
            mThreadUriFormat = threaduriformat;
            mDiscussionUriFormat = discussionuriformat;
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

        public override ACL GetACL(ISession session, int id)
        {
            InstanceType instance = session.Load<InstanceType>(id);
            return mGetACLsDelegate(session, instance, typeof(InstanceType));
        }

        public override ACL GetACL(ISession session, int id, Type type)
        {
            InstanceType instance = session.Load<InstanceType>(id);
            return mGetACLsDelegate(session, instance, type);
        }

        public delegate int GetOwnerIdDelegate(InstanceType instance, int id);
        private GetOwnerIdDelegate mGetOwnerIdDelegate;

        public delegate string GetObjectNameDelegate(InstanceType instance, int id);
        private GetObjectNameDelegate mGetObjectNameDelegate;

        public delegate ACL GetACLsDelegate(ISession session, InstanceType instance, Type type);
        private GetACLsDelegate mGetACLsDelegate;

        public ManagedDiscussionMapEntry(
            GetOwnerIdDelegate ownerdelegate, 
            GetObjectNameDelegate namedelegate, 
            GetACLsDelegate getaclsdelegate,
            string name, 
            string uriformat)
            : this(
            ownerdelegate, 
            namedelegate,
            getaclsdelegate, 
            name, 
            uriformat, 
            uriformat)
        {
        }

        public ManagedDiscussionMapEntry(
            GetOwnerIdDelegate ownerdelegate, 
            GetObjectNameDelegate namedelegate, 
            GetACLsDelegate getaclsdelegate,
            string name, 
            string threaduriformat, 
            string discussionuriformat)
            : base(
            name, 
            threaduriformat, 
            discussionuriformat)
        {
            mGetOwnerIdDelegate = ownerdelegate;
            mGetObjectNameDelegate = namedelegate;
            mGetACLsDelegate = getaclsdelegate;
        }
    };

    public abstract class ManagedDiscussionMap
    {
        #region GetOwnerId

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
        public static int GetOwnerId(AccountAuditEntry instance, int id) { return instance.AccountId; }
        
        public static int GetOwnerId(AccountGroup group, int id) 
        {
            foreach (AccountGroupAccount instance in group.AccountGroupAccounts)
                if (instance.IsAdministrator)
                    return instance.Account.Id;

            throw new Exception(string.Format("Missing Owner: AccountGroup {0}", group.Id)); 
        }

        public static int GetOwnerId(AccountGroupPicture instance, int id) { return instance.Account.Id; }

        #endregion

        #region GetObjectName

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
        public static string GetObjectName(AccountGroup instance, int id) { return instance.Name; }
        public static string GetObjectName(AccountAuditEntry instance, int id) { return "Broadcast"; }

        #endregion

        #region GetACL

        public static ACL GetACL(ISession session, AccountPicture instance, Type type) { return new ManagedAccountPicture(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountStory instance, Type type) { return new ManagedAccountStory(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, Account instance, Type type) { return new ManagedAccount(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, Place instance, Type type) { return new ManagedPlace(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, PlacePicture instance, Type type) { return new ManagedPlacePicture(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountFeedItem instance, Type type) { return new ManagedAccountFeedItem(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountBlogPost instance, Type type) { return new ManagedAccountBlogPost(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountStoryPicture instance, Type type) { return new ManagedAccountStoryPicture(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountEvent instance, Type type) { return new ManagedAccountEvent(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountEventPicture instance, Type type) { return new ManagedAccountEventPicture(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountGroupPicture instance, Type type) { return new ManagedAccountGroupPicture(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountGroup instance, Type type) { return new ManagedAccountGroup(session, instance).GetACL(type); }
        public static ACL GetACL(ISession session, AccountAuditEntry instance, Type type) { return new ManagedAccountAuditEntry(session, instance).GetACL(type); }

        #endregion

        private static Dictionary<Type, ManagedDiscussionMapEntry> s_GlobalTypeMap = null;
        
        private static void CreateTypeMap()
        {
            if (s_GlobalTypeMap != null)
                return;

            lock (GlobalMap)
            {
                if (s_GlobalTypeMap != null)
                    return;

                s_GlobalTypeMap = new Dictionary<Type, ManagedDiscussionMapEntry>();
                
                foreach (ManagedDiscussionMapEntry entry in GlobalMap)
                {
                    s_GlobalTypeMap[entry.Type] = entry;
                }
            }
        }

        public static ManagedDiscussionMapEntry[] GlobalMap =
        {
            new ManagedDiscussionMapEntry<AccountPicture>(GetOwnerId, GetObjectName, GetACL, 
                "Picture Comments", "AccountPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountStory>(GetOwnerId, GetObjectName, GetACL, 
                "Story Comments", "AccountStoryView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<Account>(GetOwnerId, GetObjectName, GetACL, 
                "Testimonials", "AccountView.aspx?id={0}&#testimonials"),
            new ManagedDiscussionMapEntry<Place>(GetOwnerId, GetObjectName, GetACL, 
                "Place Comments", "PlaceView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<PlacePicture>(GetOwnerId, GetObjectName, GetACL, 
                "Place Picture Comments", "PlacePictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountFeedItem>(GetOwnerId, GetObjectName, GetACL, 
                "Feed Entry Comments", "AccountFeedItemView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountBlogPost>(GetOwnerId, GetObjectName, GetACL, 
                "Blog Post Comments", "AccountBlogPostView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountStoryPicture>(GetOwnerId, GetObjectName, GetACL, 
                "Story Picture Comments", "AccountStoryPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountEvent>(GetOwnerId, GetObjectName, GetACL, 
                "Event Comments", "AccountEventView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountEventPicture>(GetOwnerId, GetObjectName, GetACL, 
                "Event Picture Comments", "AccountEventPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountGroupPicture>(GetOwnerId, GetObjectName, GetACL, 
                "Group Picture Comments", "AccountGroupPictureView.aspx?id={0}&#comments"),
            new ManagedDiscussionMapEntry<AccountGroup>(GetOwnerId, GetObjectName, GetACL,
                "Group Discussion", string.Empty, "AccountGroupView.aspx?id={0}"),
            new ManagedDiscussionMapEntry<AccountAuditEntry>(GetOwnerId, GetObjectName, GetACL,
                "Broadcast", string.Empty, "AccountAuditEntry.aspx?id={0}")
        };

        public static ManagedDiscussionMapEntry Find(ISession session, int dataobject_id)
        {
            DataObject dataobject = session.Load<DataObject>(dataobject_id);
            return Find(dataobject);
        }

        public static bool TryFind(ISession session, int dataobject_id, out ManagedDiscussionMapEntry result)
        {
            DataObject dataobject = session.Load<DataObject>(dataobject_id);
            return TryFind(dataobject, out result);
        }

        public static ManagedDiscussionMapEntry Find(DataObject dataobject)
        {
            if (dataobject == null)
            {
                throw new Exception("Missing Discussion Map for a unreferenced data object");
            }

            ManagedDiscussionMapEntry entry;
            if (TryFind(dataobject, out entry))
                return entry;

            throw new Exception(string.Format("Missing Discussion Map for {0}", dataobject.Name));
        }

        public static bool TryFind(DataObject dataobject, out ManagedDiscussionMapEntry result)
        {
            result = null;
            if (dataobject == null) return false;
            CreateTypeMap();
            Type t = Assembly.GetAssembly(typeof(DataObject)).GetType(dataobject.Name, true);
            return s_GlobalTypeMap.TryGetValue(t, out result);
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
            CreateTypeMap();
            return s_GlobalTypeMap.TryGetValue(type, out result);
        }
    }
}
