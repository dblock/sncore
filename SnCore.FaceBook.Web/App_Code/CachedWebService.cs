using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Caching;

public class CachedWebService<WebServiceType>
    where WebServiceType : new()
{
    public static WebServiceType GetEndPoint(Cache cache)
    {
        string key = string.Format("_{0}", typeof(WebServiceType).Name);
        WebServiceType endpoint = (WebServiceType) cache[key];
        if (endpoint == null)
        {
            endpoint = new WebServiceType();
            cache[key] = endpoint;
        }
        return endpoint;
    }
}
