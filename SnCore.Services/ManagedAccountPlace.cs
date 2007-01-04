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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountPlace : TransitService<AccountPlace>
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

        public TransitAccountPlace(AccountPlace instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountPlace instance)
        {
            Type = instance.Type.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountId = instance.Account.Id;
            PlaceId = instance.Place.Id;
            AccountName = instance.Account.Name;
            PlaceName = instance.Place.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account);
            PlacePictureId = ManagedService<PlacePicture, TransitPlacePicture>.GetRandomElementId(instance.Place.PlacePictures);
            CanWrite = instance.Type.CanWrite;
            base.SetInstance(instance);
        }

        public override AccountPlace GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPlace instance = base.GetInstance(session, sec);
            instance.Description = this.Description;
            instance.Type = ManagedAccountPlaceType.Find(session, this.Type);

            if (Id == 0)
            {
                // the account and the place cannot be switched after the relationship is created
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Place = (Place)session.Load(typeof(Place), PlaceId);
            }

            return instance;
        }
    }

    public class ManagedAccountPlace : ManagedService<AccountPlace, TransitAccountPlace>
    {
        public ManagedAccountPlace()
        {

        }

        public ManagedAccountPlace(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlace(ISession session, int id)
            : base(session, id)
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
            Collection<AccountPlace>.GetSafeCollection(mInstance.Account.AccountPlaces).Remove(mInstance);
            Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces).Remove(mInstance);
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
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}
