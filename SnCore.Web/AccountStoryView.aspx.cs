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
        listPictures.OnGetDataSource += new EventHandler(listPictures_OnGetDataSource);
        if (!IsPostBack)
        {
            object[] s_args = { SessionManager.Ticket, RequestId };
            TransitAccountStory ts = SessionManager.GetCachedItem<TransitAccountStory>(
                SessionManager.StoryService, "GetAccountStoryById", s_args);

            object[] a_args = { ts.AccountId };
            TransitAccount acct = SessionManager.GetCachedItem<TransitAccount>(
                SessionManager.AccountService, "GetAccountById", a_args);

            licenseView.AccountId = acct.Id;

            this.Title = string.Format("{0}'s {1}", Renderer.Render(acct.Name), Renderer.Render(ts.Name));

            storyName.Text = Renderer.Render(ts.Name);
            storySummary.Text = RenderEx(ts.Summary);

            object[] p_args = { RequestId };
            listPictures.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                SessionManager.StoryService, "GetAccountStoryPicturesCountById", p_args);
            listPictures_OnGetDataSource(sender, e);

            object[] d_args = { RequestId };
            storyComments.DiscussionId = SessionManager.GetCachedCollectionCount(
                SessionManager.DiscussionService, "GetAccountStoryDiscussionId", d_args);

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

    void listPictures_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = listPictures.CurrentPageIndex;
        options.PageSize = listPictures.PageSize;
        object[] p_args = { RequestId, options };
        listPictures.DataSource = SessionManager.GetCachedCollection<TransitAccountStoryPicture>(
            SessionManager.StoryService, "GetAccountStoryPicturesById", p_args);
        listPictures.DataBind();
    }
}
