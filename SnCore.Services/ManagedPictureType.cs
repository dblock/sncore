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
    public class TransitPictureType : TransitService<PictureType>
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

        public TransitPictureType(PictureType instance)
            : base(instance)
        {

        }

        public override void SetInstance(PictureType instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override PictureType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PictureType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedPictureType : ManagedService<PictureType, TransitPictureType>
    {
        public ManagedPictureType()
        {

        }

        public ManagedPictureType(ISession session)
            : base(session)
        {

        }

        public ManagedPictureType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPictureType(ISession session, PictureType value)
            : base(session, value)
        {

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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
