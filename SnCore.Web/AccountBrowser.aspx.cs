using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using SnCore.SiteMap;
using System.Collections.Generic;
using System.Text;

public partial class AccountBrowser : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Help", Request, "Help.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Browser Information", Request.Url));
            StackSiteMap(sitemapdata);

            GetCookies();
            GetHeaders();
        }
    }

    private void GetCookies()
    {
        IList<HttpCookie> cookies = new List<HttpCookie>(Request.Cookies.Count);
        for (int i = 0; i < Request.Cookies.AllKeys.Length; i++)
        {
            cookies.Add(Request.Cookies[i]);
        }

        gridCookies.DataSource = cookies;
        gridCookies.DataBind();
    }

    private void GetHeaders()
    {
        gridHeaders.DataSource = Request.Headers;
        gridHeaders.DataBind();
    }

    public string GetSplitValue(string value, int size)
    {
        StringBuilder result = new StringBuilder();
        while (value.Length > size)
        {
            result.AppendLine(value.Substring(0, size));
            value = value.Substring(size, value.Length - size);
        }
        result.Append(value);
        return result.ToString();
    }
}
