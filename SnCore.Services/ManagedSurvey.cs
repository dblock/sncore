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
    public class TransitSurvey : TransitService
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

        public TransitSurvey()
        {

        }

        public TransitSurvey(Survey s)
            : base(s.Id)
        {
            Name = s.Name;
        }

        public Survey GetSurvey(ISession session)
        {
            Survey p = (Id != 0) ? (Survey)session.Load(typeof(Survey), Id) : new Survey();
            p.Name = this.Name;
            return p;
        }

    }

    public class ManagedSurvey : ManagedService<Survey>
    {
        public class InvalidSurveyException : SoapException
        {
            public InvalidSurveyException()
                : base("Invalid survey", SoapException.ClientFaultCode)
            {

            }
        }

        private Survey mSurvey = null;

        public ManagedSurvey(ISession session)
            : base(session)
        {

        }

        public ManagedSurvey(ISession session, int id)
            : base(session)
        {
            mSurvey = (Survey)session.Load(typeof(Survey), id);
        }

        public ManagedSurvey(ISession session, Survey value)
            : base(session)
        {
            mSurvey = value;
        }

        public int Id
        {
            get
            {
                return mSurvey.Id;
            }
        }

        public string Name
        {
            get
            {
                return mSurvey.Name;
            }
        }

        public TransitSurvey TransitSurvey
        {
            get
            {
                return new TransitSurvey(mSurvey);
            }
        }

        public void Create(TransitSurvey s)
        {
            mSurvey = s.GetSurvey(Session);
            Session.Save(mSurvey);
        }

        public void Delete()
        {
            Session.Delete(mSurvey);
        }

        public static int GetSurveyId(ISession session, string name)
        {
            Survey s = (Survey)session.CreateCriteria(typeof(Survey))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();

            if (s == null)
            {
                throw new InvalidSurveyException();
            }

            return s.Id;
        }
    }
}
