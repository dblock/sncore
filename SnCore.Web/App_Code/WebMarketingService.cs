using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Text;
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web marketing services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebMarketingService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebMarketingService : WebService
    {
        public WebMarketingService()
        {

        }

        #region Marketing Campaign

        /// <summary>
        /// Get campaigns count.
        /// </summary>
        [WebMethod(Description = "Get marketing campaigns count.", CacheDuration = 60)]
        public int GetCampaignsCount(string ticket)
        {
            return WebServiceImpl<TransitCampaign, ManagedCampaign, Campaign>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all marketing campaigns.
        /// </summary>
        [WebMethod(Description = "Get all marketing campaigns.", CacheDuration = 60)]
        public List<TransitCampaign> GetCampaigns(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitCampaign, ManagedCampaign, Campaign>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Create or update a marketing campaign.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a marketing campaign.")]
        public int CreateOrUpdateCampaign(string ticket, TransitCampaign campaign)
        {
            return WebServiceImpl<TransitCampaign, ManagedCampaign, Campaign>.CreateOrUpdate(
                ticket, campaign);
        }

        /// <summary>
        /// Delete a marketing campaign.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a marketing campaign.")]
        public void DeleteCampaign(string ticket, int id)
        {
            WebServiceImpl<TransitCampaign, ManagedCampaign, Campaign>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a marketing campaign by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a marketing campaign by id.")]
        public TransitCampaign GetCampaignById(string ticket, int id)
        {
            return WebServiceImpl<TransitCampaign, ManagedCampaign, Campaign>.GetById(
                ticket, id);
        }

        #endregion

        #region Marketing Campaign Account Recepients

        /// <summary>
        /// Get campaign accounts count.
        /// </summary>
        [WebMethod(Description = "Get marketing campaign account recepients count.", CacheDuration = 60)]
        public int GetCampaignAccountRecepientsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient, CampaignAccountRecepient>.GetCount(
                ticket, string.Format("WHERE CampaignAccountRecepient.Campaign.Id = {0}", id));
        }

        /// <summary>
        /// Get all marketing campaign account recepients.
        /// </summary>
        [WebMethod(Description = "Get all marketing campaign accounts.", CacheDuration = 60)]
        public List<TransitCampaignAccountRecepient> GetCampaignAccountRecepients(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Campaign.Id", id) };
            return WebServiceImpl<TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient, CampaignAccountRecepient>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Create or update a marketing campaign account recepeient.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a marketing campaign account recepient.")]
        public int CreateOrUpdateCampaignAccountRecepient(string ticket, TransitCampaignAccountRecepient recepient)
        {
            return WebServiceImpl<TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient, CampaignAccountRecepient>.CreateOrUpdate(
                ticket, recepient);
        }

        /// <summary>
        /// Delete a marketing campaign account recepient.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a marketing campaign account recepient.")]
        public void DeleteCampaignAccountRecepient(string ticket, int id)
        {
            WebServiceImpl<TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient, CampaignAccountRecepient>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Delete all marketing campaign account recepients.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete all marketing campaign account recepients.")]
        public void DeleteCampaignAccountRecepients(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedCampaign campaign = new ManagedCampaign(session, id);
                campaign.DeleteRecepients(sec);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a marketing campaign account recepient by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a marketing campaign account recepient by id.")]
        public TransitCampaignAccountRecepient GetCampaignAccountRecepientById(string ticket, int id)
        {
            return WebServiceImpl<TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient, CampaignAccountRecepient>.GetById(
                ticket, id);
        }

        #endregion

        #region Import Marketing Campaign Account Recepients

        /// <summary>
        /// Import marketing campaign account recepeients by account ids.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Import marketing campaign account recepeients by account ids.")]
        public int ImportCampaignAccountRecepients(string ticket, TransitCampaignAccountRecepient[] recepients)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ITransaction trans = session.BeginTransaction();

                int count = 0;

                try
                {
                    foreach (TransitCampaignAccountRecepient recepient in recepients)
                    {
                        CampaignAccountRecepient existing = (CampaignAccountRecepient) session.CreateCriteria(typeof(CampaignAccountRecepient))
                            .Add(Expression.Eq("Account.Id", recepient.AccountId))
                            .Add(Expression.Eq("Campaign.Id", recepient.CampaignId))
                            .UniqueResult();

                        if (existing != null)
                            continue;

                        ManagedCampaignAccountRecepient newrecepient = new ManagedCampaignAccountRecepient(session);
                        newrecepient.CreateOrUpdate(recepient, sec);
                        count++;
                    }

                    trans.Commit();
                    SnCore.Data.Hibernate.Session.Flush();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                return count;
            }
        }

        /// <summary>
        /// Import marketing campaign account emails.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Import marketing campaign account emails.")]
        public int ImportCampaignAccountEmails(string ticket, int campaign_id, bool verified_emails, bool unverified_emails)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ITransaction trans = session.BeginTransaction();

                int count = 0;

                try
                {
                    ICriteria c = session.CreateCriteria(typeof(Account));
                    IList list = c.List();

                    foreach (Account account in list)
                    {
                        ManagedAccount ma = new ManagedAccount(session, account);

                        if (ma.HasVerifiedEmail)
                        {
                            if (! verified_emails)
                                continue;
                        }
                        else
                        {
                            if (! unverified_emails)
                                continue;
                        }

                        CampaignAccountRecepient existing = (CampaignAccountRecepient) session.CreateCriteria(typeof(CampaignAccountRecepient))
                            .Add(Expression.Eq("Account.Id", ma.Id))
                            .Add(Expression.Eq("Campaign.Id", campaign_id))
                            .UniqueResult();

                        if (existing != null)
                            continue;

                        ManagedCampaignAccountRecepient newrecepient = new ManagedCampaignAccountRecepient(session);
                        TransitCampaignAccountRecepient newtransitrecepient = new TransitCampaignAccountRecepient();
                        newtransitrecepient.AccountId = ma.Id;
                        newtransitrecepient.CampaignId = campaign_id;
                        newtransitrecepient.Created = newtransitrecepient.Modified = DateTime.UtcNow;
                        newtransitrecepient.Sent = false;
                        newrecepient.CreateOrUpdate(newtransitrecepient, sec);
                        count++;
                    }

                    trans.Commit();
                    SnCore.Data.Hibernate.Session.Flush();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                return count;
            }
        }

        /// <summary>
        /// Import accounts into a marketing campaign by matching property value.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Import accounts into a marketing campaign by matching property value.")]
        public int ImportCampaignAccountPropertyValues(string ticket, int campaign_id, int pid, string value, bool unset)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ITransaction trans = session.BeginTransaction();

                int count = 0;

                try
                {
                    StringBuilder squery = new StringBuilder();
                    squery.Append(
                        "SELECT {Account.*} FROM {Account} WHERE Account_Id IN (" +
                        " SELECT Account.Account_Id FROM Account INNER JOIN AccountPropertyValue" +
                        " ON Account.Account_Id = AccountPropertyValue.Account_Id" +
                        " WHERE AccountPropertyValue.AccountProperty_Id = " + pid.ToString() + 
                        " AND AccountPropertyValue.Value LIKE '" + Renderer.SqlEncode(value) + "')"); 
                                           
                    if (unset)
                    {
                        squery.AppendFormat(
                            " OR Account_Id NOT IN (" +
                            " SELECT Account.Account_Id FROM Account INNER JOIN AccountPropertyValue" +
                            " ON Account.Account_Id = AccountPropertyValue.Account_Id" +
                            " AND AccountPropertyValue.AccountProperty_Id = {0}" +
                            ")", pid);
                    }


                    IQuery query = session.CreateSQLQuery(squery.ToString(), "Account", typeof(Account));
                    IList list = query.List();

                    foreach (Account account in list)
                    {
                        ManagedAccount ma = new ManagedAccount(session, account);

                        if (! ma.HasVerifiedEmail)
                            continue;

                        CampaignAccountRecepient existing = (CampaignAccountRecepient)session.CreateCriteria(typeof(CampaignAccountRecepient))
                            .Add(Expression.Eq("Account.Id", ma.Id))
                            .Add(Expression.Eq("Campaign.Id", campaign_id))
                            .UniqueResult();

                        if (existing != null)
                            continue;

                        ManagedCampaignAccountRecepient newrecepient = new ManagedCampaignAccountRecepient(session);
                        TransitCampaignAccountRecepient newtransitrecepient = new TransitCampaignAccountRecepient();
                        newtransitrecepient.AccountId = ma.Id;
                        newtransitrecepient.CampaignId = campaign_id;
                        newtransitrecepient.Created = newtransitrecepient.Modified = DateTime.UtcNow;
                        newtransitrecepient.Sent = false;
                        newrecepient.CreateOrUpdate(newtransitrecepient, sec);
                        count++;
                    }

                    trans.Commit();
                    SnCore.Data.Hibernate.Session.Flush();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                return count;
            }
        }

       #endregion

    }
}