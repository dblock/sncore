using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Tools.Web;
using SnCore.Data.Hibernate;
using SnCore.Tools;
using NHibernate;
using System.Globalization;

namespace SnCore.Services
{
    public class AccountActivityQueryOptions
    {
        public string SortOrder = "LastLogin";
        public bool SortAscending = false;
        public bool PicturesOnly = false;
        public bool BloggersOnly = false;
        public bool VerifiedOnly = true;
        public string Country;
        public string State;
        public string City;
        public string Name;
        public string Email;

        public AccountActivityQueryOptions()
        {

        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Account.City LIKE '{0}'", City);
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Account.Country.Name = '{0}'", Country);
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Account.State.Name = '{0}'", State);
            }

            if (PicturesOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("EXISTS ( FROM AccountPicture ap WHERE ap.Account = Account AND ap.Hidden = 0 )");
            }

            if (BloggersOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("(EXISTS ( FROM AccountFeed af WHERE af.Account = Account ) OR EXISTS ( FROM AccountBlog ab WHERE ab.Account = Account ))");
            }

            if (VerifiedOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("(EXISTS ( FROM AccountEmail ae WHERE ae.Account = Account AND ae.Verified = 1 ))");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                if (Name.Length == 1)
                {
                    b.AppendFormat("Account.Name LIKE '{0}%'", Renderer.SqlEncode(Name));
                }
                else
                {
                    b.AppendFormat("Account.Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
                }
            }

            if (!string.IsNullOrEmpty(Email))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("(EXISTS ( FROM AccountEmail ae WHERE ae.Account = Account AND ae.Address = '{0}'))", Renderer.SqlEncode(Email));
            }

            // delay accounts, prevent bots from pushing accounts on top
            // and avoid privacy violation of showing someone online
            if (SortOrder == "LastLogin")
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Account.LastLogin < '{0}'", 
                    DateTime.UtcNow.AddMinutes(-15).ToString(DateTimeFormatInfo.InvariantInfo));
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT Account FROM Account Account ");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }
            return b.ToString();
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountActivity : TransitAccount
    {
        private int mNewFriends = 0;

        public int NewFriends
        {
            get
            {
                return mNewFriends;
            }
            set
            {
                mNewFriends = value;
            }
        }

        private int mNewPictures = 0;

        public int NewPictures
        {
            get
            {
                return mNewPictures;
            }
            set
            {
                mNewPictures = value;
            }
        }

        private int mNewDiscussionPosts = 0;

        public int NewDiscussionPosts
        {
            get
            {
                return mNewDiscussionPosts;
            }
            set
            {
                mNewDiscussionPosts = value;
            }
        }

        private int mNewSyndicatedContent = 0;

        public int NewSyndicatedContent
        {
            get
            {
                return mNewSyndicatedContent;
            }
            set
            {
                mNewSyndicatedContent = value;
            }
        }

        public TransitAccountActivity()
        {

        }

        public TransitAccountActivity(Account o)
            : base(o)
        {

        }

        public override void SetInstance(Account value)
        {
            base.SetInstance(value);
        }

        public override Account GetInstance(ISession session, ManagedSecurityContext sec)
        {
            return base.GetInstance(session, sec);
        }

        protected long Count
        {
            get
            {
                return NewPictures + NewDiscussionPosts + NewSyndicatedContent + NewFriends;
            }
        }

        public static int CompareByLastActivity(TransitAccountActivity left, TransitAccountActivity right)
        {
            return right.Count.CompareTo(left.Count);
        }

        public static int CompareByLastLogin(TransitAccountActivity left, TransitAccountActivity right)
        {
            return TransitAccount.CompareByLastLogin(left, right);
        }
    };

    public class ManagedAccountActivity : ManagedService<Account, TransitAccountActivity>
    {
        public override TransitAccountActivity GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountActivity t_instance = base.GetTransitInstance(sec);

            DateTime limit = DateTime.UtcNow.AddDays(-14);

            // new photos (count one week of photos)

            t_instance.NewPictures = Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountPicture p " +
                "WHERE p.Account.Id = {0} AND p.Modified > '{1}' AND p.Hidden = 0",
                mInstance.Id, limit.ToString(DateTimeFormatInfo.InvariantInfo)))
                .UniqueResult<int>();

            t_instance.NewDiscussionPosts = Session.CreateQuery(string.Format("SELECT COUNT(*) FROM DiscussionPost p, Discussion d, DiscussionThread t " +
                "WHERE p.AccountId = {0} AND p.Modified > '{1}' AND p.DiscussionThread.Id = t.Id and t.Discussion.Id = d.Id AND d.Personal = 0",
                mInstance.Id, limit.ToString(DateTimeFormatInfo.InvariantInfo)))
                .UniqueResult<int>();

            t_instance.NewSyndicatedContent = Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountFeed f " +
                "WHERE f.Account.Id = {0} AND f.Created > '{1}'",
                mInstance.Id, limit.ToString(DateTimeFormatInfo.InvariantInfo)))
                .UniqueResult<int>();

            t_instance.NewFriends = Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountFriend f " +
                "WHERE (f.Account.Id = {0} OR f.Keen.Id = {0}) AND (f.Created > '{1}')",
                mInstance.Id, limit.ToString(DateTimeFormatInfo.InvariantInfo)))
                .UniqueResult<int>();

            return t_instance;
        }

        public ManagedAccountActivity()
        {

        }

        public ManagedAccountActivity(ISession session)
            : base(session)
        {

        }

        public ManagedAccountActivity(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountActivity(ISession session, Account value)
            : base(session, value)
        {

        }

        public TransitAccountActivity GetTransformedInstanceFromAccountFriend(ISession session, ManagedSecurityContext sec, AccountFriend friend)
        {
            ManagedAccountActivity m_instance = new ManagedAccountActivity();
            m_instance.SetInstance(session, (friend.Account.Id != mInstance.Id) ? friend.Account : friend.Keen);
            return m_instance.GetTransitInstance(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
