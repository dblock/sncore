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

        #region Account Activity

        /// <summary>
        /// Get new accounts.
        /// </summary>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get new accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetNewAccounts(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options,
                    "SELECT Account FROM Account Account WHERE EXISTS (" +
                        " SELECT FROM AccountPicture AccountPicture " +
                        " WHERE AccountPicture.Account = Account AND AccountPicture.Hidden = 0" +
                        ") ORDER BY Account.Created DESC");
        }

        /// <summary>
        /// Get active accounts.
        /// </summary>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get active accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetActiveAccounts(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options,
                    "SELECT Account FROM Account Account WHERE EXISTS (" +
                        " FROM AccountPicture AccountPicture " +
                        " WHERE AccountPicture.Account = Account AND AccountPicture.Hidden = 0" +
                        ") ORDER BY Account.LastLogin DESC");
        }

        /// <summary>
        /// Get account activity count.
        /// </summary>
        /// <returns>transit account activity count</returns>
        [WebMethod(Description = "Get account activity count.", CacheDuration = 60)]
        public int GetAccountActivityCount(string ticket, AccountActivityQueryOptions queryoptions)
        {
            return WebServiceImpl<TransitAccountActivity, ManagedAccountActivity, Account>.GetCount(
                ticket, queryoptions.CreateCountQuery());
        }

        /// <summary>
        /// Get account activity.
        /// </summary>
        /// <returns>transit account activity</returns>
        [WebMethod(Description = "Get account activity.", CacheDuration = 60)]
        public List<TransitAccountActivity> GetAccountActivity(string ticket, AccountActivityQueryOptions queryoptions, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountActivity, ManagedAccountActivity, Account>.GetList(
                ticket, options, queryoptions.CreateQuery());
        }

        #endregion

        #region N-th Degree of Separation

        /// <summary>
        /// Get n-th degree count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of n-th degree contacts</returns>
        [WebMethod(Description = "Get n-th degree count.", CacheDuration = 60)]
        public int GetNDegreeCountById(string ticket, int id, int deg)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount acct = new ManagedAccount(session, id);
                return acct.GetNDegreeCount(sec, deg);
            }
        }

        /// <summary>
        /// Get 1st degree count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of 1st degree contacts</returns>
        [WebMethod(Description = "Get 1st degree count.", CacheDuration = 60)]
        public int GetFirstDegreeCountById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriend, ManagedAccountFriend, AccountFriend>.GetCount(
                ticket, string.Format("WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0}) ",
                    id));
        }

        #endregion

        #region Friends Activity

        /// <summary>
        /// Get friends activity count.
        /// </summary>
        /// <returns>transit account activity count</returns>
        [WebMethod(Description = "Get friends activity count.", CacheDuration = 60)]
        public int GetFriendsAccountActivityCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriend, ManagedAccountFriend, AccountFriend>.GetCount(
                ticket, string.Format("WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0}) ",
                    id));
        }

        /// <summary>
        /// Get friends activity.
        /// </summary>
        /// <returns>transit account activity</returns>
        [WebMethod(Description = "Get friends activity.", CacheDuration = 60)]
        public List<TransitAccountActivity> GetFriendsAccountActivity(string ticket, int id, ServiceQueryOptions options)
        {
            ManagedAccountActivity m_activity = null;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                m_activity = new ManagedAccountActivity(session, id);
            }

            List<TransitAccountActivity> result = WebServiceImpl<TransitAccountActivity, ManagedAccountFriend, AccountFriend>.GetList(
                ticket, options, string.Format("SELECT AccountFriend FROM AccountFriend AccountFriend WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0})", id),
                m_activity.GetTransformedInstanceFromAccountFriend);

            result.Sort(TransitAccountActivity.CompareByLastActivity);

            return result;
        }

        #endregion

        #region Friend Requests

        /// <summary>
        /// Create a new friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="friendid">target friend account id</param>
        /// <param name="message">optional message to send with the request</param>
        /// <returns>request id</returns>
        [WebMethod(Description = "Create a new friend request.")]
        public int CreateOrUpdateAccountFriendRequest(string ticket, int friendid, string message)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, userid);
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                int result = acct.CreateAccountFriendRequest(sec, friendid, message);
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

                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                req.Accept(sec, message);
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

                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                req.Reject(sec, message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get total friend requests sent count by account id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request sent count by account id.", CacheDuration = 60)]
        public int GetSentAccountFriendRequestsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.GetCount(
                ticket, string.Format("WHERE AccountFriendRequest.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get total friend requests count by account id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get friend request count by account id.", CacheDuration = 60)]
        public int GetAccountFriendRequestsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.GetCount(
                ticket, string.Format("WHERE AccountFriendRequest.Keen.Id = {0} AND AccountFriendRequest.Rejected = 0", id));
        }

        /// <summary>
        /// Get sent friend requests.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get sent friend requests.")]
        public List<TransitAccountFriendRequest> GetSentAccountFriendRequests(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get outstanding friend requests.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get outstanding friend requests.")]
        public List<TransitAccountFriendRequest> GetAccountFriendRequests(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = 
            { 
                Expression.Eq("Keen.Id", id),
                Expression.Eq("Rejected", false)
            };

            return WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a friend request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">friend request id</param>
        [WebMethod(Description = "Delete a friend request.")]
        public void DeleteAccountFriendRequest(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.Delete(
                ticket, id);
        }

        [WebMethod(Description = "Get an account friend request.")]
        public TransitAccountFriendRequest GetAccountFriendRequestById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriendRequest, ManagedAccountFriendRequest, AccountFriendRequest>.GetById(
                ticket, id);
        }

        #endregion

        #region Friends

        /// <summary>
        /// Delete a friend.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">friend id</param>
        [WebMethod(Description = "Delete a friend.")]
        public void DeleteAccountFriend(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFriend, ManagedAccountFriend, AccountFriend>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get friends count.
        /// </summary>
        [WebMethod(Description = "Get friends count.")]
        public int GetAccountFriendsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFriend, ManagedAccountFriend, AccountFriend>.GetCount(
                ticket, string.Format("WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0})", id));
        }

        /// <summary>
        /// Get friends.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account id">account id</param>
        [WebMethod(Description = "Get friends.", CacheDuration = 60)]
        public List<TransitAccountFriend> GetAccountFriends(string ticket, int id, ServiceQueryOptions options)
        {
            ManagedAccount m_account = null;
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                m_account = new ManagedAccount(session, id);
            }

            return WebServiceImpl<TransitAccountFriend, ManagedAccountFriend, AccountFriend>.GetList(
                ticket, options, string.Format("SELECT AccountFriend FROM AccountFriend AccountFriend WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0}) ORDER BY AccountFriend.Created DESC", id),
                m_account.GetTransformedInstanceFromAccountFriend);
        }

        #endregion
    }
}
