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
    public class TransitReminderEvent : TransitService<ReminderEvent>
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

        public TransitReminderEvent(ReminderEvent instance)
            : base(instance)
        {

        }

        public override void SetInstance(ReminderEvent instance)
        {
            ReminderId = instance.Reminder.Id;
            AccountId = instance.Account.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override ReminderEvent GetInstance(ISession session, ManagedSecurityContext sec)
        {
            ReminderEvent instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Reminder = (Reminder)session.Load(typeof(Reminder), ReminderId);
                instance.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            return instance;
        }
    }

    public class ManagedReminderEvent : ManagedService<ReminderEvent, TransitReminderEvent>
    {
        public ManagedReminderEvent(ISession session)
            : base(session)
        {

        }

        public ManagedReminderEvent(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedReminderEvent(ISession session, ReminderEvent value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }
}
