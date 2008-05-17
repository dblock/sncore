using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using SnCore.Data.Hibernate;
using NHibernate;
using NHibernate.Expression;
using MIME;
using SnCore.Services;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace SnCore.BackEndServices
{
    public class SystemSMTPMessageService : SystemService
    {
        public static string Name = "SnCore System SMTP Queue Service";
        public static string Description = "Routes NDRs processed and queued by the SMTP sink.";

        public SystemSMTPMessageService()
        {
            ServiceName = Name;
        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunQueue));
        }

        private void UpdateFailureEmails(MimeDSNRecipient r, ISession session)
        {
            IList<AccountEmail> emails = session.CreateCriteria(typeof(AccountEmail))
                .Add(Expression.Eq("Address", r.FinalRecipientEmailAddress))
                .List<AccountEmail>();

            if (emails == null)
                return;

            foreach (AccountEmail email in emails)
            {
                if (email.Failed)
                    continue;

                EventLogManager.WriteEntry(string.Format("Marked {0} [{1}] (id:{2}) with failure [{3}].",
                    email.Account.Name, email.Address, email.Id, r.DiagnosticCode));

                // check whether there're pending invitations for this e-mail
                foreach (AccountEmailConfirmation confirmation in Collection<AccountEmailConfirmation>.GetSafeCollection(email.AccountEmailConfirmations))
                {
                    session.Delete(confirmation);
                }

                email.Failed = true;
                email.LastError = r.DiagnosticCode;
                email.Modified = DateTime.UtcNow;
                session.Save(email);
                session.Flush();
            }
        }

        private void UpdateFailureInvitations(MimeDSNRecipient r, ISession session)
        {
            IList<AccountInvitation> invitations = session.CreateCriteria(typeof(AccountInvitation))
                .Add(Expression.Eq("Email", r.FinalRecipientEmailAddress))
                .List<AccountInvitation>();

            if (invitations == null)
                return;

            foreach (AccountInvitation invitation in invitations)
            {
                if (invitation.Failed)
                    continue;

                EventLogManager.WriteEntry(string.Format("Marked \"{0}\" [invited by {1}] (id:{2}) with failure [{3}].",
                    invitation.Email, invitation.Account.Name, invitation.Id, r.DiagnosticCode));

                invitation.Failed = true;
                invitation.LastError = r.DiagnosticCode;
                invitation.Modified = DateTime.UtcNow;
                session.Save(invitation);
                session.Flush();
            }
        }

        private void UpdateFailure(MimeDSNRecipient r)
        {
            ISession session = SnCore.Data.Hibernate.Session.Factory.OpenSession();
            try
            {
                EventLogManager.WriteEntry(string.Format("Searching for {0}.", r.FinalRecipientEmailAddress));
                UpdateFailureEmails(r, session);
                UpdateFailureInvitations(r, session);
            }
            finally
            {
                session.Close();
            }
        }

        public void RunQueue(ISession session, ManagedSecurityContext sec)
        {
            string path = ManagedConfiguration.GetValue(session, "SnCore.SMTP.Queue",
                Path.Combine(Path.GetTempPath(), "Queue"));

            EventLogManager.WriteEntry(string.Format("Processing SMTP queue in {0}.", path));

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                EventLogManager.WriteEntry(string.Format("Processing {0}.", file));
                
                try
                {
                    ProcessMessage(file);
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    EventLogManager.WriteEntry(string.Format("Error processing {0}: {1}",
                        file, ex.Message), EventLogEntryType.Warning);
                }

                Thread.Sleep(1000 * InterruptInterval);
            }
        }

        private void ProcessMessage(string filename)
        {
            MimeMessage msg = new MimeMessage();
            msg.ReadFromFile(filename);

            ArrayList bodylist = new ArrayList();
            msg.GetBodyPartList(bodylist);
            EventLogManager.WriteEntry(string.Format("Loaded {0} parts of message \"{1}\".", 
                bodylist.Count, filename));

            List<MimeDSNRecipient> failures = new List<MimeDSNRecipient>();

            for (int i = 0; i < bodylist.Count; i++)
            {
                MimeBody ab = (MimeBody)bodylist[i];
                EventLogManager.WriteEntry(string.Format("Parsing body part {0}: \"{1}\" ({2}).", i, ab.GetName(), ab.GetContentType()));
                switch (ab.GetContentType().ToLower())
                {
                    case "message/delivery-status":
                        /// TODO: move to Mime processor
                        MimeDSN dsn = new MimeDSN();
                        dsn.LoadBody(ab.GetText());
                        foreach (MimeDSNRecipient r in dsn.Recipients)
                        {
                            EventLogManager.WriteEntry(string.Format("Checking {0} ({1}).", r.FinalRecipientEmailAddress, r.Action));
                            switch (r.Action)
                            {
                                case "failed":
                                    failures.Add(r);
                                    break;
                            }
                        }
                        break;
                }
            }

            if (failures.Count > 0)
            {
                foreach (MimeDSNRecipient r in failures)
                {
                    EventLogManager.WriteEntry(string.Format("Processing {0} ({1}).",
                        r.FinalRecipientEmailAddress, r.Action));
                    UpdateFailure(r);
                }
            }

            EventLogManager.WriteEntry(string.Format("Processed message \"{0}\".", filename));
        }
    }
}
