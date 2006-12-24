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
    public class TransitPictureType : TransitService
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

        public TransitPictureType()
        {

        }

        public TransitPictureType(PictureType o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public PictureType GetPictureType(ISession session)
        {
            PictureType p = (Id != 0) ? (PictureType)session.Load(typeof(PictureType), Id) : new PictureType();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed picture type.
    /// </summary>
    public class ManagedPictureType : ManagedService<PictureType>
    {
        private PictureType mPictureType = null;

        public ManagedPictureType(ISession session)
            : base(session)
        {

        }

        public ManagedPictureType(ISession session, int id)
            : base(session)
        {
            mPictureType = (PictureType)session.Load(typeof(PictureType), id);
        }

        public ManagedPictureType(ISession session, PictureType value)
            : base(session)
        {
            mPictureType = value;
        }

        public ManagedPictureType(ISession session, TransitPictureType value)
            : base(session)
        {
            mPictureType = value.GetPictureType(session);
        }

        public int Id
        {
            get
            {
                return mPictureType.Id;
            }
        }

        public TransitPictureType TransitPictureType
        {
            get
            {
                return new TransitPictureType(mPictureType);
            }
        }

        public void CreateOrUpdate(TransitPictureType o)
        {
            mPictureType = o.GetPictureType(Session);
            Session.Save(mPictureType);
        }

        public void Delete()
        {
            Session.Delete(mPictureType);
        }

        public static PictureType Find(ISession session, string name)
        {
            return (PictureType)session.CreateCriteria(typeof(PictureType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
