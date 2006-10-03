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
using System.Reflection;
using SnCore.Tools.Web;

namespace SnCore.BackEndServices
{
    public class SystemReminderService : SystemService
    {
        private int mChunkSize = 50;

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

        public SystemReminderService()
        {

        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunCleanupStaleAccounts));
            AddJob(new SessionJobDelegate(RunInvitationReminders));
            AddJob(new SessionJobDelegate(RunSystemReminders));
        }

        public void RunCleanupStaleAccounts(ISession session)
        {
            // fetch accounts that have not been logged in for two months and that don't have a verified e-mail
            IList accounts = session.CreateQuery(
                string.Format(
                 "FROM Account account" +
                 " WHERE NOT EXISTS ( " +
                  " FROM AccountEmail AS email" +
                  " WHERE email.Account = account" + 
                  " AND email.Verified = 0" + 
                 ") AND account.LastLogin < '{0}'", DateTime.UtcNow.AddMonths(-2)))
                .SetMaxResults(ChunkSize) // avoid draining resources
                .List();

            foreach(Account account in accounts)
            {
                ManagedAccount ma = new ManagedAccount(session, account);
                ma.Delete();
            }

            session.Flush();
        }

        public void RunInvitationReminders(ISession session)
        {
            // fetch invitations that are older than a month
            IList invitations = session.CreateCriteria(typeof(AccountInvitation))
                .Add(Expression.Le("Modified", DateTime.UtcNow.AddMonths(-1)))
                .SetMaxResults(ChunkSize) // avoid draining resources
                .List();

            foreach (AccountInvitation invitation in invitations)
            {
                if (invitation.Created == invitation.Modified)
                {
                    try
                    {
                        // this invitation was never resent
                        ManagedAccountInvitation mi = new ManagedAccountInvitation(session, invitation);
                        mi.Send();
                    }
                    finally
                    {
                        invitation.Modified = DateTime.UtcNow;
                        session.Save(invitation);
                    }
                }
                else
                {
                    // delete an invitation that has not been responded to for over a month
                    session.Delete(invitation);
                }
            }
        }

        public void RunSystemReminders(ISession session)
        {
            // get reminders
            IList reminders = session.CreateCriteria(typeof(Reminder))
                .Add(Expression.Eq("Enabled", true))
                .List();

            foreach (Reminder reminder in reminders)
            {
                reminder.LastRun = DateTime.UtcNow;
                reminder.LastRunError = string.Empty;
                session.Save(reminder);

                try
                {
                    // get the type of the object seeked
                    Type objecttype = Assembly.GetAssembly(typeof(DataObject))
                        .GetType(reminder.DataObject.Name, true);

                    // todo: currently this works with date-based fields only

                    // anything older than this time
                    DateTime timeboundary = DateTime.UtcNow.AddHours(-reminder.DeltaHours);

                    // find all records matching the property
                    IList objects = session.CreateCriteria(objecttype)
                        .Add(Expression.Le(reminder.DataObjectField, timeboundary))
                        .List();

                    // currently only support account identities
                    // the object is either an Account object or has an AccountId property
                    string accountidproperty = (reminder.DataObject.Name == "Account") ? "Id" : "AccountId";

                    PropertyInfo accountidpropertyinfo = objecttype.GetProperty(accountidproperty);

                    if (accountidpropertyinfo == null)
                    {
                        throw new Exception(string.Format("Object {0} does not have a property {1}.",
                            reminder.DataObject.Name, accountidproperty));
                    }

                    foreach (object o in objects)
                    {
                        int accountid = (int)accountidpropertyinfo.GetValue(o, null);

                        ReminderEvent reminderevent = (ReminderEvent)session.CreateCriteria(typeof(ReminderEvent))
                            .Add(Expression.Eq("Reminder.Id", reminder.Id))
                            .Add(Expression.Eq("Account.Id", accountid))
                            .UniqueResult();

                        Account acct = (Account)session.Load(typeof(Account), accountid);

                        if (reminderevent == null)
                        {
                            reminderevent = new ReminderEvent();
                            reminderevent.Account = acct;
                            reminderevent.Reminder = reminder;
                            reminderevent.Created = reminderevent.Modified = DateTime.UtcNow;
                        }
                        else
                        {
                            if (reminderevent.Modified >= timeboundary)
                            {
                                // this field was already noticed and event was fired in a prior run
                                continue;
                            }

                            if (!reminder.Recurrent)
                            {
                                // this reminder has already been sent but is not recurrent
                                continue;
                            }

                            reminderevent.Modified = DateTime.UtcNow;
                        }

                        ManagedAccount ma = new ManagedAccount(session, acct);

                        if (!string.IsNullOrEmpty(ma.ActiveEmailAddress))
                        {
                            ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                                session,
                                new MailAddress(ma.ActiveEmailAddress, ma.Name).ToString(),
                                string.Format("{0}?id={1}", reminder.Url, ma.Id));
                        }

                        session.Save(reminderevent);
                    }
                }
                catch (Exception ex)
                {
                    reminder.LastRunError = ex.Message;
                    session.Save(reminder);
                }

                Thread.Sleep(1000 * InterruptInterval);
            }
        }
    }

}
