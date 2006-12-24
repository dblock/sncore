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
    public class TransitCountry : TransitService
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

        public TransitCountry()
        {

        }

        public TransitCountry(Country c)
            : base(c.Id)
        {
            Name = c.Name;
        }

        public Country GetCountry(ISession session)
        {
            Country p = (Id != 0) ? (Country)session.Load(typeof(Country), Id) : new Country();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed country.
    /// </summary>
    public class ManagedCountry : ManagedService<Country>
    {
        public class InvalidCountryException : SoapException
        {
            public InvalidCountryException()
                : base("Invalid country", SoapException.ClientFaultCode)
            {

            }
        }

        private Country mCountry = null;

        public ManagedCountry(ISession session)
            : base(session)
        {

        }

        public ManagedCountry(ISession session, int id)
            : base(session)
        {
            mCountry = (Country)session.Load(typeof(Country), id);
        }

        public ManagedCountry(ISession session, Country value)
            : base(session)
        {
            mCountry = value;
        }

        public ManagedCountry(ISession session, TransitCountry value)
            : base(session)
        {
            mCountry.Name = value.Name;
        }

        public int Id
        {
            get
            {
                return mCountry.Id;
            }
        }

        public string Name
        {
            get
            {
                return mCountry.Name;
            }
        }

        public TransitCountry TransitCountry
        {
            get
            {
                return new TransitCountry(mCountry);
            }
        }

        public void Create(TransitCountry c)
        {
            mCountry = c.GetCountry(Session);
            Session.Save(mCountry);
        }

        public void Delete()
        {
            Session.Delete(mCountry);
        }

        public static Country Find(ISession session, string name)
        {
            Country c = (Country)session.CreateCriteria(typeof(Country))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();

            if (c == null)
            {
                throw new ManagedCountry.InvalidCountryException();
            }

            return c;
        }

        public static int GetCountryId(ISession session, string name)
        {
            return Find(session, name).Id;
        }
    }
}
