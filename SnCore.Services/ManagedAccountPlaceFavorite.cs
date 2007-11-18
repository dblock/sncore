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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountPlaceFavorite : TransitService<AccountPlaceFavorite>
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

        private int mPlaceId = 0;

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

        private int mAccountPictureId;

        public int AccountPictureId
        {
            get
            {

                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
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

        private int mPlacePictureId;

        public int PlacePictureId
        {
            get
            {

                return mPlacePictureId;
            }
            set
            {
                mPlacePictureId = value;
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

        public TransitAccountPlaceFavorite()
        {

        }

        public TransitAccountPlaceFavorite(AccountPlaceFavorite instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountPlaceFavorite instance)
        {
            Created = instance.Created;
            AccountId = instance.Account.Id;
            PlaceId = instance.Place.Id;
            AccountName = instance.Account.Name;
            PlaceName = instance.Place.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account);
            PlacePictureId = ManagedService<PlacePicture, TransitPlacePicture>.GetRandomElementId(instance.Place.PlacePictures);
            if (instance.Place.City != null) PlaceCity = instance.Place.City.Name;
            if (instance.Place.City != null && instance.Place.City.State != null) PlaceState = instance.Place.City.State.Name;
            if (instance.Place.City != null && instance.Place.City.Country != null) PlaceCountry = instance.Place.City.Country.Name;
            base.SetInstance(instance);
        }

        public override AccountPlaceFavorite GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPlaceFavorite instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                // the account and the place cannot be switched after the relationship is created
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Place = session.Load<Place>(PlaceId);
            }

            return instance;
        }
    }

    public class ManagedAccountPlaceFavorite : ManagedService<AccountPlaceFavorite, TransitAccountPlaceFavorite>, IAuditableService
    {
        public ManagedAccountPlaceFavorite()
        {

        }

        public ManagedAccountPlaceFavorite(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceFavorite(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPlaceFavorite(ISession session, AccountPlaceFavorite value)
            : base(session, value)
        {

        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountPlaceFavorite>.GetSafeCollection(mInstance.Account.AccountPlaceFavorites).Remove(mInstance);
            Collection<AccountPlaceFavorite>.GetSafeCollection(mInstance.Place.AccountPlaceFavorites).Remove(mInstance);
            base.Delete(sec);
        }

        public Place Place
        {
            get
            {
                return mInstance.Place;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow; 
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        public IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] has added [place:{1}] to his/her favorites",
                        mInstance.Account.Id, mInstance.Place.Id),
                        string.Format("PlaceView.aspx?id={0}", mInstance.Place.Id)));
                    break;
            }
            return result;
        }
    }
}
