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
    public class TransitConfiguration : TransitService
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

        public TransitConfiguration(Configuration e)
            : base(e.Id)
        {
            Name = e.OptionName;
            Value = e.OptionValue;
            Password = e.Password;
        }

        public Configuration GetConfiguration(ISession session)
        {
            Configuration c = (Id > 0) ? (Configuration)session.Load(typeof(Configuration), Id) : new Configuration();
            c.OptionName = this.Name;
            c.OptionValue = this.Value;
            c.Password = this.Password;
            return c;
        }
    }

    /// <summary>
    /// Managed configuration.
    /// </summary>
    public class ManagedConfiguration : ManagedService<Configuration>
    {
        private Configuration mConfiguration = null;

        public class InvalidConfigurationException : SoapException
        {
            public InvalidConfigurationException()
                : base("Invalid configuration setting", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedConfiguration(ISession session)
            : base(session)
        {

        }

        public ManagedConfiguration(ISession session, int id)
            : base(session)
        {
            mConfiguration = (Configuration)session.Load(typeof(Configuration), id);
        }

        public ManagedConfiguration(ISession session, Configuration value)
            : base(session)
        {
            mConfiguration = value;
        }

        public ManagedConfiguration(ISession session, TransitConfiguration value)
            : base(session)
        {
            mConfiguration.OptionName = value.Name;
            mConfiguration.OptionValue = value.Value;
        }

        public int Id
        {
            get
            {
                return mConfiguration.Id;
            }
        }

        public string Name
        {
            get
            {
                return mConfiguration.OptionName;
            }
        }

        public string Value
        {
            get
            {
                return mConfiguration.OptionValue;
            }
        }

        public TransitConfiguration TransitConfiguration
        {
            get
            {
                return new TransitConfiguration(mConfiguration);
            }
        }

        public void Create(TransitConfiguration c)
        {
            mConfiguration = new Configuration();
            mConfiguration.OptionName = c.Name;
            mConfiguration.OptionValue = c.Value;
            Session.Save(mConfiguration);
        }

        public void Delete()
        {
            Session.Delete(mConfiguration);
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
    }
}
