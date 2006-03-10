using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace SnCore.Tools.Web
{
    public class UriBuilder : System.UriBuilder
    {
        #region QueryItemCollection

        private class QueryItemCollection : NameValueCollection
        {
            private bool _isDirty = false;

            internal bool IsDirty
            {
                get { return _isDirty; }
                set { _isDirty = value; }
            }

            public override void Add(string name, string value)
            {
                _isDirty = true;
                base.Add(name, value);
            }

            public override void Remove(string name)
            {
                _isDirty = true;
                base.Remove(name);
            }

            public override void Set(string name, string value)
            {
                _isDirty = true;
                base.Set(name, value);
            }
        }

        #endregion

        private QueryItemCollection queryItems;
        private bool _queryIsDirty = false;

        public UriBuilder()
        {
        }

        public UriBuilder(string uri)
            : base(uri)
        {
            _queryIsDirty = true;
        }

        public UriBuilder(Uri uri)
            : base(uri)
        {
            _queryIsDirty = true;
        }

        public UriBuilder(string schemeName, string hostName)
            : base(schemeName, hostName)
        {
        }

        /// <summary>
        /// The delegate identity URL must be canonical.
        /// It must be prefixed with the http protocal and 
        /// trailing slash if there's no path component.
        /// </summary>
        public static string NormalizeUrl(string url)
        {
            url = url.Trim();

            // Check if it begins with http
            if (!url.StartsWith("http://"))
                url = "http://" + url;

            Uri uri = new Uri(url);

            return uri.AbsoluteUri;
        }

        public UriBuilder(string scheme, string host, int portNumber)
            : base(scheme, host, portNumber)
        {
        }

        public UriBuilder(string scheme, string host, int port, string pathValue)
            : base(scheme, host, port, pathValue)
        {
        }

        public UriBuilder(string scheme, string host, int port, string path, string extraValue)
            : base(scheme, host, port, path, extraValue)
        {
        }

        public NameValueCollection QueryItems
        {
            get
            {
                if (queryItems == null)
                    queryItems = new QueryItemCollection();

                SyncQueryItems();

                return queryItems;
            }
        }

        public new string Query
        {
            get
            {
                SyncQuery();
                return base.Query;
            }
            set
            {
                if (value != null && value.Length > 0 && value[0] == '?')
                    value = value.Substring(1);

                base.Query = value;
                _queryIsDirty = true;
            }
        }

        public override string ToString()
        {
            SyncQuery();
            return base.ToString();
        }

        public new Uri Uri
        {
            get
            {
                SyncQuery();
                return base.Uri;
            }
        }

        private void SyncQueryItems()
        {
            if (_queryIsDirty)
            {
                //Console.WriteLine(">> Sync QueryItems");
                CreateItemsFromQuery();
                _queryIsDirty = false;
            }
        }

        private void CreateItemsFromQuery()
        {
            queryItems.Clear();

            if (base.Query.Length > 0)
            {
                string query = HttpUtility.UrlDecode(base.Query.Substring(1));

                string[] items = query.Split('&');
                foreach (string item in items)
                {
                    if (item.Length > 0)
                    {
                        string[] namevalue = item.Split('=');

                        if (namevalue.Length > 1)
                            queryItems.Add(namevalue[0], namevalue[1]);
                        else
                            queryItems.Add(namevalue[0], "");
                    }
                }
            }
        }

        private void SyncQuery()
        {
            if (queryItems != null)
            {
                // First check if queryItems has been cleared (using 
                // QueryItems.Clear()), because this doesn't 
                // update dirty flag!!!
                if (queryItems.Count == 0)
                {
                    base.Query = "";
                }
                else if (queryItems.IsDirty)
                {
                    //Console.WriteLine(">> Sync Query");
                    CreateQueryFromItems();
                }

                queryItems.IsDirty = false;
            }
        }

        private void CreateQueryFromItems()
        {
            string query = "";
            foreach (string key in queryItems.AllKeys)
            {
                string[] values = queryItems.GetValues(key);
                foreach (string value in values)
                {
                    query += (key + "=" + value + "&");
                }
            }

            if (query.Length > 0)
                query = query.Substring(0, query.Length - 1);

            base.Query = query;
        }
    }
}
