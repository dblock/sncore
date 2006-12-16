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

public partial class DiscussionThreadView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId == 0)
                {
                    Redirect("Default.aspx");
                    return;
                }

                TransitDiscussionThread t = SessionManager.DiscussionService.GetDiscussionThreadById(
                    SessionManager.Ticket, RequestId);

                TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(t.DiscussionId);

                if (td.Personal)
                {
                    if (td.Name == SessionManager.DiscussionService.GetAccountPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAccountStoryDiscussionName())
                    {
                        Redirect(string.Format("AccountStoryView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAccountTestimonialsDiscussionName())
                    {
                        Redirect(string.Format("AccountView.aspx?id={0}&#testimonials", td.AccountId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetPlaceDiscussionName())
                    {
                        Redirect(string.Format("PlaceView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetPlacePictureDiscussionName())
                    {
                        Redirect(string.Format("PlacePictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAcountFeedItemDiscussionName())
                    {
                        Redirect(string.Format("AccountFeedItemView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAcountBlogPostDiscussionName())
                    {
                        Redirect(string.Format("AccountBlogPostView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAccountStoryPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountStoryPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAccountEventDiscussionName())
                    {
                        Redirect(string.Format("AccountEventView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == SessionManager.DiscussionService.GetAccountEventPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountEventPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                }

                this.Title = Renderer.Render(td.Name);

                discussionMain.DiscussionThreadId = RequestId;
                discussionMain.DiscussionId = t.DiscussionId;
                discussionMain.DataBind();

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
                sitemapdata.Add(new SiteMapDataAttributeNode("Thread", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
