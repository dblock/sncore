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

public partial class AccountStoryPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                inputPicture.Src = "AccountStoryPicture.aspx?id=" + RequestId;

                TransitAccountStoryPicture p = StoryService.GetAccountStoryPictureById(SessionManager.Ticket, RequestId);
                TransitAccountStory s = StoryService.GetAccountStoryById(SessionManager.Ticket, p.AccountStoryId);
                TransitAccount a = AccountService.GetAccountById(s.AccountId);

                inputPicture.Src = "AccountStoryPicture.aspx?id=" + RequestId;
                linkAccountStory.Text = Renderer.Render(s.Name);
                linkAccountStory.NavigateUrl = "AccountStoryView.aspx?id=" + s.Id.ToString();
                linkAccount.Text = Renderer.Render(a.Name);
                linkAccount.NavigateUrl = "AccountView.aspx?id=" + a.Id.ToString();
                labelPicture.Text = Renderer.Render(p.Name);

                this.Title = string.Format("{0}'s {1} {2}",
                    Renderer.Render(a.Name), Renderer.Render(s.Name), Renderer.Render(p.Name));

                linkComments.Visible = p.CommentCount > 0;
                linkComments.Text = string.Format("{0} comment{1}",
                    (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
                    (p.CommentCount == 1) ? "" : "s");

                discussionComments.DiscussionId = DiscussionService.GetAccountStoryPictureDiscussionId(RequestId);
                discussionComments.DataBind();

            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
