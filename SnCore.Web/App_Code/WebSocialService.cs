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
using System.Web.Services.Protocols;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web social services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebSocialService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebSocialService : WebService
    {
        public WebSocialService()
        {

        }

        /// <summary>
        /// Get new accounts.
        /// </summary>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get new accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetNewAccounts(int max)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateQuery(
                    "FROM Account acct WHERE EXISTS ELEMENTS(acct.AccountPictures) ORDER BY acct.Created DESC")
                    .SetMaxResults(max)
                    .List();

                List<TransitAccount> result = new List<TransitAccount>(list.Count);
                foreach (Account a in list)
                {
                    result.Add(new ManagedAccount(session, a).TransitAccount);
                }

                return result;
            }
        }

        /// <summary>
        /// Get active accounts.
        /// </summary>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get active accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetActiveAccounts(int max)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateQuery(
                    "FROM Account acct WHERE EXISTS ELEMENTS(acct.AccountPictures) ORDER BY acct.LastLogin DESC")
                    .SetMaxResults(max)
                    .List();

                List<TransitAccount> result = new List<TransitAccount>(list.Count);
                foreach (Account a in list)
                {
                    result.Add(new ManagedAccount(session, a).TransitAccount);
                }

                return result;
            }
        }

        /// <summary>
        /// Get accounts count.
        /// </summary>
        /// <returns>accounts count</returns>
        [WebMethod(Description = "Get accounts count.", CacheDuration = 60)]
        public int GetAccountsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(a) FROM Account a").UniqueResult();
            }
        }

        /// <summary>
        /// Get account activity count.
        /// </summary>
        /// <returns>transit account activity count</returns>
        [WebMethod(Description = "Get account activity count.", CacheDuration = 60)]
        public int GetAccountActivityCount(AccountActivityQueryOptions queryoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) queryoptions.CreateCountQuery(session).UniqueResult();
            }
        }

        /// <summary>
        /// Get account activity.
        /// </summary>
        /// <returns>transit account activity</returns>
        [WebMethod(Description = "Get account activity.", CacheDuration = 60)]
        public List<TransitAccountActivity> GetAccountActivity(
            AccountActivityQueryOptions queryoptions, 
            ServiceQueryOptions serviceoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery q = queryoptions.CreateQuery(session);

                if (serviceoptions != null)
                {
                    q.SetMaxResults(serviceoptions.PageSize);
                    q.SetFirstResult(serviceoptions.PageNumber * serviceoptions.PageSize);
                }

                IList list = q.List();

                List<TransitAccountActivity> result = new List<TransitAccountActivity>(list.Count);
                foreach (Account a in list)
                {
                    result.Add(new ManagedAccount(session, a).TransitAccountActivity);
                }

                return result;
            }
        }

        /// <summary>
        /// Get n-th degree count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of n-th degree contacts</returns>
        [WebMethod(Description = "Get n-th degree count.", CacheDuration = 60)]
        public int GetNDegreeCount(string ticket, int deg)
        {
            return GetNDegreeCountById(ManagedAccount.GetAccountId(ticket), deg);
        }

        /// <summary>
        /// Get n-th degree count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of n-th degree contacts</returns>
        [WebMethod(Description = "Get n-th degree count.", CacheDuration = 60)]
        public int GetNDegreeCountById(int accountid, int deg)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, accountid);
                return acct.GetNDegreeCount(deg);
            }
        }

        /// <summary>
        /// Get 1st degree count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of 1st degree contacts</returns>
        [WebMethod(Description = "Get first degree count.", CacheDuration = 60)]
        public int GetFirstDegreeCount(string ticket)
        {
            return GetFirstDegreeCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get 1st degree count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of 1st degree contacts</returns>
        [WebMethod(Description = "Get 1st degree count.", CacheDuration = 60)]
        public int GetFirstDegreeCountById(int accountid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(f) FROM AccountFriend f WHERE (f.Account.Id = {0} OR f.Keen.Id = {0}) ",
                    accountid)).UniqueResult();
            }
        }

        /// <summary>
        /// Get friends activity.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account activity</returns>
        [WebMethod(Description = "Get friends activity.", CacheDuration = 60)]
        public List<TransitAccountActivity> GetFriendsActivity(string ticket, ServiceQueryOptions options)
        {
            return GetFriendsActivityById(ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get friends activity count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account activity acount</returns>
        [WebMethod(Description = "Get friends activity count.", CacheDuration = 60)]
        public int GetFriendsActivityCount(string ticket)
        {
            return GetFriendsActivityCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get friends activity count.
        /// </summary>
        /// <returns>transit account activity count</returns>
        [WebMethod(Description = "Get friends activity count.", CacheDuration = 60)]
        public int GetFriendsActivityCountById(int accountid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(f) FROM AccountFriend f WHERE (f.Account.Id = {0} OR f.Keen.Id = {0}) ",
                    accountid)).UniqueResult();
            }
        }

        /// <summary>
        /// Get friends activity.
        /// </summary>
        /// <returns>transit account activity</returns>
        [WebMethod(Description = "Get friends activity.", CacheDuration = 60)]
        public List<TransitAccountActivity> GetFriendsActivityById(int accountid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(string.Format(
                    "SELECT f FROM AccountFriend f WHERE (f.Account.Id = {0} OR f.Keen.Id = {0}) ", 
                    accountid));

                if (options != null)
                {
                    query.SetMaxResults(options.PageSize);
                    query.SetFirstResult(options.FirstResult);
                }

                IList list = query.List();

                List<TransitAccountActivity> result = new List<TransitAccountActivity>(list.Count);

                foreach (AccountFriend friend in list)
                {
                    result.Add((friend.Account.Id != accountid)
                        ? new ManagedAccount(session, friend.Account).TransitAccountActivity
                        : new ManagedAccount(session, friend.Keen).TransitAccountActivity);
                }

                result.Sort(TransitAccountActivity.CompareByLastActivity);

                return result;
            }
        }


        /// <summary>
        /// Create a new friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="friendid">target friend account id</param>
        /// <param name="message">optional message to send with the request</param>
        /// <returns>request id</returns>
        [WebMethod(Description = "Create a new friend request.")]
        public int CreateAccountFriendRequest(string ticket, int friendid, string message)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            if (friendid == userid)
            {
                throw new SoapException(
                    "You cannot be friends with yourself.", 
                    SoapException.ClientFaultCode);
            }

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, userid);

                if (!acct.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                int result = acct.CreateAccountFriendRequest(friendid, message);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Accept a friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">request id</param>
        /// <param name="message">optional message</param>
        [WebMethod(Description = "Accept a friend request.")]
        public void AcceptAccountFriendRequest(string ticket, int id, string message)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountFriendRequest req;
                try
                {
                    req = new ManagedAccountFriendRequest(session, id);
                }
                catch (NHibernate.ObjectNotFoundException)
                {
                    throw new SoapException("This friend request cannot be found. You may have already accepted it.",
                        SoapException.ClientFaultCode);
                }

                ManagedAccount user = new ManagedAccount(session, userid);

                if (req.KeenId != userid && ! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                req.Accept(message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Reject a friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">request id</param>
        /// <param name="message">optional message, no mail sent when blank</param>
        [WebMethod(Description = "Reject a friend request.")]
        public void RejectAccountFriendRequest(string ticket, int id, string message)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountFriendRequest req;
                try
                {
                    req = new ManagedAccountFriendRequest(session, id);
                }
                catch (NHibernate.ObjectNotFoundException)
                {
                    throw new SoapException("This friend request cannot be found. You may have already rejected it.",
                        SoapException.ClientFaultCode);
                }

                ManagedAccount user = new ManagedAccount(session, userid);

                if (req.KeenId != userid && ! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                req.Reject(message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get total incoming friend requests.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request count.")]
        public int GetAccountFriendRequestsCount(string ticket)
        {
            return GetAccountFriendRequestsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get total friend requests sent.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request sent count.")]
        public int GetAccountFriendRequestsSentCount(string ticket)
        {
            return GetAccountFriendRequestsSentCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get total friend requests sent count by account id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request sent count by account id.", CacheDuration = 60)]
        public int GetAccountFriendRequestsSentCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                return (int)session.CreateQuery(
                    string.Format(
                        "select count(*)" +
                        " from AccountFriendRequest r" +
                        " where r.Account.Id = {0}",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get total friend requests count by account id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request count by account id.", CacheDuration = 60)]
        public int GetAccountFriendRequestsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                return (int)session.CreateQuery(
                    string.Format(
                        "select count(*)" +
                        " from AccountFriendRequest r" +
                        " where r.Keen.Id = {0} and r.Rejected = 0",
                        id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get sent friend requests.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get sent friend requests.")]
        public List<TransitAccountFriendRequest> GetAccountFriendRequestsSent(string ticket, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountFriendRequest))
                    .Add(Expression.Eq("Account.Id", userid));
 
                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountFriendRequest> result = new List<TransitAccountFriendRequest>(list.Count);
                foreach (AccountFriendRequest request in list)
                {
                    result.Add(new TransitAccountFriendRequest(request));
                }
                return result;
            }
        }

        /// <summary>
        /// Get outstanding friend requests.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get outstanding friend requests.")]
        public List<TransitAccountFriendRequest> GetAccountFriendRequests(string ticket, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountFriendRequest))
                    .Add(Expression.Eq("Keen.Id", userid))
                    .Add(Expression.Eq("Rejected", false));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountFriendRequest> result = new List<TransitAccountFriendRequest>(list.Count);
                foreach (AccountFriendRequest request in list)
                {
                    result.Add(new TransitAccountFriendRequest(request));
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">friend request id</param>
        [WebMethod(Description = "Delete a friend request.")]
        public void DeleteAccountFriendRequest(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountFriendRequest request = new ManagedAccountFriendRequest(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (request.AccountId != userid && ! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                request.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete a friend.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">friend id</param>
        [WebMethod(Description = "Delete a friend.")]
        public void DeleteAccountFriend(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountFriend friend = new ManagedAccountFriend(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (friend.AccountId != userid && friend.KeenId != userid && ! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                friend.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get friends count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friends count.")]
        public int GetFriendsCount(string ticket)
        {
            return GetFriendsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get friends count.
        /// </summary>
        [WebMethod(Description = "Get friends count.")]
        public int GetFriendsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format("SELECT COUNT(f) FROM AccountFriend f " +
                        "WHERE (f.Account.Id = {0} OR f.Keen.Id = {0})", id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get friends.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friends.")]
        public List<TransitAccountFriend> GetFriends(string ticket, ServiceQueryOptions options)
        {
            return GetFriendsById(ticket, ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get friends.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account id">account id</param>
        [WebMethod(Description = "Get friends.", CacheDuration = 60)]
        public List<TransitAccountFriend> GetFriendsById(string ticket, int accountid, ServiceQueryOptions options)
        {
            // anyone can see everyone's friends (for now)
            // int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery q = session.CreateQuery(
                    string.Format("SELECT FROM AccountFriend f " +
                        "WHERE (f.Account.Id = {0} OR f.Keen.Id = {0}) " +
                        "ORDER BY f.Created DESC", accountid));

                if (options != null)
                {
                    q.SetFirstResult(options.FirstResult);
                    q.SetMaxResults(options.PageSize);
                }

                IList list = q.List();

                List<TransitAccountFriend> result = new List<TransitAccountFriend>(list.Count);
                foreach (AccountFriend friend in list)
                {
                    result.Add(new TransitAccountFriend(friend, friend.Account.Id != accountid));
                }

                return result;
            }
        }
    }
}
