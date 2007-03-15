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
    public class TransitPlacePropertyGroup : TransitService<PlacePropertyGroup>
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

        public TransitPlacePropertyGroup(PlacePropertyGroup instance)
            : base(instance)
        {

        }

        public override void SetInstance(PlacePropertyGroup instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            base.SetInstance(instance);
        }

        public override PlacePropertyGroup GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePropertyGroup instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedPlacePropertyGroup : ManagedService<PlacePropertyGroup, TransitPlacePropertyGroup>
    {
        public ManagedPlacePropertyGroup()
        {

        }

        public ManagedPlacePropertyGroup(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePropertyGroup(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlacePropertyGroup(ISession session, PlacePropertyGroup value)
            : base(session, value)
        {

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

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
