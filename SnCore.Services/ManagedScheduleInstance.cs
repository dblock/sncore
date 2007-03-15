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
    public class TransitScheduleInstance : TransitService<ScheduleInstance>
    {
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

        private DateTime mEndDateTime;

        public DateTime EndDateTime
        {
            get
            {
                return mEndDateTime;
            }
            set
            {
                mEndDateTime = value;
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

        private int mScheduleId;

        public int ScheduleId
        {
            get
            {
                return mScheduleId;
            }
            set
            {
                mScheduleId = value;
            }
        }

        private DateTime mStartDateTime;

        public DateTime StartDateTime
        {
            get
            {
                return mStartDateTime;
            }
            set
            {
                mStartDateTime = value;
            }
        }


        public TransitScheduleInstance()
        {

        }

        public TransitScheduleInstance(ScheduleInstance instance)
            : base(instance)
        {

        }

        public override void SetInstance(ScheduleInstance instance)
        {
            Created = instance.Created;
            Modified = instance.Modified;
            StartDateTime = instance.StartDateTime;
            EndDateTime = instance.EndDateTime;
            ScheduleId = instance.Schedule.Id;
            base.SetInstance(instance);
        }

        public override ScheduleInstance GetInstance(ISession session, ManagedSecurityContext sec)
        {
            ScheduleInstance instance = base.GetInstance(session, sec);
            instance.StartDateTime = StartDateTime;
            instance.EndDateTime = EndDateTime;
            if (Id == 0) instance.Schedule = session.Load<Schedule>(ScheduleId);
            return instance;
        }
    }

    public class ManagedScheduleInstance : ManagedService<ScheduleInstance, TransitScheduleInstance>
    {
        public ManagedScheduleInstance()
        {

        }

        public ManagedScheduleInstance(ISession session)
            : base(session)
        {

        }

        public ManagedScheduleInstance(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedScheduleInstance(ISession session, ScheduleInstance value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
