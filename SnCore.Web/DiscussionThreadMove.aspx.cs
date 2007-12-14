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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.WebControls;

public partial class DiscussionThreadMove : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitDiscussionThread tt = SessionManager.DiscussionService.GetDiscussionThreadById(
                SessionManager.Ticket, RequestId);

            TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, tt.DiscussionId);

            TransitDiscussionPost tp = SessionManager.DiscussionService.GetDiscussionThreadPost(
                SessionManager.Ticket, tt.Id);

            this.Title = Renderer.Render(td.Name);

            if (td.ObjectId != 0 && ! string.IsNullOrEmpty(td.ParentObjectType))
            {
                listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussionsByObjectId(
                    SessionManager.Ticket, td.ParentObjectType, td.ObjectId, null);
            }
            else
            {
                listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussions(
                    SessionManager.Ticket, null);
            }

            listDiscussions.DataBind();

            ListItemManager.TrySelect(listDiscussions, tt.DiscussionId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(tp.Subject, Request, string.Format("DiscussionThreadView.aspx?id={0}&did={1}", tp.DiscussionThreadId, td.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Move", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void move_Click(object sender, EventArgs args)
    {
        SessionManager.DiscussionService.MoveDiscussionThread(
            SessionManager.Ticket,
            RequestId,
            int.Parse(listDiscussions.SelectedValue));

        ReportInfo("Discussion thread moved.");
    }
}
