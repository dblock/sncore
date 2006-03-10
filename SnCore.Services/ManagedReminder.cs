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
    public class TransitReminder : TransitService
    {
        private string mSubject;

        public string Subject
        {
            get
            {

                return mSubject;
            }
            set
            {
                mSubject = value;
            }
        }

        private string mBody;

        public string Body
        {
            get
            {

                return mBody;
            }
            set
            {
                mBody = value;
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

        private int mReminderEventCount;

        public int ReminderEventCount
        {
            get
            {

                return mReminderEventCount;
            }
            set
            {
                mReminderEventCount = value;
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

        public TransitReminder(Reminder o)
            : base(o.Id)
        {
            Subject = o.Subject;
            Body = o.Body;
            DeltaHours = o.DeltaHours;
            DataObjectField = o.DataObjectField;
            DataObject_Id = o.DataObject.Id;
            Recurrent = o.Recurrent;
            Enabled = o.Enabled;
            ReminderEventCount = o.ReminderEvents.Count;
            LastRun = o.LastRun;
            LastRunError = o.LastRunError;
        }

        public Reminder GetReminder(ISession session)
        {
            Reminder p = (Id != 0) ? (Reminder)session.Load(typeof(Reminder), Id) : new Reminder();
            p.Subject = this.Subject;
            p.Body = this.Body;
            p.DeltaHours = this.DeltaHours;
            p.DataObjectField = this.DataObjectField;
            if (this.DataObject_Id > 0) p.DataObject = (DataObject)session.Load(typeof(DataObject), this.DataObject_Id);
            p.Recurrent = this.Recurrent;
            p.Enabled = this.Enabled;
            p.LastRun = this.LastRun;
            p.LastRunError = this.LastRunError;
            return p;
        }
    }

    /// <summary>
    /// Managed reminder.
    /// </summary>
    public class ManagedReminder : ManagedService
    {
        private Reminder mReminder = null;

        public ManagedReminder(ISession session)
            : base(session)
        {

        }

        public ManagedReminder(ISession session, int id)
            : base(session)
        {
            mReminder = (Reminder)session.Load(typeof(Reminder), id);
        }

        public ManagedReminder(ISession session, Reminder value)
            : base(session)
        {
            mReminder = value;
        }

        public ManagedReminder(ISession session, TransitReminder value)
            : base(session)
        {
            mReminder = value.GetReminder(session);
        }

        public int Id
        {
            get
            {
                return mReminder.Id;
            }
        }

        public TransitReminder TransitReminder
        {
            get
            {
                return new TransitReminder(mReminder);
            }
        }

        public void CreateOrUpdate(TransitReminder o)
        {
            mReminder = o.GetReminder(Session);
            if (o.Id == 0) mReminder.LastRun = DateTime.UtcNow;
            Session.Save(mReminder);
        }

        public void Delete()
        {
            Session.Delete(mReminder);
        }
    }
}
