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
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            discussionMain.DiscussionId = RequestId;
            discussionMain.DataBind();

            TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, RequestId);
            this.Title = Renderer.Render(td.Name);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", RequestId)));
            StackSiteMap(sitemapdata);
        }
    }
}
