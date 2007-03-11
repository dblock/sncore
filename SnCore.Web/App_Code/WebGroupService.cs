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
    /// Managed web account group services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebGroupService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebGroupService : WebService
    {
        public WebGroupService()
        {

        }

        #region AccountGroup

        /// <summary>
        /// Create or update an account group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group">transit account group</param>
        [WebMethod(Description = "Create or update an account group.")]
        public int CreateOrUpdateAccountGroup(string ticket, TransitAccountGroup t_instance)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group.
        /// </summary>
        /// <returns>transit account group</returns>
        [WebMethod(Description = "Get an account group.")]
        public TransitAccountGroup GetAccountGroupById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account groups.
        /// </summary>
        /// <returns>list of transit account groups</returns>
        [WebMethod(Description = "Get all account groups.")]
        public List<TransitAccountGroup> GetAccountGroups(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all account groups count.
        /// </summary>
        /// <returns>number of account groups</returns>
        [WebMethod(Description = "Get all account groups count.")]
        public int GetAccountGroupsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get public account groups.
        /// </summary>
        /// <returns>list of transit account groups</returns>
        [WebMethod(Description = "Get public account groups.")]
        public List<TransitAccountGroup> GetPublicAccountGroups(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.GetList(
                ticket, options, "WHERE AccountGroup.IsPrivate = 0");
        }

        /// <summary>
        /// Get public account groups count.
        /// </summary>
        /// <returns>number of account groups</returns>
        [WebMethod(Description = "Get public account groups count.")]
        public int GetPublicAccountGroupsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.GetCount(
                ticket, "WHERE AccountGroup.IsPrivate = 0");
        }

        /// <summary>
        /// Delete an account group.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group.")]
        public void DeleteAccountGroup(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroup, ManagedAccountGroup, AccountGroup>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountGroupAccount

        /// <summary>
        /// Create or update an account group account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group account">transit account group account</param>
        [WebMethod(Description = "Create or update an account group account.")]
        public int CreateOrUpdateAccountGroupAccount(string ticket, TransitAccountGroupAccount t_instance)
        {
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group account.
        /// </summary>
        /// <returns>transit account group account</returns>
        [WebMethod(Description = "Get an account group account.")]
        public TransitAccountGroupAccount GetAccountGroupAccountById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account group accounts.
        /// </summary>
        /// <returns>list of transit account group accounts</returns>
        [WebMethod(Description = "Get all account group accounts.")]
        public List<TransitAccountGroupAccount> GetAccountGroupAccounts(string ticket, int groupid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountGroup.Id", groupid) };
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group accounts count.
        /// </summary>
        /// <returns>number of account group accounts</returns>
        [WebMethod(Description = "Get all account group accounts count.")]
        public int GetAccountGroupAccountsCount(string ticket, int groupid)
        {
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccount.AccountGroup.Id = {0}", groupid));
        }

        /// <summary>
        /// Get all account group accounts for a given account.
        /// </summary>
        /// <returns>list of transit account group accounts for a given account</returns>
        [WebMethod(Description = "Get all account group accounts for a given account.")]
        public List<TransitAccountGroupAccount> GetAccountGroupAccountsByAccountId(string ticket, int accountid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", accountid) };
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group accounts count for a given account.
        /// </summary>
        /// <returns>number of account group accounts</returns>
        [WebMethod(Description = "Get all account group accounts count for a given account.")]
        public int GetAccountGroupAccountsByAccountIdCount(string ticket, int accountid)
        {
            return WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccount.Account.Id = {0}", accountid));
        }

        /// <summary>
        /// Delete an account group account.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group account.")]
        public void DeleteAccountGroupAccount(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroupAccount, ManagedAccountGroupAccount, AccountGroupAccount>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountGroupPicture

        /// <summary>
        /// Create or update an account group picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group picture">transit account group picture</param>
        [WebMethod(Description = "Create or update an account group picture.")]
        public int CreateOrUpdateAccountGroupPicture(string ticket, TransitAccountGroupPicture t_instance)
        {
            return WebServiceImpl<TransitAccountGroupPicture, ManagedAccountGroupPicture, AccountGroupPicture>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group picture.
        /// </summary>
        /// <returns>transit account group picture</returns>
        [WebMethod(Description = "Get an account group picture.", BufferResponse = true)]
        public TransitAccountGroupPicture GetAccountGroupPictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroupPicture, ManagedAccountGroupPicture, AccountGroupPicture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account group pictures.
        /// </summary>
        /// <returns>list of transit account group pictures</returns>
        [WebMethod(Description = "Get all account group pictures.", BufferResponse = true)]
        public List<TransitAccountGroupPicture> GetAccountGroupPictures(string ticket, int groupid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountGroup.Id", groupid) };
            return WebServiceImpl<TransitAccountGroupPicture, ManagedAccountGroupPicture, AccountGroupPicture>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group pictures count.
        /// </summary>
        /// <returns>number of account group pictures</returns>
        [WebMethod(Description = "Get all account group pictures count.", BufferResponse = true)]
        public int GetAccountGroupPicturesCount(string ticket, int groupid)
        {
            return WebServiceImpl<TransitAccountGroupPicture, ManagedAccountGroupPicture, AccountGroupPicture>.GetCount(
                ticket, string.Format("WHERE AccountGroupPicture.AccountGroup.Id = {0}", groupid));
        }

        /// <summary>
        /// Delete an account group picture.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group picture.")]
        public void DeleteAccountGroupPicture(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroupPicture, ManagedAccountGroupPicture, AccountGroupPicture>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountGroupPlace

        /// <summary>
        /// Create or update an account group place.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group place">transit account group place</param>
        [WebMethod(Description = "Create or update an account group place.")]
        public int CreateOrUpdateAccountGroupPlace(string ticket, TransitAccountGroupPlace t_instance)
        {
            return WebServiceImpl<TransitAccountGroupPlace, ManagedAccountGroupPlace, AccountGroupPlace>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group place.
        /// </summary>
        /// <returns>transit account group place</returns>
        [WebMethod(Description = "Get an account group place.")]
        public TransitAccountGroupPlace GetAccountGroupPlaceById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroupPlace, ManagedAccountGroupPlace, AccountGroupPlace>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account group places.
        /// </summary>
        /// <returns>list of transit account group places</returns>
        [WebMethod(Description = "Get all account group places.")]
        public List<TransitAccountGroupPlace> GetAccountGroupPlaces(string ticket, int groupid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountGroup.Id", groupid) };
            return WebServiceImpl<TransitAccountGroupPlace, ManagedAccountGroupPlace, AccountGroupPlace>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group places count.
        /// </summary>
        /// <returns>number of account group places</returns>
        [WebMethod(Description = "Get all account group places count.")]
        public int GetAccountGroupPlacesCount(string ticket, int groupid)
        {
            return WebServiceImpl<TransitAccountGroupPlace, ManagedAccountGroupPlace, AccountGroupPlace>.GetCount(
                ticket, string.Format("WHERE AccountGroupPlace.AccountGroup.Id = {0}", groupid));
        }

        /// <summary>
        /// Delete an account group place.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group place.")]
        public void DeleteAccountGroupPlace(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroupPlace, ManagedAccountGroupPlace, AccountGroupPlace>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountGroupAccountInvitation

        /// <summary>
        /// Create or update an account group account invitation.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group account invitation">transit account group account invitation</param>
        [WebMethod(Description = "Create or update an account group account invitation.")]
        public int CreateOrUpdateAccountGroupAccountInvitation(string ticket, TransitAccountGroupAccountInvitation t_instance)
        {
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group account invitation.
        /// </summary>
        /// <returns>transit account group account invitation</returns>
        [WebMethod(Description = "Get an account group account invitation.")]
        public TransitAccountGroupAccountInvitation GetAccountGroupAccountInvitationById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account group account invitations.
        /// </summary>
        /// <returns>list of transit account group account invitations</returns>
        [WebMethod(Description = "Get all account group account invitations.")]
        public List<TransitAccountGroupAccountInvitation> GetAccountGroupAccountInvitations(string ticket, int groupid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountGroup.Id", groupid) };
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group account invitations count.
        /// </summary>
        /// <returns>number of account group account invitations</returns>
        [WebMethod(Description = "Get all account group account invitations count.")]
        public int GetAccountGroupAccountInvitationsCount(string ticket, int groupid)
        {
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccountInvitation.AccountGroup.Id = {0}", groupid));
        }

        /// <summary>
        /// Get all account group account invitations by account id.
        /// </summary>
        /// <returns>list of transit account group account invitations</returns>
        [WebMethod(Description = "Get all account group account invitations by account id.")]
        public List<TransitAccountGroupAccountInvitation> GetAccountGroupAccountInvitationsByAccountId(string ticket, int accountid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", accountid) };
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group account invitations count by account id.
        /// </summary>
        /// <returns>number of account group account invitations</returns>
        [WebMethod(Description = "Get all account group account invitations count by account id.")]
        public int GetAccountGroupAccountInvitationsByAccountIdCount(string ticket, int accountid)
        {
            return WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccountInvitation.Account.Id = {0}", accountid));
        }

        /// <summary>
        /// Delete an account group account invitation.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group account invitation.")]
        public void DeleteAccountGroupAccountInvitation(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation, AccountGroupAccountInvitation>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountGroupAccountRequest

        /// <summary>
        /// Create or update an account group account request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account group account request">transit account group account request</param>
        [WebMethod(Description = "Create or update an account group account request.")]
        public int CreateOrUpdateAccountGroupAccountRequest(string ticket, TransitAccountGroupAccountRequest t_instance)
        {
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account group account request.
        /// </summary>
        /// <returns>transit account group account request</returns>
        [WebMethod(Description = "Get an account group account request.")]
        public TransitAccountGroupAccountRequest GetAccountGroupAccountRequestById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account group account requests.
        /// </summary>
        /// <returns>list of transit account group account requests</returns>
        [WebMethod(Description = "Get all account group account requests.")]
        public List<TransitAccountGroupAccountRequest> GetAccountGroupAccountRequests(string ticket, int groupid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountGroup.Id", groupid) };
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group account requests count.
        /// </summary>
        /// <returns>number of account group account requests</returns>
        [WebMethod(Description = "Get all account group account requests count.")]
        public int GetAccountGroupAccountRequestsCount(string ticket, int groupid)
        {
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccountRequest.AccountGroup.Id = {0}", groupid));
        }

        /// <summary>
        /// Get all account group account requests by account id.
        /// </summary>
        /// <returns>list of transit account group account requests</returns>
        [WebMethod(Description = "Get all account group account requests by account id.")]
        public List<TransitAccountGroupAccountRequest> GetAccountGroupAccountRequestsByAccountId(string ticket, int accountid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", accountid) };
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account group account requests count by account id.
        /// </summary>
        /// <returns>number of account group account requests</returns>
        [WebMethod(Description = "Get all account group account requests count by account id.")]
        public int GetAccountGroupAccountRequestsByAccountIdCount(string ticket, int accountid)
        {
            return WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.GetCount(
                ticket, string.Format("WHERE AccountGroupAccountRequest.Account.Id = {0}", accountid));
        }

        /// <summary>
        /// Delete an account group account request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account group account request.")]
        public void DeleteAccountGroupAccountRequest(string ticket, int id)
        {
            WebServiceImpl<TransitAccountGroupAccountRequest, ManagedAccountGroupAccountRequest, AccountGroupAccountRequest>.Delete(
                ticket, id);
        }

        #endregion
    }
}
