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
    public class TransitAccountGroupPlace : TransitService<AccountGroupPlace>
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

        private int mAccountGroupId = 0;

        public int AccountGroupId
        {
            get
            {

                return mAccountGroupId;
            }
            set
            {
                mAccountGroupId = value;
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

        private string mAccountGroupName;

        public string AccountGroupName
        {
            get
            {

                return mAccountGroupName;
            }
            set
            {
                mAccountGroupName = value;
            }
        }

        private int mAccountGroupPictureId;

        public int AccountGroupPictureId
        {
            get
            {

                return mAccountGroupPictureId;
            }
            set
            {
                mAccountGroupPictureId = value;
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

        public TransitAccountGroupPlace()
        {

        }

        public TransitAccountGroupPlace(AccountGroupPlace instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountGroupPlace instance)
        {
            Created = instance.Created;
            AccountGroupId = instance.AccountGroup.Id;
            PlaceId = instance.Place.Id;
            AccountGroupName = instance.AccountGroup.Name;
            PlaceName = instance.Place.Name;
            AccountGroupPictureId = ManagedAccountGroup.GetRandomAccountGroupPictureId(instance.AccountGroup);
            PlacePictureId = ManagedService<PlacePicture, TransitPlacePicture>.GetRandomElementId(instance.Place.PlacePictures);
            if (instance.Place.City != null) PlaceCity = instance.Place.City.Name;
            if (instance.Place.City != null && instance.Place.City.State != null) PlaceState = instance.Place.City.State.Name;
            if (instance.Place.City != null && instance.Place.City.Country != null) PlaceCountry = instance.Place.City.Country.Name;
            base.SetInstance(instance);
        }

        public override AccountGroupPlace GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroupPlace instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                // the AccountGroup and the place cannot be switched after the relationship is created
                instance.AccountGroup = session.Load<AccountGroup>(AccountGroupId);
                instance.Place = session.Load<Place>(PlaceId);
            }

            return instance;
        }
    }

    public class ManagedAccountGroupPlace : ManagedService<AccountGroupPlace, TransitAccountGroupPlace>
    {
        public ManagedAccountGroupPlace()
        {

        }

        public ManagedAccountGroupPlace(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroupPlace(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroupPlace(ISession session, AccountGroupPlace value)
            : base(session, value)
        {

        }

        public AccountGroup AccountGroup
        {
            get
            {
                return mInstance.AccountGroup;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountGroupPlace>.GetSafeCollection(mInstance.AccountGroup.AccountGroupPlaces).Remove(mInstance);
            Collection<AccountGroupPlace>.GetSafeCollection(mInstance.Place.AccountGroupPlaces).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow; 
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            // members can create or see the places depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Retreive | DataOperation.Create));
            }
            return acl;
        }

        public override int CreateOrUpdate(TransitAccountGroupPlace t_instance, ManagedSecurityContext sec)
        {
            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, t_instance.AccountGroupId);

            // check whether the user is already a member
            if (t_instance.Id == 0 && m_group.HasPlace(t_instance.PlaceId))
            {
                throw new Exception(string.Format(
                    "This place has already been added to \"{0}\".", m_group.Instance.Name));
            }

            return base.CreateOrUpdate(t_instance, sec);
        }

        public IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] has added [place:{1}] to [group:{2}]",
                        sec.Account.Id, mInstance.Place.Id, mInstance.AccountGroup.Id),
                        string.Format("AccountGroupView.aspx?id={0}", mInstance.AccountGroup.Id)));
                    break;
            }
            return result;
        }
    }
}
