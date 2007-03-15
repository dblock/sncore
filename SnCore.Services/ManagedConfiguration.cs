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
        public class InvalidConfigurationException : SoapException
        {
            public InvalidConfigurationException()
                : base("Invalid configuration setting", SoapException.ClientFaultCode)
            {

            }
        }

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

        public static ManagedConfiguration GetConfigurationByName(ISession session, string name)
        {
            return new ManagedConfiguration(session, GetConfiguration(session, name));
        }

        public static Configuration GetConfiguration(ISession session, string name)
        {
            return GetConfiguration(session, name, true);
        }

        public static Configuration GetConfiguration(ISession session, string name, bool throwonerror)
        {
            Configuration c = (Configuration)session.CreateCriteria(typeof(Configuration))
                .Add(Expression.Eq("OptionName", name))
                .UniqueResult();

            if (c == null && throwonerror)
            {
                throw new ManagedConfiguration.InvalidConfigurationException();
            }

            return c;
        }

        public static ManagedConfiguration SetValue(ISession session, string name, string value)
        {
            Configuration c = null;
            try
            {
                c = GetConfiguration(session, name);
            }
            catch (InvalidConfigurationException)
            {
                c = new Configuration();
                c.OptionName = name;
            }

            c.OptionValue = value;
            session.Save(c);
            session.Flush();
            return new ManagedConfiguration(session, c);
        }

        public static string GetValue(ISession session, string name)
        {
            return GetConfiguration(session, name).OptionValue;
        }

        public static string GetValue(ISession session, string name, string defaultvalue)
        {
            try
            {
                return GetConfiguration(session, name).OptionValue;
            }
            catch (InvalidConfigurationException)
            {
                return defaultvalue;
            }
        }

        public static int GetValue(ISession session, string name, int defaultvalue)
        {
            try
            {
                int result = 0;
                if (int.TryParse(GetConfiguration(session, name).OptionValue, out result))
                    return result;
                return defaultvalue;
            }
            catch (InvalidConfigurationException)
            {
                return defaultvalue;
            }
        }

        public static int GetConfigurationId(ISession session, string name)
        {
            return GetConfiguration(session, name).Id;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
