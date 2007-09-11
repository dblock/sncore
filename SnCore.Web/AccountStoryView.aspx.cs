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

public partial class AccountStoryView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountStory ts = SessionManager.GetInstance<TransitAccountStory, int>(
                RequestId, SessionManager.StoryService.GetAccountStoryById);

            TransitAccount acct = SessionManager.GetInstance<TransitAccount, int>(
                ts.AccountId, SessionManager.AccountService.GetAccountById);

            licenseView.AccountId = acct.Id;
            picturesView.AccountStoryId = ts.Id;

            this.Title = string.Format("{0}'s {1}", Renderer.Render(acct.Name), Renderer.Render(ts.Name));

            storyName.Text = Renderer.Render(ts.Name);
            storySummary.Text = RenderEx(ts.Summary);

            storyComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
                typeof(AccountStory).Name, RequestId, SessionManager.DiscussionService.GetOrCreateDiscussionId);

            storyComments.DataBind();

            linkEdit.NavigateUrl = string.Format("AccountStoryEdit.aspx?id={0}", ts.Id);
            linkAddPhotos.NavigateUrl = string.Format("AccountStoryPicturesManage.aspx?id={0}", ts.Id);

            panelOwner.Visible = SessionManager.IsLoggedIn &&
                (SessionManager.IsAdministrator || ts.AccountId == SessionManager.Account.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Stories", Request, "AccountStoriesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ts.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }
}
