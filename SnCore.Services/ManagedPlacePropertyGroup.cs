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

namespace SnCore.Services
{
    public class TransitPlacePropertyGroup : TransitService
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

        public TransitPlacePropertyGroup()
        {

        }

        public TransitPlacePropertyGroup(PlacePropertyGroup o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
        }

        public PlacePropertyGroup GetPlacePropertyGroup(ISession session)
        {
            PlacePropertyGroup p = (Id != 0) ? (PlacePropertyGroup)session.Load(typeof(PlacePropertyGroup), Id) : new PlacePropertyGroup();
            p.Name = this.Name;
            p.Description = this.Description;
            return p;
        }
    }

    public class ManagedPlacePropertyGroup : ManagedService
    {
        private PlacePropertyGroup mPlacePropertyGroup = null;

        public ManagedPlacePropertyGroup(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePropertyGroup(ISession session, int id)
            : base(session)
        {
            mPlacePropertyGroup = (PlacePropertyGroup)session.Load(typeof(PlacePropertyGroup), id);
        }

        public ManagedPlacePropertyGroup(ISession session, PlacePropertyGroup value)
            : base(session)
        {
            mPlacePropertyGroup = value;
        }

        public ManagedPlacePropertyGroup(ISession session, TransitPlacePropertyGroup value)
            : base(session)
        {
            mPlacePropertyGroup = value.GetPlacePropertyGroup(session);
        }

        public int Id
        {
            get
            {
                return mPlacePropertyGroup.Id;
            }
        }

        public TransitPlacePropertyGroup TransitPlacePropertyGroup
        {
            get
            {
                return new TransitPlacePropertyGroup(mPlacePropertyGroup);
            }
        }

        public void CreateOrUpdate(TransitPlacePropertyGroup o)
        {
            mPlacePropertyGroup = o.GetPlacePropertyGroup(Session);
            Session.Save(mPlacePropertyGroup);
        }

        public void Delete()
        {
            Session.Delete(mPlacePropertyGroup);
        }

        public static PlacePropertyGroup Find(ISession session, string name)
        {
            return (PlacePropertyGroup)session.CreateCriteria(typeof(PlacePropertyGroup))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
