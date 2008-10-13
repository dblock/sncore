using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SnCore.Data.Mapping
{
    public class RemoveMappingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("class", IsRequired = true)]
        public String Class
        {
            get
            {
                return (String)this["class"];
            }
            set
            {
                this["class"] = value;
            }
        }

        [ConfigurationProperty("bag", IsRequired = true)]
        public String Bag
        {
            get
            {
                return (String)this["bag"];
            }
            set
            {
                this["bag"] = value;
            }
        }
    }

    public class RemoveMappingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RemoveMappingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return string.Format("{0}:{1}",
                ((RemoveMappingConfigurationElement)element).Class,
                ((RemoveMappingConfigurationElement)element).Bag);
        }
    }

    public class RemoveMappingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public RemoveMappingCollection RemoveMappings
        {
            get
            {
                return (RemoveMappingCollection)this[""];
            }
        }
    }
}
