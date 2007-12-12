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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;
using System.Collections.Generic;

public partial class DiscussionView : Page
{
    public DiscussionView()
    {
        mIsMobileEnabled = true;
    }

    public bool ParentRedirect
    {
        get
        {
            bool result = true;
            bool.TryParse(Request["ParentRedirect"], out result);
            return result;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            discussionMain.DiscussionId = RequestId;
            discussionMain.DataBind();

            TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, RequestId);

            if (td == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            if (td.Personal && ParentRedirect)
            {
                string uri = SessionManager.DiscussionService.GetDiscussionRedirectUri(
                    SessionManager.Ticket, td.Id);

                if (!string.IsNullOrEmpty(uri))
                {
                    Redirect(uri);
                    return;
                }
            }

            this.Title = Renderer.Render(td.Name);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            if (td.Personal)
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, 
                    string.Format("{0}&ReturnUrl={1}", td.ParentObjectUri, Renderer.UrlEncode(Request.Url.PathAndQuery))));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            }
            sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }
}
