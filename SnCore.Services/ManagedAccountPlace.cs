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
    public class TransitAccountPlace : TransitService
    {
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

        private bool mCanWrite;

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

        public TransitAccountPlace()
        {

        }

        public TransitAccountPlace(AccountPlace o)
            : base(o.Id)
        {
            Type = o.Type.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            AccountId = o.Account.Id;
            PlaceId = o.Place.Id;
            AccountName = o.Account.Name;
            PlaceName = o.Place.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.Account);
            PlacePictureId = ManagedService<PlacePicture>.GetRandomElementId(o.Place.PlacePictures);
            CanWrite = o.Type.CanWrite;
        }

        public AccountPlace GetAccountPlace(ISession session)
        {
            AccountPlace p = (Id != 0) ? (AccountPlace)session.Load(typeof(AccountPlace), Id) : new AccountPlace();
            p.Description = this.Description;
            p.Type = ManagedAccountPlaceType.Find(session, this.Type);

            if (Id == 0)
            {
                // the account and the place cannot be switched after the relationship is created
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
                if (PlaceId > 0) p.Place = (Place)session.Load(typeof(Place), PlaceId);
            }

            return p;
        }
    }

    public class ManagedAccountPlace : ManagedService<AccountPlace>
    {
        private AccountPlace mAccountPlace = null;

        public ManagedAccountPlace(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlace(ISession session, int id)
            : base(session)
        {
            mAccountPlace = (AccountPlace)session.Load(typeof(AccountPlace), id);
        }

        public ManagedAccountPlace(ISession session, AccountPlace value)
            : base(session)
        {
            mAccountPlace = value;
        }

        public ManagedAccountPlace(ISession session, TransitAccountPlace value)
            : base(session)
        {
            mAccountPlace = value.GetAccountPlace(session);
        }

        public int Id
        {
            get
            {
                return mAccountPlace.Id;
            }
        }

        public TransitAccountPlace TransitAccountPlace
        {
            get
            {
                return new TransitAccountPlace(mAccountPlace);
            }
        }

        public Account Account
        {
            get
            {
                return mAccountPlace.Account;
            }
        }

        public void Delete()
        {
            mAccountPlace.Account.AccountPlaces.Remove(mAccountPlace);
            mAccountPlace.Place.AccountPlaces.Remove(mAccountPlace);
            Session.Delete(mAccountPlace);
        }

        public Place Place
        {
            get
            {
                return mAccountPlace.Place;
            }
        }
    }
}
