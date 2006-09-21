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
            listPictures.OnGetDataSource += new EventHandler(listPictures_OnGetDataSource);
            if (!IsPostBack)
            {
                object[] s_args = { SessionManager.Ticket, RequestId };
                TransitAccountStory ts = SessionManager.GetCachedItem<TransitAccountStory>(
                    StoryService, "GetAccountStoryById", s_args);

                object[] a_args = { ts.AccountId };
                TransitAccount acct = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", a_args);

                licenseView.AccountId = acct.Id;

                this.Title = string.Format("{0}'s {1}", Renderer.Render(acct.Name), Renderer.Render(ts.Name));

                linkAccountStory.Text = Renderer.Render(ts.Name);
                linkAccount.Text = Renderer.Render(acct.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", acct.Id);

                storyName.Text = Renderer.Render(ts.Name);
                storySummary.Text = RenderEx(ts.Summary);

                object[] p_args = { RequestId };
                listPictures.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                    StoryService, "GetAccountStoryPicturesCountById", p_args);
                listPictures_OnGetDataSource(sender, e);

                object[] d_args = { RequestId };
                storyComments.DiscussionId = SessionManager.GetCachedCollectionCount(
                    DiscussionService, "GetAccountStoryDiscussionId", d_args);

                storyComments.DataBind();

                linkEdit.NavigateUrl = string.Format("AccountStoryEdit.aspx?id={0}", ts.Id);
                linkAddPhotos.NavigateUrl = string.Format("AccountStoryPicturesManage.aspx?id={0}", ts.Id);

                panelOwner.Visible = SessionManager.IsLoggedIn &&
                    (SessionManager.IsAdministrator || ts.AccountId == SessionManager.Account.Id);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void listPictures_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = listPictures.CurrentPageIndex;
            options.PageSize = listPictures.PageSize;
            object[] p_args = { RequestId, options };
            listPictures.DataSource = SessionManager.GetCachedCollection<TransitAccountStoryPicture>(
                StoryService, "GetAccountStoryPicturesById", p_args);
            listPictures.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
