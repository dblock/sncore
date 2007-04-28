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
    public class TransitConfiguration : TransitService<Configuration>
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

        private string mValue;

        public string Value
        {
            get
            {

                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        private bool mPassword;

        public bool Password
        {
            get
            {

                return mPassword;
            }
            set
            {
                mPassword = value;
            }
        }

        public TransitConfiguration()
        {

        }

        public TransitConfiguration(Configuration instance)
            : base(instance)
        {

        }

        public override void SetInstance(Configuration instance)
        {
            Name = instance.OptionName;
            Password = instance.Password;
            Value = instance.OptionValue;
            if (Password) Value = string.Empty;
            base.SetInstance(instance);
        }

        public override Configuration GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Configuration instance = base.GetInstance(session, sec);
            instance.OptionName = this.Name;
            instance.OptionValue = this.Value;
            instance.Password = this.Password;
            return instance;
        }
    }

    public class ManagedConfiguration : ManagedService<Configuration, TransitConfiguration>
    {
        public ManagedConfiguration()
        {

        }


        public ManagedConfiguration(ISession session)
            : base(session)
        {

        }

        public ManagedConfiguration(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedConfiguration(ISession session, Configuration value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.OptionName;
            }
        }

        public string Value
        {
            get
            {
                return mInstance.OptionValue;
            }
        }

        public static bool TryGetConfiguration(ISession session, string name, out Configuration configuration)
        {
            configuration = session.CreateCriteria(typeof(Configuration))
                .Add(Expression.Eq("OptionName", name))
                .UniqueResult<Configuration>();

            return (configuration != null);
        }

        public static string GetValue(ISession session, string name, string defaultvalue)
        {
            Configuration result = null;

            if (!TryGetConfiguration(session, name, out result))
                return defaultvalue;
            
            return result.OptionValue;
        }

        public static int GetValue(ISession session, string name, int defaultvalue)
        {
            Configuration c = null;

            if (!TryGetConfiguration(session, name, out c))
                return defaultvalue;

            int result = 0;

            if (!int.TryParse(c.OptionValue, out result))
                return defaultvalue;

            return result;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
