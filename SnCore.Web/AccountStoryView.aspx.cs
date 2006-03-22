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

public partial class AccountStoryView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                TransitAccountStory ts = StoryService.GetAccountStoryById(
                    SessionManager.Ticket, RequestId);

                TransitAccount acct = AccountService.GetAccountById(
                    ts.AccountId);

                this.Title = string.Format("{0}'s {1}", Renderer.Render(acct.Name), Renderer.Render(ts.Name));

                linkAccountStory.Text = Renderer.Render(ts.Name);
                linkAccount.Text = Renderer.Render(acct.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", acct.Id);

                storyName.Text = Renderer.Render(ts.Name);
                storySummary.Text = RenderEx(ts.Summary);
                listPictures.DataSource = StoryService.GetAccountStoryPicturesById(RequestId);
                listPictures.DataBind();

                if (listPictures.Items.Count == 0) storyNoPicture.Visible = true;

                storyComments.DiscussionId = DiscussionService.GetAccountStoryDiscussionId(RequestId);
                storyComments.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
