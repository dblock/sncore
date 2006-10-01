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

namespace SnCore.Services
{
    public class TransitAccountPlaceFavorite : TransitService
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

        public TransitAccountPlaceFavorite(AccountPlaceFavorite o)
            : base(o.Id)
        {
            Created = o.Created;
            AccountId = o.Account.Id;
            PlaceId = o.Place.Id;
            AccountName = o.Account.Name;
            PlaceName = o.Place.Name;
            AccountPictureId = ManagedService.GetRandomElementId(o.Account.AccountPictures);
            PlacePictureId = ManagedService.GetRandomElementId(o.Place.PlacePictures);
            PlaceCity = o.Place.City.Name;
            PlaceState = o.Place.City.State.Name;
            PlaceCountry = o.Place.City.Country.Name;
        }

        public AccountPlaceFavorite GetAccountPlaceFavorite(ISession session)
        {
            AccountPlaceFavorite p = (Id != 0) ? (AccountPlaceFavorite)session.Load(typeof(AccountPlaceFavorite), Id) : new AccountPlaceFavorite();

            if (Id == 0)
            {
                // the account and the place cannot be switched after the relationship is created
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
                if (PlaceId > 0) p.Place = (Place)session.Load(typeof(Place), PlaceId);
            }

            return p;
        }
    }

    public class ManagedAccountPlaceFavorite : ManagedService
    {
        private AccountPlaceFavorite mAccountPlaceFavorite = null;

        public ManagedAccountPlaceFavorite(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceFavorite(ISession session, int id)
            : base(session)
        {
            mAccountPlaceFavorite = (AccountPlaceFavorite)session.Load(typeof(AccountPlaceFavorite), id);
        }

        public ManagedAccountPlaceFavorite(ISession session, AccountPlaceFavorite value)
            : base(session)
        {
            mAccountPlaceFavorite = value;
        }

        public ManagedAccountPlaceFavorite(ISession session, TransitAccountPlaceFavorite value)
            : base(session)
        {
            mAccountPlaceFavorite = value.GetAccountPlaceFavorite(session);
        }

        public int Id
        {
            get
            {
                return mAccountPlaceFavorite.Id;
            }
        }

        public TransitAccountPlaceFavorite TransitAccountPlaceFavorite
        {
            get
            {
                return new TransitAccountPlaceFavorite(mAccountPlaceFavorite);
            }
        }

        public Account Account
        {
            get
            {
                return mAccountPlaceFavorite.Account;
            }
        }

        public void Delete()
        {
            mAccountPlaceFavorite.Account.AccountPlaceFavorites.Remove(mAccountPlaceFavorite);
            mAccountPlaceFavorite.Place.AccountPlaceFavorites.Remove(mAccountPlaceFavorite);
            Session.Delete(mAccountPlaceFavorite);
        }

        public Place Place
        {
            get
            {
                return mAccountPlaceFavorite.Place;
            }
        }
    }
}
