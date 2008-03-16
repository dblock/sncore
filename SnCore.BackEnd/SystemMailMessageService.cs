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
        public static string Name = "SnCore System PostMaster Service";
        public static string Description = "Routes e-mails between the SnCore users and an SMTP server.";
        
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

        public SystemMailMessageService()
        {
            ServiceName = Name;
        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunEmailQueue));
            AddJob(new SessionJobDelegate(RunMarketingCampaign));
        }

        public void RunEmailQueue(ISession session, ManagedSecurityContext sec)
        {
            IList emailmessages = session.CreateCriteria(typeof(AccountEmailMessage))
                .Add(Expression.Eq("Sent", false))
                .SetMaxResults(ChunkSize)
                .List();

            SleepInterval = emailmessages.Count >= ChunkSize ? 1 : 15;

            SmtpClient smtp = new SmtpClient(
                ManagedConfiguration.GetValue(session, "SnCore.Mail.Server", "localhost"),
                int.Parse(ManagedConfiguration.GetValue(session, "SnCore.Mail.Port", "25")));
            smtp.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod),
                ManagedConfiguration.GetValue(session, "SnCore.Mail.Delivery", "Network"));
            smtp.PickupDirectoryLocation = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.PickupDirectoryLocation", string.Empty);

            string smtpusername = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.Username", string.Empty);

            string smtppassword = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.Password", string.Empty);

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
                    message.Headers.Add("Content-Type", "text/html; charset=\"ISO-8859-1\"");
                    message.IsBodyHtml = true;
                    Encoding iso8859 = Encoding.GetEncoding(28591);
                    message.BodyEncoding = iso8859;
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

                session.Flush();
                Thread.Sleep(1000 * InterruptInterval);
            }
        }

        public void RunMarketingCampaign(ISession session, ManagedSecurityContext sec)
        {
            IList campaigns = session.CreateCriteria(typeof(Campaign))
                .Add(Expression.Eq("Active", true))
                .List();

            foreach (Campaign campaign in campaigns)
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
                ContentPageParameters parameters = new ContentPageParameters();
                parameters.UserAgent = ManagedConfiguration.GetValue(session, "SnCore.Web.UserAgent", "SnCore/1.0");
                parameters.BaseUri = new Uri(baseuri);
                string content = ContentPage.GetContent(new Uri(campaign.Url), parameters);

                foreach (CampaignAccountRecepient recepient in recepients)
                {
                    try
                    {
                        ManagedAccount m_recepient = new ManagedAccount(session, recepient.Account);

                        string mailto;
                        if (!m_recepient.TryGetVerifiedEmailAddress(out mailto, sec))
                            throw new Exception("No verified e-mail address.");

                        m_recepient.SendAccountEmailMessage(
                            campaign.SenderEmailAddress, mailto,
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
        }

    }

}
