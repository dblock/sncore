using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Threading;
using System.Net.Mail;
using System.Diagnostics;
using System.Text;
using System.Net;
using SnCore.Tools.Web;

namespace SnCore.BackEndServices
{
    public class SystemMailMessageService : SystemService
    {
        private int mSleepInterval = 5;
        private int mInterruptInterval = 1;
        private int mChunkSize = 10;

        public int ChunkSize
        {
            get
            {
                return mChunkSize;
            }
            set
            {
                mChunkSize = value;
            }
        }

        public int InterruptInterval
        {
            get
            {
                return mInterruptInterval;
            }
            set
            {
                mInterruptInterval = value;
            }
        }

        public int SleepInterval
        {
            get
            {
                return mSleepInterval;
            }
            set
            {
                mSleepInterval = value;
            }
        }

        public SystemMailMessageService()
        {

        }

        private ISessionFactory mFactory = null;

        protected ISessionFactory Factory
        {
            get
            {
                if (mFactory == null)
                {
                    mFactory = SnCore.Data.Hibernate.Session.Configuration.BuildSessionFactory();
                }
                return mFactory;
            }
        }

        public override void Run()
        {
            while (!IsStopping)
            {
                RunEmailQueue();
                Thread.Sleep(1000 * SleepInterval);
                RunMarketingCampaign();
                Thread.Sleep(1000 * SleepInterval);
            }
        }

        private void RunEmailQueue()
        {
            try
            {
                IDbConnection conn = GetNewConnection();
                conn.Open();

                ISession session = Factory.OpenSession(conn);

                try
                {

                    IList emailmessages = session.CreateCriteria(typeof(AccountEmailMessage))
                        .Add(Expression.Eq("Sent", false))
                        .SetMaxResults(ChunkSize)
                        .List();

                    SleepInterval = emailmessages.Count >= ChunkSize ? 5 : 30;

                    SmtpClient smtp = new SmtpClient(
                        ManagedConfiguration.GetValue(session, "SnCore.Mail.Server", "localhost"),
                        int.Parse(ManagedConfiguration.GetValue(session, "SnCore.Mail.Port", "25")));
                    smtp.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod),
                        ManagedConfiguration.GetValue(session, "SnCore.Mail.Delivery", "Network"));

                    string smtpusername = ConfigurationManager.AppSettings["smtp.username"];
                    string smtppassword = ConfigurationManager.AppSettings["smtp.password"];

                    if (!string.IsNullOrEmpty(smtpusername))
                    {
                        smtp.Credentials = new NetworkCredential(smtpusername, smtppassword);
                    }
                    else
                    {
                        smtp.UseDefaultCredentials = true;
                    }

                    foreach (AccountEmailMessage m in emailmessages)
                    {
                        try
                        {
                            MailMessage message = new MailMessage();
                            message.Headers.Add("x-mimeole", string.Format("Produced By {0} {1}",
                                ManagedSystem.Title, ManagedSystem.ProductVersion));
                            message.Headers.Add("Content-class", "urn:content-classes:message");
                            message.Headers.Add("Content-Type", "text/html; charset=\"utf-8\"");
                            message.IsBodyHtml = true;
                            message.Body = m.Body;
                            message.ReplyTo = new MailAddress(m.MailFrom);
                            message.From = new MailAddress(
                                ManagedConfiguration.GetValue(session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                                ManagedConfiguration.GetValue(session, "SnCore.Admin.Name", "Admin")
                                );
                            message.To.Add(new MailAddress(m.MailTo));
                            message.Subject = m.Subject;
                            smtp.Send(message);
                            m.Sent = true;
                            if (m.DeleteSent)
                            {
                                session.Delete(m);
                            }
                            else
                            {
                                session.Save(m);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (m.Created.AddDays(1) < DateTime.UtcNow)
                            {
                                session.Delete(m);
                            }
                            else
                            {
                                m.SendError = ex.Message;
                                session.Save(m);
                            }
                        }

                        Thread.Sleep(1000 * InterruptInterval);
                    }
                    session.Flush();
                }
                finally
                {
                    conn.Close();
                    session.Close();
                }
            }
            catch
            {

            }
        }

        private void RunMarketingCampaign()
        {
            try
            {
                IDbConnection conn = GetNewConnection();
                conn.Open();

                ISession session = Factory.OpenSession(conn);

                try
                {

                    IList campaigns = session.CreateCriteria(typeof(Campaign))
                        .Add(Expression.Eq("Active", true))
                        .List();

                    foreach(Campaign campaign in campaigns)
                    {
                        IList recepients = session.CreateCriteria(typeof(CampaignAccountRecepient))
                            .Add(Expression.Eq("Sent", false))
                            .Add(Expression.Eq("Campaign.Id", campaign.Id))
                            .List();

                        if (recepients.Count == 0)
                        {
                            campaign.Active = false;
                            session.Save(campaign);
                            continue;
                        }

                        string baseuri = ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCore");
                        if (!baseuri.EndsWith("/")) baseuri = baseuri + "/";
                        string content = ContentPage.GetContent(new Uri(campaign.Url), new Uri(baseuri));

                        foreach (CampaignAccountRecepient recepient in recepients)
                        {
                            try
                            {
                                ManagedAccount m_recepient = new ManagedAccount(session, recepient.Account);
                                if (!m_recepient.HasVerifiedEmail)
                                    throw new Exception("No verified e-mail address.");

                                m_recepient.SendAccountMailMessage(
                                    campaign.SenderEmailAddress, m_recepient.ActiveEmailAddress,
                                    campaign.Name, content, true);

                                recepient.LastError = string.Empty;
                            }
                            catch (Exception ex)
                            {
                                recepient.LastError = ex.Message;
                            }
                            finally
                            {
                                recepient.Modified = DateTime.UtcNow;
                                recepient.Sent = true;
                                session.Save(recepient);
                                session.Flush();
                            }

                            Thread.Sleep(100);
                        }

                        Thread.Sleep(1000 * InterruptInterval);
                    }
                    session.Flush();
                }
                finally
                {
                    conn.Close();
                    session.Close();
                }
            }
            catch
            {

            }
        }

    }

}
