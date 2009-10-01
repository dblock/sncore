using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Globalization;
using System.IO;

namespace SnCore.Services
{
    public class TransitCultureInfo
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

        private int mLCID;

        public int LCID
        {
            get
            {
                return mLCID;
            }
            set
            {
                mLCID = value;
            }
        }

        private string mNativeName;

        public string NativeName
        {
            get
            {
                return mNativeName;
            }
            set
            {
                mNativeName = value;
            }
        }

        public TransitCultureInfo(string name, int lcid)
        {
            mLCID = lcid;
            mName = name;
            mNativeName = name;
        }

        public TransitCultureInfo(CultureInfo ci)
        {
            mLCID = ci.LCID;
            mName = ci.Name;
            mNativeName = ci.NativeName;
        }

        public bool Equals(TransitCultureInfo value)
        {
            return mLCID == value.LCID;
        }

        public override bool Equals(object obj)
        {
            if (obj is TransitCultureInfo)
            {
                return Equals(obj as TransitCultureInfo);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public TransitCultureInfo()
            : this(CultureInfo.CurrentCulture)
        {

        }
    }
}
