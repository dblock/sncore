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
    public class TransitPlaceType : TransitService
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

        public TransitPlaceType()
        {

        }

        public TransitPlaceType(PlaceType o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public PlaceType GetPlaceType(ISession session)
        {
            PlaceType p = (Id != 0) ? (PlaceType)session.Load(typeof(PlaceType), Id) : new PlaceType();
            p.Name = this.Name;
            return p;
        }
    }

    public class ManagedPlaceType : ManagedService
    {
        private PlaceType mPlaceType = null;

        public ManagedPlaceType(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceType(ISession session, int id)
            : base(session)
        {
            mPlaceType = (PlaceType)session.Load(typeof(PlaceType), id);
        }

        public ManagedPlaceType(ISession session, PlaceType value)
            : base(session)
        {
            mPlaceType = value;
        }

        public ManagedPlaceType(ISession session, TransitPlaceType value)
            : base(session)
        {
            mPlaceType = value.GetPlaceType(session);
        }

        public int Id
        {
            get
            {
                return mPlaceType.Id;
            }
        }

        public TransitPlaceType TransitPlaceType
        {
            get
            {
                return new TransitPlaceType(mPlaceType);
            }
        }

        public void CreateOrUpdate(TransitPlaceType o)
        {
            mPlaceType = o.GetPlaceType(Session);
            Session.Save(mPlaceType);
        }

        public void Delete()
        {
            Session.Delete(mPlaceType);
        }

        public static PlaceType Find(ISession session, string name)
        {
            return (PlaceType)session.CreateCriteria(typeof(PlaceType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
