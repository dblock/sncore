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
    public class TransitReminderAccountProperty : TransitService
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

        public TransitReminderAccountProperty(ReminderAccountProperty o)
            : base(o.Id)
        {
            ReminderId = o.Reminder.Id;
            AccountPropertyId = o.AccountProperty.Id;
            AccountPropertyName = o.AccountProperty.Name;
            Value = o.Value;
            Unset = o.Unset;
        }

        public ReminderAccountProperty GetReminderAccountProperty(ISession session)
        {
            ReminderAccountProperty p = (Id != 0) ? (ReminderAccountProperty)session.Load(typeof(ReminderAccountProperty), Id) : new ReminderAccountProperty();
            if (this.ReminderId > 0) p.Reminder = (Reminder)session.Load(typeof(Reminder), this.ReminderId);
            if (this.AccountPropertyId > 0) p.AccountProperty = (AccountProperty)session.Load(typeof(AccountProperty), this.AccountPropertyId);
            p.Value = this.Value;
            p.Unset = this.Unset;
            return p;
        }
    }

    /// <summary>
    /// Managed ReminderAccountProperty.
    /// </summary>
    public class ManagedReminderAccountProperty : ManagedService
    {
        private ReminderAccountProperty mReminderAccountProperty = null;

        public ManagedReminderAccountProperty(ISession session)
            : base(session)
        {

        }

        public ManagedReminderAccountProperty(ISession session, int id)
            : base(session)
        {
            mReminderAccountProperty = (ReminderAccountProperty)session.Load(typeof(ReminderAccountProperty), id);
        }

        public ManagedReminderAccountProperty(ISession session, ReminderAccountProperty value)
            : base(session)
        {
            mReminderAccountProperty = value;
        }

        public ManagedReminderAccountProperty(ISession session, TransitReminderAccountProperty value)
            : base(session)
        {
            mReminderAccountProperty = value.GetReminderAccountProperty(session);
        }

        public int Id
        {
            get
            {
                return mReminderAccountProperty.Id;
            }
        }

        public TransitReminderAccountProperty TransitReminderAccountProperty
        {
            get
            {
                return new TransitReminderAccountProperty(mReminderAccountProperty);
            }
        }

        public void CreateOrUpdate(TransitReminderAccountProperty o)
        {
            mReminderAccountProperty = o.GetReminderAccountProperty(Session);
            Session.Save(mReminderAccountProperty);
        }

        public void Delete()
        {
            Session.Delete(mReminderAccountProperty);
        }
    }
}
