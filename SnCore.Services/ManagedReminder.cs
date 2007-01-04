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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitReminder : TransitService<Reminder>
    {
        private string mUrl;

        public string Url
        {
            get
            {

                return mUrl;
            }
            set
            {
                mUrl = value;
            }
        }

        private int mDeltaHours;

        public int DeltaHours
        {
            get
            {

                return mDeltaHours;
            }
            set
            {
                mDeltaHours = value;
            }
        }

        private string mDataObjectField;

        public string DataObjectField
        {
            get
            {

                return mDataObjectField;
            }
            set
            {
                mDataObjectField = value;
            }
        }

        private int mDataObject_Id;

        public int DataObject_Id
        {
            get
            {

                return mDataObject_Id;
            }
            set
            {
                mDataObject_Id = value;
            }
        }

        private bool mRecurrent;

        public bool Recurrent
        {
            get
            {

                return mRecurrent;
            }
            set
            {
                mRecurrent = value;
            }
        }

        private bool mEnabled;

        public bool Enabled
        {
            get
            {

                return mEnabled;
            }
            set
            {
                mEnabled = value;
            }
        }

        private int mInstanceEventCount;

        public int ReminderEventCount
        {
            get
            {

                return mInstanceEventCount;
            }
            set
            {
                mInstanceEventCount = value;
            }
        }

        private DateTime mLastRun;

        public DateTime LastRun
        {
            get
            {

                return mLastRun;
            }
            set
            {
                mLastRun = value;
            }
        }

        private string mLastRunError;

        public string LastRunError
        {
            get
            {

                return mLastRunError;
            }
            set
            {
                mLastRunError = value;
            }
        }

        public TransitReminder()
        {

        }

        public TransitReminder(Reminder value)
            : base(value)
        {

        }

        public override void SetInstance(Reminder instance)
        {
            Url = instance.Url;
            DeltaHours = instance.DeltaHours;
            DataObjectField = instance.DataObjectField;
            DataObject_Id = instance.DataObject.Id;
            Recurrent = instance.Recurrent;
            Enabled = instance.Enabled;
            ReminderEventCount = instance.ReminderEvents.Count;
            LastRun = instance.LastRun;
            LastRunError = instance.LastRunError;
            base.SetInstance(instance);
        }

        public override Reminder GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Reminder instance = base.GetInstance(session, sec);
            instance.Url = this.Url;
            instance.DeltaHours = this.DeltaHours;
            instance.DataObjectField = this.DataObjectField;
            if (this.DataObject_Id > 0) instance.DataObject = (DataObject)session.Load(typeof(DataObject), this.DataObject_Id);
            instance.Recurrent = this.Recurrent;
            instance.Enabled = this.Enabled;
            instance.LastRun = this.LastRun;
            instance.LastRunError = this.LastRunError;
            return instance;
        }
    }

    public class ManagedReminder : ManagedService<Reminder, TransitReminder>
    {
        public ManagedReminder()
        {
        
        }

        public ManagedReminder(ISession session)
            : base(session)
        {

        }

        public ManagedReminder(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedReminder(ISession session, Reminder value)
            : base(session, value)
        {

        }

        public bool CanSend(Account account)
        {
            return CanSend(account.Id);
        }

        public bool CanSend(int account_id)
        {
            foreach (ReminderAccountProperty rap in Collection<ReminderAccountProperty>.GetSafeCollection(mInstance.ReminderAccountProperties))
            {
                // find the account property value
                AccountPropertyValue apv = (AccountPropertyValue)Session.CreateCriteria(typeof(AccountPropertyValue))
                    .Add(Expression.Eq("Account.Id", account_id))
                    .Add(Expression.Eq("AccountProperty.Id", rap.AccountProperty.Id))
                    .SetMaxResults(1)
                    .UniqueResult();

                if (apv == null && rap.Unset)
                {
                    // account doesn't have this property value, but the reminder is set 
                    // to include accounts that don't have the property set
                    return true;
                }

                if (apv.Value != rap.Value)
                {
                    // property value doesn't match
                    return false;
                }
            }

            return true;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }
}
