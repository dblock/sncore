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
        public SystemReminderService()
        {

        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunSystemReminders));
            AddJob(new SessionJobDelegate(RunInvitationReminders));
        }

        public void RunInvitationReminders(ISession session)
        {

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
