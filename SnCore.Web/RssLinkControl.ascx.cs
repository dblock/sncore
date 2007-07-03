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
using SnCore.Tools.Web;

public partial class RssLinkControl : System.Web.UI.UserControl
{
    private HtmlLink mLink = new HtmlLink();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(mLink.Href))
            {
                mLink.Attributes.Add("rel", "alternate");
                mLink.Attributes.Add("type", "application/rss+xml");
                Page.Header.Controls.Add(mLink);
            }
        }
    }

    public string Title
    {
        get
        {
            string result = mLink.Attributes["title"];
            if (string.IsNullOrEmpty(result)) result = Page.Title;
            return result;
        }
        set
        {
            linkRss.ToolTip = mLink.Attributes["title"] = value;
            linkEmail.ToolTip = string.Format("Subscribe to {0}", value);
            linkEmail.NavigateUrl = string.Format("AccountRssWatchEdit.aspx?url={0}&name={1}&ReturnUrl={2}",
                Renderer.UrlEncode(NavigateUrl), Renderer.UrlEncode(value), Renderer.UrlEncode(Request.Url.PathAndQuery));
        }
    }

    public string NavigateUrl
    {
        get
        {
            return mLink.Href;
        }
        set
        {
            linkRss.NavigateUrl = mLink.Href = value;
            linkEmail.NavigateUrl = string.Format("AccountRssWatchEdit.aspx?url={0}&name={1}&ReturnUrl={2}", 
                Renderer.UrlEncode(value), Renderer.UrlEncode(Title), Renderer.UrlEncode(Request.Url.PathAndQuery));
        }
    }

    public bool ButtonVisible
    {
        get
        {
            return linkRss.Visible;
        }
        set
        {
            linkRss.Visible = value;
            linkEmail.Visible = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (string.IsNullOrEmpty(mLink.Attributes["title"]))
        {
            linkRss.ToolTip = mLink.Attributes["title"] = Page.Title;
            linkEmail.ToolTip = string.Format("Subscribe to {0}", Page.Title);
        }
        
        base.OnPreRender(e);
    }
}
