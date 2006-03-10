using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;

namespace SnCore.Services
{
    public class TransitReminderEvent : TransitService
    {
        private int mReminderId;

        public int ReminderId
        {
            get
            {

                return mReminderId;
            }
            set
            {
                mReminderId = value;
            }
        }

        private int mAccountId;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        public TransitReminderEvent()
        {

        }

        public TransitReminderEvent(ReminderEvent o)
            : base(o.Id)
        {
            ReminderId = o.Reminder.Id;
            AccountId = o.Account.Id;
            Created = o.Created;
            Modified = o.Modified;
        }

        public ReminderEvent GetReminderEvent(ISession session)
        {
            ReminderEvent p = (Id != 0) ? (ReminderEvent)session.Load(typeof(ReminderEvent), Id) : new ReminderEvent();

            if (Id == 0)
            {
                if (ReminderId > 0) p.Reminder = (Reminder)session.Load(typeof(Reminder), ReminderId);
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            return p;
        }
    }

    /// <summary>
    /// Managed ReminderEvent.
    /// </summary>
    public class ManagedReminderEvent : ManagedService
    {
        private ReminderEvent mReminderEvent = null;

        public ManagedReminderEvent(ISession session)
            : base(session)
        {

        }

        public ManagedReminderEvent(ISession session, int id)
            : base(session)
        {
            mReminderEvent = (ReminderEvent)session.Load(typeof(ReminderEvent), id);
        }

        public ManagedReminderEvent(ISession session, ReminderEvent value)
            : base(session)
        {
            mReminderEvent = value;
        }

        public ManagedReminderEvent(ISession session, TransitReminderEvent value)
            : base(session)
        {
            mReminderEvent = value.GetReminderEvent(session);
        }

        public int Id
        {
            get
            {
                return mReminderEvent.Id;
            }
        }

        public TransitReminderEvent TransitReminderEvent
        {
            get
            {
                return new TransitReminderEvent(mReminderEvent);
            }
        }

        public void CreateOrUpdate(TransitReminderEvent o)
        {
            mReminderEvent = o.GetReminderEvent(Session);
            mReminderEvent.Modified = DateTime.UtcNow;
            if (mReminderEvent.Id == 0) mReminderEvent.Created = mReminderEvent.Modified;
            Session.Save(mReminderEvent);
        }

        public void Delete()
        {
            Session.Delete(mReminderEvent);
        }
    }
}
