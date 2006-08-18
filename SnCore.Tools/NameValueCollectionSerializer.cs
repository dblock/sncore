using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace SnCore.Tools.Web
{
    public class NameValueCollectionSerializer
    {
        private List<string> mNames = null;
        private List<string> mValues = null;
        private NameValueCollection mCollection = null;

        public string[] Names
        {
            get
            {
                return mNames.ToArray();
            }
        }

        public string[] Values
        {
            get
            {
                return mValues.ToArray();
            }
        }

        public NameValueCollection Collection
        {
            get
            {
                return mCollection;
            }
        }

        public NameValueCollectionSerializer(string[] names, string[] values)
        {
            mNames = new List<string>(names);
            mValues = new List<string>(values);

            if (names.Length != values.Length)
            {
                throw new ArgumentException();
            }

            mCollection = new NameValueCollection();

            for (int i = 0; i < names.Length; i++)
                mCollection.Add(names[i], values[i]);
        }

        public NameValueCollectionSerializer(NameValueCollection nvc)
        {
            mCollection = nvc;
            mNames = new List<string>(nvc.Count);
            mValues = new List<string>(nvc.Count);

            for (int i = 0; i < nvc.Count; i++)
            {
                string key = nvc.GetKey(i);
                string[] vs = nvc.GetValues(i);
                foreach (string value in vs)
                {
                    mNames.Add(key);
                    mValues.Add(value);
                }
            }
        }
    }
}
