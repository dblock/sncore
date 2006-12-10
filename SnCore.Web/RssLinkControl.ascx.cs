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

public partial class RssLinkControl : System.Web.UI.UserControl
{
    private HtmlLink mLink = new HtmlLink();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            mLink.Attributes.Add("rel", "alternate");
            mLink.Attributes.Add("type", "application/rss+xml");
            Page.Header.Controls.Add(mLink);
        }
    }

    public string Title
    {
        get
        {
            return mLink.Attributes["title"];
        }
        set
        {
            linkRss.ToolTip = mLink.Attributes["title"] = value;
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
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (string.IsNullOrEmpty(mLink.Attributes["title"]))
        {
            linkRss.ToolTip = mLink.Attributes["title"] = Page.Title;
        }
        
        base.OnPreRender(e);
    }
}
