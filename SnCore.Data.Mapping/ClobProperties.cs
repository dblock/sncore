using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SnCore.Data.Mapping
{
    public class ClobPropertyConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("property", IsRequired = true)]
        public String Property
        {
            get
            {
                return (String)this["property"];
            }
            set
            {
                this["property"] = value;
            }
        }
    }

    public class ClobPropertyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClobPropertyConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClobPropertyConfigurationElement)element).Property;
        }
    }

    public class ClobPropertiesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public ClobPropertyCollection ClobProperties
        {
            get
            {
                return (ClobPropertyCollection)this[""];
            }
        }
    }
}
