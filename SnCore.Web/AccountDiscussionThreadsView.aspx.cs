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
        object[] args = { QueryOptions };
        discussionThreadView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            DiscussionService, "GetUserDiscussionThreadsCount", args);
        gridManage_OnGetDataSource(sender, e);
        discussionThreadView.DataBind();
    }
 
    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = discussionThreadView.CurrentPageIndex;
            options.PageSize = discussionThreadView.PageSize;
            object[] args = { QueryOptions, options };
            discussionThreadView.DataSource = SessionManager.GetCachedCollection<TransitDiscussionPost>(
                DiscussionService, "GetUserDiscussionThreads", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        discussionThreadView.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        try
        {
            if (!IsPostBack)
            {
                TopOfThreads = false;

                object[] args = { AccountId };
                TransitAccount ta = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", args);

                linkAccount.Text = Renderer.Render(ta.Name);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ta.Id);
                linkRelRss.Title = this.Title = labelHeader.Text = string.Format("{0}'s Discussion Posts", Renderer.Render(ta.Name));                
                linkRelRss.NavigateUrl = string.Format("AccountDiscussionThreadsRss.aspx?id={0}&toplevel={1}", ta.Id, TopOfThreads);
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        linkNewThreads.Text = TopOfThreads ? "&#187; Show All Posts" : "&#187; Show New Threads";
        base.OnPreRender(e);
    }

    public void linkNewThreads_Click(object sender, EventArgs e)
    {
        try
        {
            TopOfThreads = ! TopOfThreads;
            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
