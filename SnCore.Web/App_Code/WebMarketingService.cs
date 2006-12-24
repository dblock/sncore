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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return (int) session.CreateQuery("SELECT COUNT(*) FROM Campaign c").UniqueResult();
            }
        }

        /// <summary>
        /// Get all marketing campaigns.
        /// </summary>
        [WebMethod(Description = "Get all marketing campaigns.", CacheDuration = 60)]
        public List<TransitCampaign> GetCampaigns(string ticket, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ICriteria c = session.CreateCriteria(typeof(Campaign))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitCampaign> result = new List<TransitCampaign>(list.Count);
                foreach (Campaign campaign in list)
                {
                    result.Add(new TransitCampaign(campaign));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Create or update a marketing campaign.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a marketing campaign.")]
        public int CreateOrUpdateCampaign(string ticket, TransitCampaign campaign)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCampaign mc = new ManagedCampaign(session);
                mc.CreateOrUpdate(campaign);

                SnCore.Data.Hibernate.Session.Flush();
                return mc.Id;
            }
        }

        /// <summary>
        /// Delete a marketing campaign.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a marketing campaign.")]
        public void DeleteCampaign(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
 
                ManagedCampaign mc = new ManagedCampaign(session, id);
                mc.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a marketing campaign by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a marketing campaign by id.")]
        public TransitCampaign GetCampaignById(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return new ManagedCampaign(session, id).TransitCampaign;
            }
        }

        #endregion

        #region Marketing Campaign Account Recepients

        /// <summary>
        /// Get campaign accounts count.
        /// </summary>
        [WebMethod(Description = "Get marketing campaign account recepients count.", CacheDuration = 60)]
        public int GetCampaignAccountRecepientsByIdCount(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM CampaignAccountRecepient ca WHERE ca.Campaign.Id = {0}", id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all marketing campaign account recepients.
        /// </summary>
        [WebMethod(Description = "Get all marketing campaign accounts.", CacheDuration = 60)]
        public List<TransitCampaignAccountRecepient> GetCampaignAccountRecepientsById(string ticket, int id, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ICriteria c = session.CreateCriteria(typeof(CampaignAccountRecepient))
                    .Add(Expression.Eq("Campaign.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitCampaignAccountRecepient> result = new List<TransitCampaignAccountRecepient>(list.Count);
                foreach (CampaignAccountRecepient recepient in list)
                {
                    result.Add(new TransitCampaignAccountRecepient(recepient));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Create or update a marketing campaign account recepeient.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a marketing campaign account recepient.")]
        public int CreateOrUpdateCampaignAccountRecepient(string ticket, TransitCampaignAccountRecepient recepient)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCampaignAccountRecepient mc = new ManagedCampaignAccountRecepient(session);
                mc.CreateOrUpdate(recepient);

                SnCore.Data.Hibernate.Session.Flush();
                return mc.Id;
            }
        }

        /// <summary>
        /// Import marketing campaign account recepeients by account ids.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Import marketing campaign account recepeients by account ids.")]
        public int ImportCampaignAccountRecepients(string ticket, TransitCampaignAccountRecepient[] recepients)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

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
                        newrecepient.CreateOrUpdate(recepient);
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

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
                        newrecepient.CreateOrUpdate(newtransitrecepient);
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

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

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
                        newrecepient.CreateOrUpdate(newtransitrecepient);
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
        /// Delete a marketing campaign account recepient.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a marketing campaign account recepient.")]
        public void DeleteCampaignAccountRecepient(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCampaignAccountRecepient mc = new ManagedCampaignAccountRecepient(session, id);
                mc.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete all marketing campaign account recepients.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete all marketing campaign account recepients.")]
        public void DeleteCampaignAccountRecepients(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                session.Delete(string.Format("FROM CampaignAccountRecepient r WHERE r.Campaign.Id = {0}", id));
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return new ManagedCampaignAccountRecepient(session, id).TransitCampaignAccountRecepient;
            }
        }

        #endregion

    }
}