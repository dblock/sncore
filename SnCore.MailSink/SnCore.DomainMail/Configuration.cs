using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;

namespace SnCore.DomainMail
{
    public class Configuration
    {
        private IDictionary mAppSettings = null;
        private string mFilename = string.Empty;
        private XmlDocument mXmlConfiguration = null;

        public Configuration(string FilenameValue)
        {
            Filename = FilenameValue;
        }

        public XmlDocument XmlConfiguration
        {
            get
            {
                if (mXmlConfiguration == null)
                {
                    mXmlConfiguration = new XmlDocument();

                    XmlTextReader ConfigurationReader = null;

                    try
                    {
                        ConfigurationReader = new XmlTextReader(Filename);
                        XmlConfiguration.Load(ConfigurationReader);
                    }
                    finally
                    {
                        if (null != ConfigurationReader)
                            ConfigurationReader.Close();
                    }
                }
                return mXmlConfiguration;
            }
            set
            {
                mXmlConfiguration = value;
                AppSettings = null;
            }
        }

        public string Filename
        {
            get
            {
                return mFilename;
            }
            set
            {
                mFilename = value;
                XmlConfiguration = null;
            }
        }

        public string this[string key]
        {
            get
            {
                string Result = (string)AppSettings[key];
                return Result == null ? string.Empty : Result;
            }
        }

        public IDictionary AppSettings
        {
            get
            {
                if (mAppSettings == null)
                {
                    mAppSettings = GetConfig("appSettings");

                    if (mAppSettings == null)
                    {
                        mAppSettings = (IDictionary)new NameValueCollection();
                    }
                }
                return mAppSettings;
            }
            set
            {
                mAppSettings = value;
            }
        }

        public IDictionary GetConfig(string NameValue)
        {
            XmlNodeList Section = XmlConfiguration.GetElementsByTagName(NameValue);
            foreach (XmlNode Node in Section)
            {
                if (Node.LocalName == NameValue)
                {
                    DictionarySectionHandler NodeHandler = new DictionarySectionHandler();
                    return (IDictionary)NodeHandler.Create(null, null, Node);
                }
            }

            return null;
        }
    }
}