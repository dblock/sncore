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
            LogDebug("Loaded SnCore.DomainMail sink.");
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
            if (mConfiguration != null)
                return;

            Monitor.Enter(this);
            string cnf = Assembly.GetExecutingAssembly().Location + ".config";
            try
            {
                if (mConfiguration == null)
                {
                    if (File.Exists(cnf))
                    {
                        mConfiguration = new SnCore.DomainMail.Configuration(cnf);

                        object debug = mConfiguration["debug"];
                        mDebug = (debug == null) ? true : bool.Parse(debug.ToString());
                        LogDebug(string.Format("Loaded configuration file \"{0}\".", cnf));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(string.Format("Error loading configuration file \"{0}\"\n{1}", cnf, ex.Message));
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        private void UpdateFailure(MimeDSNRecipient r)
        {
            ISession session = SnCore.Data.Hibernate.Session.Factory.OpenSession();
            try
            {
                IList<AccountEmail> emails = session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Address", r.FinalRecipientEmailAddress))
                    .List<AccountEmail>();

                if (emails != null)
                {
                    foreach (AccountEmail email in emails)
                    {
                        if (email.Failed)
                            continue;

                        Log(string.Format("Marked {0} [{1}] (id:{2}) with failure [{3}].",
                            email.Account.Name, email.Address, email.Id, r.DiagnosticCode));

                        email.Failed = true;
                        email.LastError = r.DiagnosticCode;
                        email.Modified = DateTime.UtcNow;
                        session.Save(email);
                        session.Flush();
                    }
                }
            }
            finally
            {
                session.Close();
            }
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
                                        Log(string.Format("Processing {0} ({1}) in {2} with subject \"{3}\".", 
                                            r.FinalRecipientEmailAddress, r.Action, message.Rfc822MsgId, message.Rfc822MsgSubject));
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
                LogError(ex.Message + "\n" + ex.StackTrace.ToString());
            }
            finally
            {
                if (null != mailmsg)
                    Marshal.ReleaseComObject(mailmsg);
            }
        }

        private void Log(string message)
        {
            Log(message, EventLogEntryType.Information);
        }

        private void Log(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(Assembly.GetExecutingAssembly().FullName,
              message, type);
        }

        private void LogDebug(string message)
        {
            if (Debug)
            {
                Log(message);
            }
        }

        private void LogError(string message)
        {
            Log(message, EventLogEntryType.Error);
        }
    }
}
