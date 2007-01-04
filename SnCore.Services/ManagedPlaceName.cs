using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitPlaceName : TransitService<PlaceName>
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

        public TransitPlaceName()
        {

        }

        public TransitPlaceName(PlaceName instance)
            : base(instance)
        {

        }

        public override void SetInstance(PlaceName instance)
        {
            Name = instance.Name;
            Created = instance.Created;
            Modified = instance.Modified;
            PlaceId = instance.Place.Id;
            base.SetInstance(instance);
        }

        public override PlaceName GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceName instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Place = (Place)session.Load(typeof(Place), PlaceId);
            return instance;
        }
    }

    public class ManagedPlaceName : ManagedService<PlaceName, TransitPlaceName>
    {
        public ManagedPlaceName()
        {

        }

        public ManagedPlaceName(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceName(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceName(ISession session, PlaceName value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
            }
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
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.Place.Account, DataOperation.All));
            foreach (AccountPlace relationship in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
            {
                acl.Add(new ACLAccount(relationship.Account,
                    relationship.Type.CanWrite ? DataOperation.Update : DataOperation.Retreive));
            }
            return acl;
        }
    }
}
