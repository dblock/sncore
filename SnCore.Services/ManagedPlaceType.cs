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
    public class TransitPlaceType : TransitService<PlaceType>
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

        public TransitPlaceType(PlaceType value)
            : base(value)
        {

        }

        public override void SetInstance(PlaceType instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override PlaceType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedPlaceType : ManagedService<PlaceType, TransitPlaceType>
    {
        public ManagedPlaceType()
        {

        }

        public ManagedPlaceType(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceType(ISession session, PlaceType value)
            : base(session, value)
        {

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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
