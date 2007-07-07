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
using SnCore.Data.Hibernate;
using System.IO;

namespace SnCore.Services
{
    public class TransitAccountCity : TransitService<AccountCity>
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

        private string mCountry;

        public string Country
        {
            get
            {
                return mCountry;
            }
            set
            {
                mCountry = value;
            }
        }

        private string mState;

        public string State
        {
            get
            {
                return mState;
            }
            set
            {
                mState = value;
            }
        }

        private int mTotal = 0;

        public int Total
        {
            get
            {
                return mTotal;
            }
            set
            {
                mTotal = value;
            }
        }

        public TransitAccountCity()
        {
        
        }

        public TransitAccountCity(AccountCity value)
            : base(value)
        {

        }

        public override void SetInstance(AccountCity value)
        {
            mName = value.City;
            mTotal = value.Total;
            base.SetInstance(value);
        }

        public override AccountCity GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountCity instance = base.GetInstance(session, sec);
            instance.City = this.Name;
            instance.Total = this.Total;
            return instance;
        }
    }

    public class ManagedAccountCity : ManagedService<AccountCity, TransitAccountCity>
    {
        public ManagedAccountCity()
        {

        }

        public ManagedAccountCity(ISession session)
            : base(session)
        {

        }

        public ManagedAccountCity(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountCity(ISession session, AccountCity value)
            : base(session, value)
        {

        }

        public override TransitAccountCity GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountCity t_instance = base.GetTransitInstance(sec);
            if (mInstance.Country_Id != 0) t_instance.Country = Session.Load<Country>(mInstance.Country_Id).Name;
            if (mInstance.State_Id != 0) t_instance.State = Session.Load<State>(mInstance.State_Id).Name;
            int city_id = 0;
            if (ManagedCity.TryGetCityId(Session, mInstance.City, t_instance.State, t_instance.Country, out city_id))
                t_instance.Id = city_id;
            return t_instance;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
