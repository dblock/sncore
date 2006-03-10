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

        public override void Run()
        {
            ISessionFactory Factory = SnCore.Data.Hibernate.Session.Configuration.BuildSessionFactory();

            while (!IsStopping)
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
                        smtp.DeliveryMethod = (SmtpDeliveryMethod) Enum.Parse(typeof(SmtpDeliveryMethod), 
                            ManagedConfiguration.GetValue(session, "SnCore.Mail.Delivery", "Network"));
                        smtp.UseDefaultCredentials = true;

                        foreach (AccountEmailMessage m in emailmessages)
                        {
                            try
                            {
                                MailMessage message = new MailMessage();
                                message.Headers.Add("x-mimeole", string.Format("Produced By {0} {1}", 
                                    ManagedSystem.Title, ManagedSystem.ProductVersion));
                                message.Headers.Add("Content-class", "urn:content-classes:message");
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

                Thread.Sleep(1000 * SleepInterval);
            }
        }
    }

}
