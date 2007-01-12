using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Expression;
using SnCore.Tools.Web;
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitAccountEventInstanceQueryOptions
    {
        public string SortOrder = "StartDateTime";
        public bool SortAscending = true;
        public string Country;
        public string State;
        public string City;
        public string Neighborhood;
        public string Name;
        public string Type;
        public DateTime StartDateTime = DateTime.MinValue;
        public DateTime EndDateTime = DateTime.MaxValue;

        public TransitAccountEventInstanceQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(Neighborhood))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Place.Neighborhood.Name = '{0}'", Renderer.SqlEncode(Neighborhood));
            }

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.Name = '{0}'", Renderer.SqlEncode(City));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.Country.Name = {0}", Renderer.SqlEncode(Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.State.Name = {0}", Renderer.SqlEncode(State));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Type))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.AccountEventType.Name = '{0}'", Renderer.SqlEncode(Type));
            }

            if (StartDateTime != DateTime.MinValue)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("si.StartDateTime < '{0}'", EndDateTime);
            }

            if (EndDateTime != DateTime.MaxValue)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("si.EndDateTime > '{0}'", StartDateTime);
            }

            // exclude non-published events
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Publish = 1");
            }

            // join on Schedule and ScheduleInstance
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Schedule.Id = si.Schedule.Id");
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return ", ScheduleInstance si " + CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT si FROM AccountEvent AccountEvent, ScheduleInstance si ");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }
            return b.ToString();
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountEventInstance : TransitService<ScheduleInstance>
    {
        private int mAccountEventId = 0;

        public int AccountEventId
        {
            get
            {
                return mAccountEventId;
            }
            set
            {
                mAccountEventId = value;
            }
        }

        private int mPictureId = 0;

        public int PictureId
        {
            get
            {

                return mPictureId;
            }
            set
            {
                mPictureId = value;
            }
        }

        private int mPlaceId;

        public int PlaceId
        {
            get
            {
                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
            }
        }

        private string mPlaceName;

        public string PlaceName
        {
            get
            {
                return mPlaceName;
            }
            set
            {
                mPlaceName = value;
            }
        }

        private string mPlaceNeighborhood;

        public string PlaceNeighborhood
        {
            get
            {
                return mPlaceNeighborhood;
            }
            set
            {
                mPlaceNeighborhood = value;
            }
        }

        private string mPlaceCity;

        public string PlaceCity
        {
            get
            {
                return mPlaceCity;
            }
            set
            {
                mPlaceCity = value;
            }
        }

        private string mPlaceState;

        public string PlaceState
        {
            get
            {
                return mPlaceState;
            }
            set
            {
                mPlaceState = value;
            }
        }

        private string mPlaceCountry;

        public string PlaceCountry
        {
            get
            {
                return mPlaceCountry;
            }
            set
            {
                mPlaceCountry = value;
            }
        }

        private string mAccountEventType;

        public string AccountEventType
        {
            get
            {

                return mAccountEventType;
            }
            set
            {
                mAccountEventType = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private int mAccountId = 0;

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

        private string mAccountName;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
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

        private string mCost;

        public string Cost
        {
            get
            {
                return mCost;
            }
            set
            {
                mCost = value;
            }
        }

        private string mWebsite;

        public string Website
        {
            get
            {
                return mWebsite;
            }
            set
            {
                mWebsite = value;
            }
        }

        private string mEmail;

        public string Email
        {
            get
            {
                return mEmail;
            }
            set
            {
                mEmail = value;
            }
        }

        private string mPhone;

        public string Phone
        {
            get
            {
                return mPhone;
            }
            set
            {
                mPhone = value;
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

        private int mInstance;

        public int Instance
        {
            get
            {
                return mInstance;
            }
            set
            {
                mInstance = value;
            }
        }

        public TransitAccountEventInstance()
        {

        }

        public TransitAccountEventInstance(ScheduleInstance instance)
            : base(instance)
        {

        }

        public override void SetInstance(ScheduleInstance instance)
        {
            if (instance.Schedule.AccountEvents == null || instance.Schedule.AccountEvents.Count != 1)
            {
                throw new Exception(string.Format("Orphaned schedule instance {0}.", instance.Id));
            }

            AccountEvent evt = (AccountEvent)instance.Schedule.AccountEvents[0];

            AccountEventId = evt.Id;
            AccountEventType = evt.AccountEventType.Name;
            Description = evt.Description;
            Created = evt.Created;
            Modified = evt.Modified;
            AccountId = evt.Account.Id;
            AccountName = evt.Account.Name;
            ScheduleId = evt.Schedule.Id;
            PlaceId = evt.Place.Id;
            PlaceName = evt.Place.Name;
            if (evt.Place.City != null) PlaceCity = evt.Place.City.Name;
            if (evt.Place.City != null && evt.Place.City.Country != null) PlaceCountry = evt.Place.City.Country.Name;
            if (evt.Place.City != null && evt.Place.City.State != null) PlaceState = evt.Place.City.State.Name;
            if (evt.Place.Neighborhood != null) PlaceNeighborhood = evt.Place.Neighborhood.Name;
            Name = evt.Name;
            Phone = evt.Phone;
            Email = evt.Email;
            Website = evt.Website;
            Cost = evt.Cost;
            PictureId = ManagedService<AccountEventPicture, TransitAccountEventPicture>.GetRandomElementId(evt.AccountEventPictures);

            StartDateTime = instance.StartDateTime;
            EndDateTime = instance.EndDateTime;
            Instance = instance.Instance;

            base.SetInstance(instance);
        }

        public override ScheduleInstance GetInstance(ISession session, ManagedSecurityContext sec)
        {
            ScheduleInstance instance = base.GetInstance(session, sec);
            if (ScheduleId > 0) instance.Schedule = (Schedule) session.Load(typeof(Schedule), ScheduleId);
            instance.EndDateTime = EndDateTime;
            instance.StartDateTime = StartDateTime;
            return instance;
        }
    }
}
