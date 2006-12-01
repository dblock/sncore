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

                TransitDiscussionThread t = DiscussionService.GetDiscussionThreadById(
                    SessionManager.Ticket, RequestId);

                TransitDiscussion td = DiscussionService.GetDiscussionById(t.DiscussionId);

                if (td.Personal)
                {
                    if (td.Name == DiscussionService.GetAccountPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAccountStoryDiscussionName())
                    {
                        Redirect(string.Format("AccountStoryView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAccountTestimonialsDiscussionName())
                    {
                        Redirect(string.Format("AccountView.aspx?id={0}&#testimonials", td.AccountId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetPlaceDiscussionName())
                    {
                        Redirect(string.Format("PlaceView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetPlacePictureDiscussionName())
                    {
                        Redirect(string.Format("PlacePictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAcountFeedItemDiscussionName())
                    {
                        Redirect(string.Format("AccountFeedItemView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAcountBlogPostDiscussionName())
                    {
                        Redirect(string.Format("AccountBlogPostView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAccountStoryPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountStoryPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAccountEventDiscussionName())
                    {
                        Redirect(string.Format("AccountEventView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                    else if (td.Name == DiscussionService.GetAccountEventPictureDiscussionName())
                    {
                        Redirect(string.Format("AccountEventPictureView.aspx?id={0}&#comments", td.ObjectId));
                        return;
                    }
                }

                this.Title = Renderer.Render(td.Name);

                discussionMain.DiscussionThreadId = RequestId;
                discussionMain.DiscussionId = t.DiscussionId;
                discussionMain.DataBind();

                linkDiscussion.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", t.DiscussionId);
                linkDiscussion.Text = Renderer.Render(td.Name);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
