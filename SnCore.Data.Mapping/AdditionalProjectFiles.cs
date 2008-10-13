using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SnCore.Data.Mapping
{
    public class AdditionalProjectFileConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("filename", IsRequired = true)]
        public String Filename
        {
            get
            {
                return (String)this["filename"];
            }
            set
            {
                this["filename"] = value;
            }
        }
    }

    public class AdditionalProjectFileCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AdditionalProjectFileConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AdditionalProjectFileConfigurationElement) element).Filename;
        }
    }

    public class AdditionalProjectFilesConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public AdditionalProjectFileCollection AdditionalProjectFiles
        {
            get
            {
                return (AdditionalProjectFileCollection) this[""];
            }
        }
    }
}
