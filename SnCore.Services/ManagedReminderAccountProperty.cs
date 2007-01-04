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
    public class TransitReminderAccountProperty : TransitService<ReminderAccountProperty>
    {
        private string mValue;

        public string Value
        {
            get
            {

                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

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

        private int mAccountPropertyId;

        public int AccountPropertyId
        {
            get
            {
                return mAccountPropertyId;
            }
            set
            {
                mAccountPropertyId = value;
            }
        }

        private bool mUnset;

        public bool Unset
        {
            get
            {

                return mUnset;
            }
            set
            {
                mUnset = value;
            }
        }

        private string mAccountPropertyName;

        public string AccountPropertyName
        {
            get
            {
                return mAccountPropertyName;
            }
            set
            {
                mAccountPropertyName = value;
            }
        }

        public TransitReminderAccountProperty()
        {

        }

        public TransitReminderAccountProperty(ReminderAccountProperty instance)
            : base(instance)
        {

        }

        public override void SetInstance(ReminderAccountProperty instance)
        {
            ReminderId = instance.Reminder.Id;
            AccountPropertyId = instance.AccountProperty.Id;
            AccountPropertyName = instance.AccountProperty.Name;
            Value = instance.Value;
            Unset = instance.Unset;
            base.SetInstance(instance);
        }

        public override ReminderAccountProperty GetInstance(ISession session, ManagedSecurityContext sec)
        {
            ReminderAccountProperty instance = base.GetInstance(session, sec);
            if (this.ReminderId > 0) instance.Reminder = (Reminder)session.Load(typeof(Reminder), this.ReminderId);
            if (this.AccountPropertyId > 0) instance.AccountProperty = (AccountProperty)session.Load(typeof(AccountProperty), this.AccountPropertyId);
            instance.Value = this.Value;
            instance.Unset = this.Unset;
            return instance;
        }
    }

    public class ManagedReminderAccountProperty : ManagedService<ReminderAccountProperty, TransitReminderAccountProperty>
    {
        public ManagedReminderAccountProperty()
        {

        }

        public ManagedReminderAccountProperty(ISession session)
            : base(session)
        {

        }

        public ManagedReminderAccountProperty(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedReminderAccountProperty(ISession session, ReminderAccountProperty value)
            : base(session, value)
        {

        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }
}
