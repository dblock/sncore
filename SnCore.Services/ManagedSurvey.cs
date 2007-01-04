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
    public class TransitSurvey : TransitService<Survey>
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

        public TransitSurvey(Survey value)
            : base(value)
        {

        }

        public override void SetInstance(Survey value)
        {
            Name = value.Name;
            base.SetInstance(value);
        }

        public override Survey GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Survey instance = base.GetInstance(session, sec);
            instance.Name = Name;
            return instance;
        }
    }

    public class ManagedSurvey : ManagedService<Survey, TransitSurvey>
    {
        public class InvalidSurveyException : SoapException
        {
            public InvalidSurveyException()
                : base("Invalid survey", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedSurvey()
        {

        }

        public ManagedSurvey(ISession session)
            : base(session)
        {

        }

        public ManagedSurvey(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedSurvey(ISession session, Survey value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
