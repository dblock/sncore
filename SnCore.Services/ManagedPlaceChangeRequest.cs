using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;
using SnCore.Tools;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    [Serializable()]
    public class TransitPlaceChangeRequest : TransitService<PlaceChangeRequest>
    {
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

        public string mPlaceName;

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

        public TransitPlaceChangeRequest()
        {

        }

        public TransitPlaceChangeRequest(PlaceChangeRequest value)
            : base(value)
        {

        }

        public override void SetInstance(PlaceChangeRequest instance)
        {
            Name = instance.Name;
            Type = instance.Type.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            Street = instance.Street;
            Zip = instance.Zip;
            CrossStreet = instance.CrossStreet;
            Phone = instance.Phone;
            Fax = instance.Fax;
            Email = instance.Email;
            if (instance.City != null) City = instance.City.Name;
            if (instance.City != null && instance.City.State != null) State = instance.City.State.Name;
            if (instance.City != null && instance.City.Country != null) Country = instance.City.Country.Name;
            if (instance.Neighborhood != null) Neighborhood = instance.Neighborhood.Name;
            PictureId = ManagedService<PlacePicture, TransitPlacePicture>.GetRandomElementId(instance.Place.PlacePictures);
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            PlaceId = instance.Place.Id;
            PlaceName = instance.Place.Name;
            base.SetInstance(instance);
        }

        public override PlaceChangeRequest GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceChangeRequest instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Type = ManagedPlaceType.Find(session, this.Type);
            instance.Description = this.Description;
            instance.Street = this.Street;
            instance.Zip = this.Zip;
            instance.CrossStreet = this.CrossStreet;
            instance.Phone = this.Phone;
            instance.Fax = this.Fax;
            instance.Email = this.Email;
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.City = (!string.IsNullOrEmpty(City)) 
                ? ManagedCity.FindOrCreate(session, City, State, Country)
                : null;            
            instance.Neighborhood = (!string.IsNullOrEmpty(Neighborhood) && !string.IsNullOrEmpty(City)) 
                ? ManagedNeighborhood.FindOrCreate(session, Neighborhood, City, State, Country)
                : null;
            if (Id == 0) instance.Place = session.Load<Place>(PlaceId);
            return instance;
        }
    }

    public class ManagedPlaceChangeRequest : ManagedService<PlaceChangeRequest, TransitPlaceChangeRequest>
    {
        public ManagedPlaceChangeRequest()
        {

        }

        public ManagedPlaceChangeRequest(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceChangeRequest(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceChangeRequest(ISession session, PlaceChangeRequest value)
            : base(session, value)
        {

        }

        public override TransitPlaceChangeRequest GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitPlaceChangeRequest instance = base.GetTransitInstance(sec);
            return instance;
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
            acl.Add(new ACLAuthenticatedAllowCreate());
            if (mInstance != null)
            {
                acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
                acl.Add(new ACLAccount(mInstance.Place.Account, DataOperation.AllExceptUpdate));
                foreach (AccountPlace relationship in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
                {
                    if (relationship.Type.CanWrite)
                    {
                        acl.Add(new ACLAccount(relationship.Account, DataOperation.AllExceptUpdate));
                    }
                }
            }
            return acl;
        }

        protected override void Check(TransitPlaceChangeRequest t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerified();
        }

        public override int CreateOrUpdate(TransitPlaceChangeRequest t_instance, ManagedSecurityContext sec)
        {
            int id = base.CreateOrUpdate(t_instance, sec);
            Session.Flush();

            if (t_instance.Id == 0)
            {
                string uri = string.Format("EmailPlaceChangeRequestCreated.aspx?id={0}", id);
                ManagedAccount owner = new ManagedAccount(Session, mInstance.Place.Account);
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, owner, uri);

                foreach (AccountPlace place in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
                {
                    if (place.Type.CanWrite)
                    {
                        ManagedAccount acct = new ManagedAccount(Session, place.Account);
                        ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, acct, uri);
                    }
                }
            }

            return id;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            string uri = string.Format("EmailPlaceChangeRequestDeleted.aspx?id={0}", mInstance.Id);
            ManagedAccount owner = new ManagedAccount(Session, mInstance.Account);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, owner, uri);
            base.Delete(sec);
        }
    }
}
