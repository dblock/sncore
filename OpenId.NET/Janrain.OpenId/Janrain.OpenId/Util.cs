using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Janrain.OpenId
{
    public class UriUtil
    {
        private UriUtil () 
	{ }

        public static Uri NormalizeUri(string uriString)
        {
            if ((!uriString.StartsWith("http")) && (uriString.IndexOf("://") == -1))
		uriString = "http://" + uriString;
            
            UriBuilder bldr = new UriBuilder(uriString);
            bldr.Host = bldr.Host.ToLower();
            return bldr.Uri;
        }
	
	public static string CreateQueryString(NameValueCollection args)
	{
	    string encKey, encVal;
	    string[] parts = new string[args.Count];
	    uint i = 0;
	    foreach (string key in args)
	    {
		encKey = HttpUtility.UrlEncode(key);
		encVal = HttpUtility.UrlEncode(args[key]);
		parts[i] = String.Format("{0}={1}", encKey, encVal);
		i++;
	    }
	    return String.Join("&", parts);
	}

	public static void AppendQueryArgument(UriBuilder builder, string key, string value)
	{
	    NameValueCollection c = new NameValueCollection();
	    c.Add(key, value);
	    string newqs = CreateQueryString(c);
	    string qs = builder.Query;
	    if (builder.Query != null && qs != String.Empty)
		qs = String.Format("{0}&{1}", qs.Substring(1), newqs);
	    else
		qs = newqs;
	    builder.Query = qs;
	}
    }
}
