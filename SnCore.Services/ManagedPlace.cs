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
using SnCore.Tools.Web;
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitDistinctPlaceNeighborhood : TransitService
    {
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

        private long mCount;

        public long Count
        {
            get
            {
                return mCount;
            }
            set
            {
                mCount = value;
            }
        }

        public TransitDistinctPlaceNeighborhood()
        {

        }

        public TransitDistinctPlaceNeighborhood(int id)
            : base(id)
        {

        }
    }

    public class TransitPlaceQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public string Country;
        public string State;
        public string City;
        public string Neighborhood;
        public string Name;
        public string Type;
        public bool PicturesOnly = false;
        public int AccountId = 0;

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }

        public TransitPlaceQueryOptions()
        {
        }

        public string CreateSubQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(Neighborhood))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.Neighborhood.Id = '{0}'", ManagedNeighborhood.GetNeighborhoodId(session, Neighborhood, City, State, Country));
            }

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.City.Id = '{0}'", ManagedCity.GetCityId(session, City, State, Country));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.City.Country.Id = {0}", ManagedCountry.GetCountryId(session, Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.City.State.Id = {0}", ManagedState.GetStateId(session, State, Country));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Type))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.Type.Id = {0}", ManagedPlaceType.FindId(session, Type));
            }

            if (AccountId != 0)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("p.Account.Id = {0}", AccountId);
            }

            if (PicturesOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("EXISTS ELEMENTS(p.PlacePictures)");
            }

            return b.ToString();
        }

        public IQuery CreateCountQuery(ISession session)
        {
            return session.CreateQuery("SELECT COUNT(*) FROM Place p " + CreateSubQuery(session));
        }

        public IQuery CreateQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT p FROM Place p");
            b.Append(CreateSubQuery(session));
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return session.CreateQuery(b.ToString());
        }
    };

    [Serializable()]
    public class TransitPlace : TransitService
    {
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

        private string mType;

        public string Type
        {
            get
            {

                return mType;
            }
            set
            {
                mType = value;
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

        private string mStreet;

        public string Street
        {
            get
            {

                return mStreet;
            }
            set
            {
                mStreet = value;
            }
        }

        private string mZip;

        public string Zip
        {
            get
            {

                return mZip;
            }
            set
            {
                mZip = value;
            }
        }

        private string mCrossStreet;

        public string CrossStreet
        {
            get
            {

                return mCrossStreet;
            }
            set
            {
                mCrossStreet = value;
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

        private string mFax;

        public string Fax
        {
            get
            {

                return mFax;
            }
            set
            {
                mFax = value;
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

        private string mNeighborhood;

        public string Neighborhood
        {
            get
            {
                return mNeighborhood;
            }
            set
            {
                mNeighborhood = value;
            }
        }

        private string mCity;

        public string City
        {
            get
            {

                return mCity;
            }
            set
            {
                mCity = value;
            }
        }

        private string mState;

        public string State
        {
            get
            {

                return mState;
            }
            set
            {
                mState = value;
            }
        }

        private string mCountry;

        public string Country
        {
            get
            {

                return mCountry;
            }
            set
            {
                mCountry = value;
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

        private bool mCanWrite = false;

        public bool CanWrite
        {
            get
            {
                return mCanWrite;
            }
            set
            {
                mCanWrite = value;
            }
        }

        public TransitPlace()
        {

        }

        public TransitPlace(Place o)
            : base(o.Id)
        {
            Name = o.Name;
            Type = o.Type.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            Street = o.Street;
            Zip = o.Zip;
            CrossStreet = o.CrossStreet;
            Phone = o.Phone;
            Fax = o.Fax;
            Email = o.Email;
            Website = o.Website;
            if (o.City != null) City = o.City.Name;
            if (o.City != null && o.City.State != null) State = o.City.State.Name;
            if (o.City != null && o.City.Country != null) Country = o.City.Country.Name;
            if (o.Neighborhood != null) Neighborhood = o.Neighborhood.Name;
            PictureId = ManagedService<PlacePicture>.GetRandomElementId(o.PlacePictures);
            AccountId = o.Account.Id;
        }

        public Place GetPlace(ISession session)
        {
            Place p = (Id != 0) ? (Place)session.Load(typeof(Place), Id) : new Place();
            p.Name = this.Name;
            p.Type = ManagedPlaceType.Find(session, this.Type);
            p.Description = this.Description;
            p.Street = this.Street;
            p.Zip = this.Zip;
            p.CrossStreet = this.CrossStreet;
            p.Phone = this.Phone;
            p.Fax = this.Fax;
            p.Email = this.Email;
            p.Website = this.Website;
            if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            if (!string.IsNullOrEmpty(City)) p.City = ManagedCity.FindOrCreate(session, City, State, Country);
            if (!string.IsNullOrEmpty(Neighborhood) && !string.IsNullOrEmpty(City)) p.Neighborhood = ManagedNeighborhood.FindOrCreate(session, Neighborhood, City, State, Country);
            return p;
        }
    }

    /// <summary>
    /// Managed place.
    /// </summary>
    public class ManagedPlace : ManagedService<Place>
    {
        private Place mPlace = null;

        public ManagedPlace(ISession session)
            : base(session)
        {

        }

        public ManagedPlace(ISession session, int id)
            : base(session)
        {
            mPlace = (Place)session.Load(typeof(Place), id);
        }

        public ManagedPlace(ISession session, Place value)
            : base(session)
        {
            mPlace = value;
        }

        public ManagedPlace(ISession session, TransitPlace value)
            : base(session)
        {
            mPlace = value.GetPlace(session);
        }

        public int Id
        {
            get
            {
                return mPlace.Id;
            }
        }

        public TransitPlace GetTransitPlace()
        {
            return new TransitPlace(mPlace);
        }

        public TransitPlace GetTransitPlace(int user_id)
        {
            TransitPlace tp = new TransitPlace(mPlace);
            if (user_id > 0) tp.CanWrite = CanWrite(user_id);
            return tp;
        }

        public void Delete()
        {
            try
            {
                int DiscussionId = ManagedDiscussion.GetDiscussionId(
                    Session, mPlace.Account.Id, ManagedDiscussion.PlaceDiscussion, mPlace.Id, false);
                Discussion mDiscussion = (Discussion)Session.Load(typeof(Discussion), DiscussionId);
                Session.Delete(mDiscussion);
            }
            catch (ManagedDiscussion.DiscussionNotFoundException)
            {

            }

            if (mPlace.PlacePictures != null)
            {
                foreach (PlacePicture pic in mPlace.PlacePictures)
                {
                    new ManagedPlacePicture(Session, pic).Delete();
                }
            }

            if (mPlace.AccountEvents != null)
            {
                foreach (AccountEvent evt in mPlace.AccountEvents)
                {
                    new ManagedAccountEvent(Session, evt).Delete();
                }
            }

            Session.Delete(string.Format("FROM AccountPlace f WHERE f.Place.Id = {0}", Id));
            Session.Delete(string.Format("FROM AccountPlaceRequest f WHERE f.Place.Id = {0}", Id));
            Session.Delete(string.Format("FROM AccountPlaceFavorite f WHERE f.Place.Id = {0}", Id));
            Session.Delete(string.Format("FROM PlaceQueueItem q WHERE q.Place.Id = {0}", Id));
            ManagedFeature.Delete(Session, "Place", Id);
            Session.Delete(mPlace);
        }

        public bool CanWrite(int accountid)
        {
            // person suggesting a place can write
            if (mPlace.Account.Id == accountid)
                return true;

            if (mPlace.AccountPlaces == null)
                return false;

            foreach (AccountPlace ap in mPlace.AccountPlaces)
            {
                if (ap.Account.Id == accountid && ap.Type.CanWrite)
                {
                    return true;
                }
            }

            return false;
        }

        public void MigrateToAccount(Account newowner)
        {
            // migrate pictures
            IList pictures = Session.CreateCriteria(typeof(PlacePicture))
                .Add(Expression.Eq("Account.Id", mPlace.Account.Id))
                .Add(Expression.Eq("Place.Id", mPlace.Id))
                .List();

            foreach (PlacePicture pp in pictures)
            {
                ManagedPlacePicture mpp = new ManagedPlacePicture(Session, pp);
                mpp.MigrateToAccount(newowner);
            }

            // migrate review discussion
            int did = ManagedDiscussion.GetDiscussionId(
                Session, mPlace.Account.Id, ManagedDiscussion.PlaceDiscussion, mPlace.Id, false);

            ManagedDiscussion md = new ManagedDiscussion(Session, did);
            md.MigrateToAccount(newowner);

            mPlace.Account = newowner;
            Session.Save(mPlace);
        }

        #region Place Property Value

        public int CreateOrUpdate(TransitPlacePropertyValue o)
        {
            PlacePropertyValue propertyvalue = o.GetPlacePropertyValue(Session);

            if (propertyvalue.Id != 0)
            {
                if (propertyvalue.Place.Id != mPlace.Id)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            propertyvalue.Modified = DateTime.UtcNow;
            if (propertyvalue.Id == 0) propertyvalue.Created = propertyvalue.Modified;
            Session.Save(propertyvalue);
            return propertyvalue.Id;
        }

        #endregion
    }
}
