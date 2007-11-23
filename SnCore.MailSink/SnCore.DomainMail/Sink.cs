using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using MIME;
using System.Collections;
using System.Collections.Generic;
using SnCore.Data;
using SnCore.Data.Hibernate;
using NHibernate;
using NHibernate.Expression;

namespace SnCore.DomainMail
{
    [Guid("E6A74E11-7234-4096-B9E6-C65165B9B93F")]
    [ComVisible(true)]
    public class Sink : IMailTransportSubmission
    {
        private bool mDebug = true;
        private Configuration mConfiguration = null;

        public Sink()
        {
            LoadConfiguration();

            if (Debug)
            {
                EventLog.WriteEntry(
                     Assembly.GetExecutingAssembly().FullName,
                     "Loaded SnCore.DomainMail sink.",
                     EventLogEntryType.Information);
            }
        }      

        public bool Debug
        {
            get
            {
                return mDebug;
            }
        }

        public SnCore.DomainMail.Configuration Configuration
        {
            get
            {
                LoadConfiguration();
                return mConfiguration;
            }
            set
            {
                mConfiguration = value;
            }
        }

        private void LoadConfiguration()
        {
            Monitor.Enter(this);
            string cnf = Assembly.GetExecutingAssembly().Location + ".config";
            try
            {
                if (mConfiguration == null)
                {
                    if (File.Exists(cnf))
                    {
                        mConfiguration = new SnCore.DomainMail.Configuration(cnf);

                        object Debug = mConfiguration["debug"];
                        mDebug = (Debug == null) ? true : bool.Parse(Debug.ToString());

                        if (mDebug)
                        {
                            EventLog.WriteEntry(
                             Assembly.GetExecutingAssembly().FullName,
                             "Loaded configuration file " + cnf,
                             EventLogEntryType.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                     Assembly.GetExecutingAssembly().FullName,
                     "Error loading configuration file \"" + cnf + "\"\n" + ex.Message,
                     EventLogEntryType.Error);
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        private void UpdateFailure(MimeDSNRecipient r)
        {
            ISession session = SnCore.Data.Hibernate.Session.Current;
            IList<AccountEmail> emails = session.CreateCriteria(typeof(AccountEmail))
                .Add(Expression.Eq("Address", r.FinalRecipientEmailAddress))
                .List<AccountEmail>();

            if (emails != null)
            {
                foreach (AccountEmail email in emails)
                {
                    EventLog.WriteEntry(
                         Assembly.GetExecutingAssembly().FullName,
                         string.Format("Marked {0} [{1}] (id={2}) with failure.",
                            email.Account.Name, email.Address, email.Id),
                         EventLogEntryType.Information);
                    email.Failed = true;
                    email.LastError = r.DiagnosticCode;
                    email.Modified = DateTime.UtcNow;
                    session.Save(email);
                }
            }

            session.Flush();
        }

        void IMailTransportSubmission.OnMessageSubmission(
             MailMsg mailmsg,
             IMailTransportNotify notify,
             IntPtr context)
        {
            try
            {
                Message message = new Message(mailmsg);
                string raw = Encoding.ASCII.GetString(message.ReadContent(0, message.GetContentSize()));
                
                MimeMessage msg = new MimeMessage();
                msg.LoadBody(raw);

                ArrayList bodylist = new ArrayList();
                msg.GetBodyPartList(bodylist);

                for (int i = 0; i < bodylist.Count; i++)
                {
                    MimeBody ab = (MimeBody) bodylist[i];
                    switch (ab.GetContentType())
                    {
                        case "message/delivery-status":
                            /// TODO: move to Mime processor
                            MimeDSN dsn = new MimeDSN();
                            dsn.LoadBody(ab.GetText());
                            foreach (MimeDSNRecipient r in dsn.Recipients)
                            {
                                switch (r.Action)
                                {
                                    case "failed":
                                        EventLog.WriteEntry(
                                             Assembly.GetExecutingAssembly().FullName,
                                             string.Format("Processing {0} ({1}) in {2} with subject \"{3}\".",
                                                r.FinalRecipientEmailAddress, r.Action, message.Rfc822MsgId, message.Rfc822MsgSubject),
                                             EventLogEntryType.Information);
                                        UpdateFailure(r);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                  Assembly.GetExecutingAssembly().FullName,
                  ex.Message + "\n" + ex.StackTrace.ToString(),
                  EventLogEntryType.Error);
            }
            finally
            {
                if (null != mailmsg)
                    Marshal.ReleaseComObject(mailmsg);
            }
        }
    }
}
