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
using Wilco.Web.UI;
using SnCore.SiteMap;

public partial class AcountDiscussionThreadsView : Page
{
    private DiscussionQueryOptions mOptions = null;

    public DiscussionQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new DiscussionQueryOptions();
                mOptions.AccountId = AccountId;
                mOptions.TopOfThreadOnly = TopOfThreads;
            }
            return mOptions;
        }
    }

    public bool TopOfThreads
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "TopOfThreads", false);
        }
        set
        {
            ViewState["TopOfThreads"] = value;
        }
    }

    public int AccountId
    {
        get
        {
            if (RequestId != 0)
                return RequestId;

            if (SessionManager.IsLoggedIn)
                return SessionManager.Account.Id;

            return 0;
        }
    }

    void GetData(object sender, EventArgs e)
    {
        discussionThreadView.CurrentPageIndex = 0;
        discussionThreadView.VirtualItemCount = SessionManager.GetCount<TransitDiscussionPost, DiscussionQueryOptions>(
            QueryOptions, SessionManager.DiscussionService.GetUserDiscussionThreadsCount);
        gridManage_OnGetDataSource(sender, e);
        discussionThreadView.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = discussionThreadView.CurrentPageIndex;
        options.PageSize = discussionThreadView.PageSize;
        discussionThreadView.DataSource = SessionManager.GetCollection<TransitDiscussionPost, DiscussionQueryOptions>(
            QueryOptions, options, SessionManager.DiscussionService.GetUserDiscussionThreads);
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionThreadView.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TopOfThreads = false;

            TransitAccount ta = SessionManager.GetInstance<TransitAccount, int>(
                AccountId, SessionManager.AccountService.GetAccountById);

            linkRelRss.Title = this.Title = labelHeader.Text = string.Format("{0}'s Discussion Posts", Renderer.Render(ta.Name));
            linkRelRss.NavigateUrl = string.Format("AccountDiscussionThreadsRss.aspx?id={0}&toplevel={1}", ta.Id, TopOfThreads);
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Foodies", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ta.Name, Request, string.Format("AccountView.aspx?id={0}", ta.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Discussion Posts", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        linkNewThreads.Text = TopOfThreads ? "&#187; Show All Posts" : "&#187; Show New Threads";
        base.OnPreRender(e);
    }

    public void linkNewThreads_Click(object sender, EventArgs e)
    {
        TopOfThreads = !TopOfThreads;
        GetData(sender, e);
    }
}
