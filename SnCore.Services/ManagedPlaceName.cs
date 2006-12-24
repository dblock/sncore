using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;

namespace SnCore.Services
{
    public class TransitPlaceName : TransitService
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

        public TransitPlaceName(PlaceName p)
            : base(p.Id)
        {
            Name = p.Name;
            Created = p.Created;
            Modified = p.Modified;
            PlaceId = p.Place.Id;
        }

        public virtual PlaceName GetPlaceName(ISession session)
        {
            PlaceName p = (Id > 0) ? (PlaceName)session.Load(typeof(PlaceName), Id) : new PlaceName();
            p.Name = this.Name;
            p.Place = (Place)session.Load(typeof(Place), PlaceId);
            return p;
        }
    }

    /// <summary>
    /// Managed place alternative name.
    /// </summary>
    public class ManagedPlaceName : ManagedService<PlaceName>
    {
        private PlaceName mPlaceName = null;

        public ManagedPlaceName(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceName(ISession session, int id)
            : base(session)
        {
            mPlaceName = (PlaceName)session.Load(typeof(PlaceName), id);
        }

        public ManagedPlaceName(ISession session, PlaceName value)
            : base(session)
        {
            mPlaceName = value;
        }

        public int Id
        {
            get
            {
                return mPlaceName.Id;
            }
        }

        public string Name
        {
            get
            {
                return mPlaceName.Name;
            }
        }

        public DateTime Created
        {
            get
            {
                return mPlaceName.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mPlaceName.Modified;
            }
        }

        public TransitPlaceName TransitPlaceName
        {
            get
            {
                return new TransitPlaceName(mPlaceName);
            }
        }

        public void CreateOrUpdate(TransitPlaceName o)
        {
            mPlaceName = o.GetPlaceName(Session);
            mPlaceName.Modified = DateTime.UtcNow;
            if (Id == 0) mPlaceName.Created = mPlaceName.Modified;
            Session.Save(mPlaceName);
        }

        public void Delete()
        {
            Session.Delete(mPlaceName);
        }

        public Place Place
        {
            get
            {
                return mPlaceName.Place;
            }
        }
    }
}
